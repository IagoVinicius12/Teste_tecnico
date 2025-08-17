using MongoDB.Entities;
using Models.MotoModel;
using Services.Moto.Interfaces.IMotoService;
using Models.Motos.Requests.CreateMotoDTO;
using Models.Motos.Responses;
using Models.Motos.Requests.UpdateMotoPlateDTO;
using Models.Motos.Responses.MotoResponse;
using Models.RentalsModel;
using Services.Rentals.Interface.IRentalsService;
using Services.Kafka.Producer;

public class MotoService : IMotoService
{
    private readonly IRentalsService _rentalService;
    private readonly KafkaProducerService _kafkaProducerService;
    public MotoService(IRentalsService rentalService, KafkaProducerService kafkaProducerService)
    {
        _rentalService = rentalService;
        _kafkaProducerService = kafkaProducerService;
    }
    public async Task<MotoResponse> CreateMotoAsync(CreateMotoDTO motoDTO)
    {
        // metodo para cria�ao de uma moto
        try
        {
            var moto = new Moto
            {
                Identifier = motoDTO.identifier,
                Year = motoDTO.year,
                Plate = motoDTO.plate,
                Model = motoDTO.model,
            };
            await moto.SaveAsync();
            await _kafkaProducerService.PublishMotoCreatedAsync(moto);
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
        // metodo para buscar uma moto pelo ID
        try
        {
            var moto = await DB.Find<Moto>()
                           .Match(m => m.Identifier == id)
                           .ExecuteFirstAsync();
            if (moto == null)
            {
                return null;
            }
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
            throw new Exception("Error retrieving Moto by ID: " + ex.Message);
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
        // metodo para atualizar a placa de uma moto que foi erroneamente cadastrada
        try
        {
            var motoToUpdate = await DB.Find<Moto>()
                               .Match(m => m.Identifier == umotoDTO.identifier)
                               .ExecuteFirstAsync();
            if (motoToUpdate == null)
            {
                throw new Exception("Moto not found");
            }
            motoToUpdate.Plate = umotoDTO.plate;
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
            throw new Exception($"Failed to update Moto with plate {umotoDTO.plate}: " + ex.Message);
        }
    }

    public async Task<bool>DeleteMotoAsync(string id)
    {
        // metodo para deletar uma moto caso n�o haja nenhuma loca�ao associada a ela
        try
        {
            var moto=await DB.Find<Moto>()
                .Match(m=>m.Identifier==id)
                .ExecuteFirstAsync();
            if (moto == null)
            {
                throw new Exception("Moto not found");
            }

            var rental = await DB.Find<Rental>()
                .Match(l => l.MotoId == id)
                .ExecuteFirstAsync();
            if (rental != null)
            {
                throw new Exception("Cannot delete Moto, this id has a rent history");
            }
            await moto.DeleteAsync();

            return true;
        }
        catch(Exception ex)
        {
            throw new Exception("Failed to delete Moto" + ex.Message);
        }
    }

    public async Task<MotoResponse?>GetMotoByPlateAsync(string plate)
    {
        // metodo para buscar uma moto pela placa
        try
        {
            var moto = await DB.Find<Moto>()
                .Match(m => m.Plate == plate)
                .ExecuteFirstAsync();
            if (moto == null)
            {
                throw new Exception("Moto not found");
            }
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
            throw new Exception($"Error retrieving Moto with this plate {plate}: " + ex.Message);
        }
    }
}