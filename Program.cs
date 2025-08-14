using MongoDB.Driver;
using MongoDB.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Entregador.Interfaces.EntregadorInterface;
using Models.EntregadorModel;
using Models.MotoModel;
using Services.Moto.Interfaces.IMotoService;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração Básica
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Sua API", Version = "v1" });
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

// 4.Indices
// Criar índice único para Entregador
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

// Criando Indices para motos
await DB.Index<Moto>()
    .Key(m => m.Identifier, KeyType.Ascending)
    .Option(o => o.Unique = true)
    .CreateAsync();
await DB.Index<Moto>()
    .Key(m =>m.Plate, KeyType.Ascending)
    .Option(o=>o.Unique=true)
    .CreateAsync();


// 5. Registro dos Serviços
builder.Services.AddScoped<IEntregadorService, EntregadorService>();
builder.Services.AddScoped<IMotoService, MotoService>();

var app = builder.Build();

// 6. Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sua API v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Recebida requisição: {context.Request.Method} {context.Request.Path}");
    await next();
});

app.MapControllers();
app.Run();
