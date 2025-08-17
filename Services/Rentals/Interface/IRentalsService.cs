using System.Threading.Tasks;
using Models.Rentals.Requests.CreateRentalDTO;
using Models.Rentals.Responses.RentalResponse;
using Models.Rentals.Requests.DevolutionRentalDTO;
using Models.Rentals.Responses.DevolutionRentalResponse;

namespace Services.Rentals.Interface.IRentalsService;

public interface IRentalsService
{
    Task<RentalResponse> CreateRentalAsync(CreateRentalDTO rentalDTO);
    Task<RentalResponse> GetRentalById(string id);
    Task<DevolutionResponse> UpdateDevolutionDate(string id, DevolutionRentalDTO devolutionRentalDTO);
}