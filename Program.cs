using MongoDB.Driver;
using MongoDB.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Entregador.Interface.IEntregadorService;
using Models.EntregadorModel;
using Models.MotoModel;
using Services.Moto.Interfaces.IMotoService;
using Models.LocacoesModel;
using Services.Locacoes.Interface.ILocacoesInterface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Services.Auth.Interface.IAuthService;
using Services.Admin.Interface.IAdminService;
using System.Text;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using Services.Kafka.Consumer;
using Services.Kafka.Producer;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração Básica
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Sua API", Version = "v1" });

    // Configuração do Bearer Token no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite 'Bearer' seguido do token JWT.\nExemplo: Bearer 12345abcdef"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// 2. Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 3. Configuração do MongoDB.Entities
await DB.InitAsync(
    builder.Configuration["MongoDBSettings:DatabaseName"],
    MongoClientSettings.FromConnectionString(
        builder.Configuration["MongoDBSettings:ConnectionString"]
    )
);

// 4. Índices
await DB.Index<Entregador>()
    .Key(e => e.Identifier, KeyType.Ascending)
    .Option(o => o.Unique = true)
    .CreateAsync();

await DB.Index<Entregador>()
    .Key(e => e.Cnpj, KeyType.Ascending)
    .Option(o => o.Unique = true)
    .CreateAsync();

await DB.Index<Entregador>()
    .Key(e => e.CnhNumber, KeyType.Ascending)
    .Option(o => o.Unique = true)
    .CreateAsync();

await DB.Index<Moto>()
    .Key(m => m.Identifier, KeyType.Ascending)
    .Option(o => o.Unique = true)
    .CreateAsync();

await DB.Index<Moto>()
    .Key(m => m.Plate, KeyType.Ascending)
    .Option(o => o.Unique = true)
    .CreateAsync();

// 5. Registro dos serviços
builder.Services.AddScoped<IEntregadorService, EntregadorService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<ILocacoesService, LocacoesService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddSingleton<KafkaProducerService>();
builder.Services.AddHostedService<KafkaConsumerService>();

var jwtSettings = builder.Configuration.GetSection("Jwt");

if (string.IsNullOrEmpty(jwtSettings["Secret"]))
    throw new ApplicationException("Chave JWT não configurada no appsettings.json");
if (string.IsNullOrEmpty(jwtSettings["Issuer"]))
    throw new ApplicationException("Issuer JWT não configurado");
if (string.IsNullOrEmpty(jwtSettings["Audience"]))
    throw new ApplicationException("Audience JWT não configurada");

// 6. Configuração JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),

            // RoleClaimType padrão é ClaimTypes.Role, já que você está usando:
            // new Claim(ClaimTypes.Role, "Admin")
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"));
});

var app = builder.Build();

// 7. Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sua API v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");

// **Ordem correta: primeiro autenticação, depois autorização**
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Recebida requisição: {context.Request.Method} {context.Request.Path}");
    await next();
});

app.MapControllers();
app.Run();
