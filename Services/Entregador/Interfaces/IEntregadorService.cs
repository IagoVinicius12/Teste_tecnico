using System.Threading.Tasks;
using Models.Entregadores.Requests.CreateEntregadorDTO;
using Models.Entregadores.Responses.EntregadorResponse;

namespace Services.Entregador.Interfaces.EntregadorInterface;

public interface IEntregadorService
{
	Task<EntregadorResponse> Create_entregador(CreateEntregadorDTO userDTO);
	Task<EntregadorResponse?> GetEntregadorByIdAsync(string id);
	Task<List<EntregadorResponse>> ListAllEntregadores();	
}