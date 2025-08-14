using MongoDB.Entities;
using Models.Entregadores.Requests.CreateEntregadorDTO;
using Models.EntregadorModel;
using Services.Entregador.Interfaces.EntregadorInterface;
using Models.Entregadores.Responses.EntregadorResponse;
using MongoDB.Bson;

public class EntregadorService : IEntregadorService
{
    public EntregadorService()
    {
        // Construtor vazio, não precisa injetar nada
    }

    public async Task<EntregadorResponse> Create_entregador(CreateEntregadorDTO entregadorDTO)
    {
        var entregador = new Entregador
        {
            Identifier = entregadorDTO.identificador,
            Name = entregadorDTO.nome,
            Cnpj = entregadorDTO.cnpj,
            Birthdate = entregadorDTO.data_nascimento,
            CnhNumber = entregadorDTO.numero_cnh,
            CnhType = entregadorDTO.tipo_cnh,
            CnhImage = string.Empty
        };

        await entregador.SaveAsync(); // Salva usando MongoDB.Entities

        return new EntregadorResponse
        {
            id = entregador.ID, // propriedade do Entity
            identifier=entregador.Identifier,
            name = entregador.Name,
            cnpj=entregador.Cnpj,
            cnh=entregador.CnhNumber,
            cnhType=entregador.CnhType,
            birthdate = entregador.Birthdate
        };
    }

    public async Task<EntregadorResponse?> GetEntregadorByIdAsync(string id)
    {
        var entregador = await DB.Find<Entregador>()
                           .Match(u => u.Identifier == id)
                           .ExecuteFirstAsync();

        return entregador == null
            ? null
            : new EntregadorResponse
            {
                id = entregador.ID, // propriedade do Entity
                identifier = entregador.Identifier,
                name = entregador.Name,
                cnpj = entregador.Cnpj,
                cnh = entregador.CnhNumber,
                cnhType = entregador.CnhType,
                birthdate = entregador.Birthdate
            };
    }

    public async Task<List<EntregadorResponse>> ListAllEntregadores()
    {
        var entregadors = await DB.Find<Entregador>().ExecuteAsync();

        return entregadors.Select(entregador => new EntregadorResponse
        {
            id = entregador.ID, // propriedade do Entity
            identifier = entregador.Identifier,
            name = entregador.Name,
            cnpj = entregador.Cnpj,
            cnh = entregador.CnhNumber,
            cnhType = entregador.CnhType,
            birthdate = entregador.Birthdate
        }).ToList();
    }
}
