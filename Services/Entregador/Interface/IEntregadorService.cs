using System.Threading.Tasks;
using Models.Entregadores.Requests.CreateEntregadorDTO;
using Models.Entregadores.Responses.EntregadorResponse;
using Models.Entregadores.Requests.UploadCnhDTO;	

namespace Services.Entregador.Interface.IEntregadorService;

public interface IEntregadorService
{
	Task<EntregadorResponse> Create_entregador(CreateEntregadorDTO userDTO);
	Task<EntregadorResponse?> GetEntregadorByIdAsync(string id);
	Task<List<EntregadorResponse>> ListAllEntregadores();	
	Task<string> UploadCnhAsync(string id, UploadCnhDTO uploadCnhDTO,string ext);
}