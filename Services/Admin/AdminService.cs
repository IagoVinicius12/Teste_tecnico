using System.Threading.Tasks;
using Models.Admin.Requests.CreateAdminDTO;
using Models.Admin.Responses.AdminResponse;
using Services.Admin.Interface.IAdminService;
using Services.Auth.Interface.IAuthService;
using MongoDB.Entities;
using Models.AdminModel;

public class AdminService : IAdminService
{
    private readonly IAuthService _authService;
    public AdminService(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<AdminResponse> CreateAdminAsync(CreateAdminDTO adminDTO)
    {
        //metodo para criaçao de um admin
        var admin = new Admin
        {
            Name = adminDTO.nome,
            Email = adminDTO.email,
            Password = _authService.HashPassword(adminDTO.senha),
        };
        await admin.SaveAsync();
        return new AdminResponse
        {
            id = admin.ID,
            name = admin.Name,
            email = admin.Email,
            role=admin.Role,
        };
    }

    public async Task<AdminResponse?> GetAdminById(string id)
    {
        //metodo para buscar um admin pelo ID
        var admin = await DB.Find<Admin>()
                            .Match(u => u.ID == id)
                            .ExecuteFirstAsync();
        return admin == null
            ? null
            : new AdminResponse
            {
                id = admin.ID,
                name = admin.Name,
                email = admin.Email,
                role = admin.Role,
            };
    }
}