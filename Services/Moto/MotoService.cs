using MongoDB.Entities;
using Models.MotoModel;
using Services.Moto.Interfaces.IMotoService;
using Models.Motos.Requests.CreateMotoDTO;
using Models.Motos.Responses;
using Models.Motos.Requests.UpdateMotoPlateDTO;
using Models.Motos.Responses.MotoResponse;

public class MotoService : IMotoService
{
    public MotoService()
    {
        // Construtor vazio, não precisa injetar nada
    }
    public async Task<MotoResponse> CreateMotoAsync(CreateMotoDTO motoDTO)
    {
        var moto = new Moto
        {
            Identifier = motoDTO.identificador,
            Year = motoDTO.ano,
            Plate = motoDTO.placa,
            Model = motoDTO.modelo,
        };
        await moto.SaveAsync(); 
        return new MotoResponse
        {
            Id = moto.ID,
            Identifier = moto.Identifier,
            Plate = moto.Plate,
            Model = moto.Model,
            Year = moto.Year,
        };
    }
    public async Task<MotoResponse?> GetMotoByIdAsync(string id)
    {
        try
        {
            var moto = new Moto
            {
                Identifier = motoDTO.identificador,
                Year = motoDTO.ano,
                Plate = motoDTO.placa,
                Model = motoDTO.modelo,
            };
            await moto.SaveAsync();
            return new MotoResponse
            {
                Id = moto.ID,
                Identifier = moto.Identifier,
                Plate = moto.Plate,
                Model = moto.Model,
                Year = moto.Year,
            };
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create a new Moto " + ex.Message);
        }
    }
    public async Task<MotoResponse?> GetMotoByIdAsync(string id)
    {
        try
        {
            var moto = await DB.Find<Moto>()
                           .Match(m => m.Identifier == id)
                           .ExecuteFirstAsync();
            return moto == null
                ? null
                : new MotoResponse
                {
                    Id = moto.ID,
                    Identifier = moto.Identifier,
                    Plate = moto.Plate,
                    Model = moto.Model,
                    Year = moto.Year,
                };
        }
        catch(Exception ex)
        {
            throw new Exception("Moto not found"+ex.Message);
        }
    }
    public async Task<List<MotoResponse>> ListAllMotosAsync()
    {
        var motos = await DB.Find<Moto>().ExecuteAsync();
        return motos.Select(m => new MotoResponse
        {
            Id = m.ID, 
            Identifier = m.Identifier,
            Plate = m.Plate,
            Model = m.Model,
            Year = m.Year,
        }).ToList();
    }
    public async Task<MotoResponse?> UpdateMotoPlate(UpdateMotoPlateDTO umotoDTO)
    {
        try
        {
            var motoToUpdate = await DB.Find<Moto>()
                               .Match(m => m.Identifier == umotoDTO.identificador)
                               .ExecuteFirstAsync();
            if (motoToUpdate == null)
            {
                throw new Exception("Moto not found");
            }
            motoToUpdate.Plate = umotoDTO.placa;
            await motoToUpdate.SaveAsync();
            return new MotoResponse
            {
                Id = motoToUpdate.ID,
                Identifier = motoToUpdate.Identifier,
                Plate = motoToUpdate.Plate,
                Model = motoToUpdate.Model,
                Year = motoToUpdate.Year,
            };
        }
        catch(Exception ex)
        {
            throw new Exception("Erro ao atualizar a moto!!");
        }
    }
}