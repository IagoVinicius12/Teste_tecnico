using MongoDB.Entities;
using Models.DeliveryPerson.Requests.CreateDeliveryPersonDTO;
using Models.DeliveryPersonModel;
using Services.DeliveryPerson.Interface.IDeliveryPersonService;
using Models.DeliveryPerson.Responses.DeliveryPersonResponse;
using Services.Auth.Interface.IAuthService;
using Models.AdminModel;
using MongoDB.Bson;
using Models.DeliveryPerson.Requests.UploadCnhDTO;

public class DeliveryPersonService : IDeliveryPersonService
{
    private readonly IAuthService _authService;
    public DeliveryPersonService(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<DeliveryPersonResponse> Create_deliveryPerson(CreateDeliveryPersonDTO deliveryPersonDTO)
    {
        //metodo para criar o delivery_person
        try
        {
            var existingEmailOnAdmin = await DB.Find<Admin>()
                .Match(a => a.Email == deliveryPersonDTO.email)
                .ExecuteFirstAsync();
            if (existingEmailOnAdmin != null)
            {
                throw new Exception("This email is already on use");
            }
                var delivery_person = new DeliveryPerson
            {
                Identifier = deliveryPersonDTO.identifier,
                Email = deliveryPersonDTO.email,
                Password = _authService.HashPassword(deliveryPersonDTO.password),
                Name = deliveryPersonDTO.name,
                Cnpj = deliveryPersonDTO.cnpj,
                Birthdate = deliveryPersonDTO.birthDate,
                CnhNumber = deliveryPersonDTO.cnhNumber,
                CnhType = deliveryPersonDTO.cnhType,
                CnhImagepath = string.Empty
            };

            await delivery_person.SaveAsync();

            return new DeliveryPersonResponse
            {
                id = delivery_person.ID,
                identifier = delivery_person.Identifier,
                email = delivery_person.Email,
                name = delivery_person.Name,
                cnpj = delivery_person.Cnpj,
                cnh = delivery_person.CnhNumber,
                cnhType = delivery_person.CnhType,
                birthdate = delivery_person.Birthdate,
                cnhPath = delivery_person.CnhImagepath
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating delivery_person: {ex.Message}");
        }
    }

    public async Task<DeliveryPersonResponse?> GetDeliveryPersonByIdAsync(string id)
    {
        //metodo para buscar o delivery_person pelo identificador
        try
        {
            var delivery_person = await DB.Find<DeliveryPerson>()
                           .Match(u => u.Identifier == id)
                           .ExecuteFirstAsync();
            if (delivery_person == null)
            {
                throw new Exception("DeliveryPerson not found");
            }

            return delivery_person == null
            ? null
            : new DeliveryPersonResponse
            {
                id = delivery_person.ID,
                identifier = delivery_person.Identifier,
                email = delivery_person.Email,
                name = delivery_person.Name,
                cnpj = delivery_person.Cnpj,
                cnh = delivery_person.CnhNumber,
                cnhType = delivery_person.CnhType,
                birthdate = delivery_person.Birthdate,
                cnhPath = delivery_person.CnhImagepath
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving delivery_person: {ex.Message}");
        }   
    }

    public async Task<List<DeliveryPersonResponse>> ListAllDeliveryPersons()
    {
        //funçao pra listar os delivery_persons
        try
        {
            var delivery_persons = await DB.Find<DeliveryPerson>().ExecuteAsync();

            return delivery_persons.Select(delivery_person => new DeliveryPersonResponse
            {
                id = delivery_person.ID, 
                identifier = delivery_person.Identifier,
                email = delivery_person.Email,
                name = delivery_person.Name,
                cnpj = delivery_person.Cnpj,
                cnh = delivery_person.CnhNumber,
                cnhType = delivery_person.CnhType,
                birthdate = delivery_person.Birthdate,
                cnhPath = delivery_person.CnhImagepath
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error listing all delivery_persones: {ex.Message}");
        }
    }

    public async Task<string>UploadCnhAsync(string id, UploadCnhDTO uploadCnhDTO, string ext)
    {
        //metodo para receber a imagem em base64 e salvar no disco do container, vai ficar em app/Storage/
        // somente para desenvolvimento o salvamento no disco, recomendado usar um serviço de armazenamento como AWS S3, Azure Blob Storage, etc.
        try
        {
            var imagemBytes = Convert.FromBase64String(uploadCnhDTO.Cnh_Image);

            var pastaUploads = Path.Combine(Directory.GetCurrentDirectory(), "Storage", $"CNH_{id}");// Pasta para armazenar as imagens de CNH com id para melhor organização
            if (!Directory.Exists(pastaUploads))
                Directory.CreateDirectory(pastaUploads);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(pastaUploads, fileName);

            await System.IO.File.WriteAllBytesAsync(filePath, imagemBytes);

            await DB.Update<DeliveryPerson>()
                .Match(e => e.Identifier == id)
                .Modify(e => e.CnhImagepath, filePath)
                .ExecuteAsync();

            return filePath;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error uploading CNH: {ex.Message}");
        }
    }

    public async Task<string> GetByID(string id)
    {
        try
        {
            var user = await DB.Find<DeliveryPerson>()
                .Match(e => e.ID == id)
                .ExecuteFirstAsync();
            if(user == null)
            {
                throw new KeyNotFoundException("Delivery Person not Found");
            }
            return user.Identifier;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving delivery_person: {ex.Message}");
        }
    }
}
