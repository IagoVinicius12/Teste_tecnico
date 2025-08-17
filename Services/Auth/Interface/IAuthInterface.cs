using Models.Auth.Requests.LoginDTO;
using System.Threading.Tasks;

namespace Services.Auth.Interface.IAuthService;

public interface IAuthService
{
    string HashPassword(string password);
    bool VerifyPassword(string inputPassword, string storedHash);
    Task<string> GenerateTokenAsync(LoginDTO loginDTO);
}