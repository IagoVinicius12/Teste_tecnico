using System.Threading.Tasks;
using Models.Locacoes.Requests.CreateLocacaoDTO;
using Models.Locacoes.Responses.LocacaoResponse;
using Models.Locacoes.Requests.DevolucaoLocacaoDTO;
using Models.Locacoes.Responses.DevolucaoLocacaoResponse;

namespace Services.Locacoes.Interface.ILocacoesInterface;

public interface ILocacoesService
{
    Task<LocacaoResponse> CreateLocacaoAsync(CreateLocacaoDTO locacaoDTO);
    Task<LocacaoResponse> GetLocacaoById(string id);
    Task<DevolucaoLocacaoResponse> UpdateDevolutionDate(string id, DevolucaoLocacaoDTO devolucaoLocacaoDTO);
}