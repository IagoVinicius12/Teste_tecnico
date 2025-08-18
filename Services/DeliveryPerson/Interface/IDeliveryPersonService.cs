using System.Threading.Tasks;
using Models.DeliveryPerson.Requests.CreateDeliveryPersonDTO;
using Models.DeliveryPerson.Responses.DeliveryPersonResponse;
using Models.DeliveryPerson.Requests.UploadCnhDTO;	

namespace Services.DeliveryPerson.Interface.IDeliveryPersonService;

public interface IDeliveryPersonService
{
	Task<DeliveryPersonResponse> Create_deliveryPerson(CreateDeliveryPersonDTO deliveryPersonDTO);
	Task<DeliveryPersonResponse?> GetDeliveryPersonByIdAsync(string id);
	Task<List<DeliveryPersonResponse>> ListAllDeliveryPersons();	
	Task<string> UploadCnhAsync(string id, UploadCnhDTO uploadCnhDTO,string ext);
	Task<string> GetByID(string id);
}