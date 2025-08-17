using Models.Admin.Requests.CreateAdminDTO;
using System.Threading.Tasks;
using Models.Admin.Responses.AdminResponse;	

namespace Services.Admin.Interface.IAdminService;

public interface IAdminService
{
	Task<AdminResponse> CreateAdminAsync(CreateAdminDTO adminDTO);
	Task<AdminResponse?> GetAdminById(string id);
}