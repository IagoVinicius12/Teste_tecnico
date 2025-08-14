using System.Threading.Tasks;
using Models.Motos.Requests.CreateMotoDTO;
using Models.Motos.Responses.MotoResponse;
using Models.Motos.Requests.UpdateMotoPlateDTO;
namespace Services.Moto.Interfaces.IMotoService;
public interface IMotoService
{
    Task<MotoResponse> CreateMotoAsync(CreateMotoDTO motoDTO);
    Task<MotoResponse?> GetMotoByIdAsync(string id);
    Task<List<MotoResponse>> ListAllMotosAsync();
    Task<MotoResponse?> UpdateMotoPlate(UpdateMotoPlateDTO umotoDTO);
}