using System.Threading.Tasks;
using MongoDB.Entities;
using Models.LocacoesModel;
using Services.Moto.Interfaces.IMotoService;
using Services.Entregador.Interface.IEntregadorService;
using Models.Locacoes.Requests.CreateLocacaoDTO;
using Models.Locacoes.Requests.DevolucaoLocacaoDTO;
using Models.Locacoes.Responses.LocacaoResponse;
using Services.Locacoes.Interface.ILocacoesInterface;
using Models.Locacoes.Responses.DevolucaoLocacaoResponse;
using Models.EntregadorModel;
using Models.MotoModel;

public class LocacoesService : ILocacoesService
{
    public LocacoesService()
    {
    }

    public async Task<LocacaoResponse> CreateLocacaoAsync(CreateLocacaoDTO locacaoDTO)
    {
        // metodo para criar uma locacao
        try
        {
            var entregador= await DB.Find<Entregador>()
                .Match(e=>e.Identifier==locacaoDTO.entregador_id)
                .ExecuteFirstAsync();
            if (entregador == null)//confirmaçao da existencia do entregador
            {
                throw new Exception("Entregador not found");
            }
            var moto=await DB.Find<Moto>()
                .Match(moto=>moto.Identifier==locacaoDTO.moto_id)
                .ExecuteFirstAsync();
            if (moto == null)// confirmaçao da existencia da moto
            {
                throw new Exception("Moto not found");
            }

            var plan_price = new Dictionary <int, float>
            {
                { 7, 30.00f },
                { 15, 28.00f },
                { 30, 22.00f },
                { 45, 20.00f },
                { 50, 18.00f }
            };// esse dicionario foi feito para que no momento que o plano fosse inserido como entrada o valor da diaria fosse retornado

            var locacao = new Locacao
            {
                Identifier = locacaoDTO.identificador,
                EntregadorId = entregador.Identifier,
                MotoId = moto.Identifier,
                StartDate = locacaoDTO.data_inicio,
                EndDate = locacaoDTO.data_termino,
                DevolutionPrevisionDate = locacaoDTO.data_previsao_termino,
                DevolutionDate = null,
                PlanType = locacaoDTO.plano,
                DailyPrice = plan_price[locacaoDTO.plano]// input do plano e retorno do valor da diaria
            };

            await locacao.SaveAsync();

            return new LocacaoResponse
            {
                id = locacao.ID,
                identificador = locacao.Identifier,
                entregador_id = locacao.EntregadorId,
                moto_id = locacao.MotoId,
                data_inicio = locacao.StartDate,
                data_termino = locacao.EndDate,
                data_previsao_termino = locacao.DevolutionPrevisionDate,
                dailyPrice = locacao.DailyPrice,
                plano = locacao.PlanType,
                totalPrice=locacao.TotalPrice
            };
        }
        catch(Exception ex)
        {
            throw new Exception($"Error creating locacao: {ex.Message}");
        }
    }
    public async Task<LocacaoResponse> GetLocacaoById(string id)
    {
        // O método faz a consulta no banco para procurar a locacao que possui esse identificador
        try
        {
            var locacao=await DB.Find<Locacao>()
                .Match(l=>l.Identifier==id)
                .ExecuteFirstAsync();
            return locacao == null
                ? throw new Exception("Locacao not found")
                : new LocacaoResponse
                {
                    id = locacao.ID,
                    identificador = locacao.Identifier,
                    entregador_id = locacao.EntregadorId,
                    moto_id = locacao.MotoId,
                    data_inicio = locacao.StartDate,
                    data_termino = locacao.EndDate,
                    data_previsao_termino = locacao.DevolutionPrevisionDate,
                    dailyPrice = locacao.DailyPrice,
                    plano = locacao.PlanType,
                    totalPrice= locacao.TotalPrice
                };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving locacao by id: {ex.Message}");
        }
    }
    public async Task<DevolucaoLocacaoResponse> UpdateDevolutionDate(string id, DevolucaoLocacaoDTO devolucaoLocacaoDTO)
    {
        //esse método é feito para que quando a pessoa informar a data de devolução ser feito o calculo do total a ser pago
        try
        {
            var locacao = await DB.Find<Locacao>()
                .Match(l => l.Identifier == id)
                .ExecuteFirstAsync();

            if (locacao == null)
            {
                throw new Exception("Locacao not found");
            }

            locacao.DevolutionDate = devolucaoLocacaoDTO.data_devolucao;
            if (locacao.DevolutionDate.HasValue)
            {
                DateTime prevision= locacao.DevolutionPrevisionDate.Date;
                DateTime devolution = locacao.DevolutionDate.Value.Date;

                if (devolution < prevision)
                {
                    // caso a devolução seja antecipada:
                    int antecipationDays= (prevision - devolution).Days;
                    locacao.TotalPrice = (locacao.DailyPrice * Math.Abs(antecipationDays - locacao.PlanType)) + (locacao.DailyPrice * (locacao.PlanType == 7 ? 0.2f : 0.4f) * antecipationDays);
                    Console.WriteLine($"Dias de adiantamento {antecipationDays} R${locacao.TotalPrice}");
                    // A lógica usada é simplesmente olhar o dia que está sendo devolvido ignorando as horas
                    // (não foi informado quanto tempo seria um atraso então deixei somente atrasos de dia na data ex:data da devolução 21/08/2025 e data prevista 22/08/2025 = 1 dia de atraso, desconsiderando as horas)
                    // não foi informado se havia alguma taxa no plano de 30 dias ou mais então mantive os 40%
                }
                else if (devolution > prevision)
                {
                    // mesma lógica acima, porém mudando os valores, diaria atrasada +50 reais
                    int delayDays = (devolution - prevision).Days;
                    locacao.TotalPrice = (locacao.DailyPrice * locacao.PlanType) + (delayDays * 50.00f);
                    Console.WriteLine($"Dias de atraso {delayDays} R${locacao.TotalPrice}");
                }
                else
                {
                    locacao.TotalPrice = locacao.DailyPrice * locacao.PlanType;
                    Console.WriteLine($"No delay. Total Price: R${locacao.TotalPrice}");
                    // entregue no mesmo dia da previsão
                }
            }

            await locacao.SaveAsync();
            return new DevolucaoLocacaoResponse
            {
                TotalPrice=locacao.TotalPrice,
                DevolutionDate = locacao.DevolutionDate
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating devolution date: {ex.Message}");
        }
    }
}