using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using MongoDB.Entities;
using System.Threading.Tasks;
using Services.Auth.Interface.IAuthService;
using Models.Auth.Requests.LoginDTO;
using Models.DeliveryPersonModel;
using Models.AdminModel;
using Services.Admin.Interface.IAdminService;
using Services.DeliveryPerson.Interface.IDeliveryPersonService;



public class AuthService:IAuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public string HashPassword(string password)
    {
        // Usado no cadastro de usu�rios, serve para criptografar a senha, pode ser usado tanto para o admin quanto para o entregador
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        // Usado na rota de login, somente verifica se a senha informada � igual a senha criptografada salva no banco de dados
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
    
    public async Task<string> GenerateTokenAsync(LoginDTO loginDTO)
    {
        // metodo para gerar o token JWT a partir do payload sendo esse o ID gerado pelo MongoDB, o email e o role do usu�rio(Admin ou DeliveryPerson)
        var jwtSecret = _config["Jwt:Secret"] ??
            throw new ArgumentNullException("Jwt:Secret n�o configurado");


        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]); // Chave no appsettings.json

        var admin= await DB.Find<Admin>()
            .Match(a=> a.Email == loginDTO.Email)
            .ExecuteFirstAsync();

        string userId;
        string userRole;
        if (admin!= null)
        {
            userId = admin.ID;
            userRole = admin.Role;
            var isPasswordValid = VerifyPassword(loginDTO.Password, admin.Password);
            if( !isPasswordValid)
            {
                throw new UnauthorizedAccessException("Senha inv�lida");
            }
        }
        else
        {
            var entregador = await DB.Find<DeliveryPerson>()
                .Match(e => e.Email == loginDTO.Email)
                .ExecuteFirstAsync();
            if (entregador != null)
            {
                userId = entregador.ID;
                userRole = entregador.Role;
                var isPasswordValid = VerifyPassword(loginDTO.Password, entregador.Password);
                if (!isPasswordValid)
                {
                    throw new UnauthorizedAccessException("Senha inv�lida");
                }
            }
            else
            {
                throw new UnauthorizedAccessException("Usu�rio n�o encontrado");
            }
        }
        //Aqui � definido o payload do token, onde s�o adicionados os claims que ser�o utilizados para identificar o usu�rio
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, loginDTO.Email),
            new Claim(ClaimTypes.Role, userRole)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            //Os par�metros issuer e audience s�o definidas no appsettings.json
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}