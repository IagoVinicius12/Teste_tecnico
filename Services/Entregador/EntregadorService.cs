using MongoDB.Entities;
using Models.Entregadores.Requests.CreateEntregadorDTO;
using Models.EntregadorModel;
using Services.Entregador.Interface.IEntregadorService;
using Models.Entregadores.Responses.EntregadorResponse;
using Services.Auth.Interface.IAuthService;
using Models.AdminModel;
using MongoDB.Bson;
using Models.Entregadores.Requests.UploadCnhDTO;    

public class EntregadorService : IEntregadorService
{
    private readonly IAuthService _authService;
    public EntregadorService(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<EntregadorResponse> Create_entregador(CreateEntregadorDTO entregadorDTO)
    {
        //metodo para criar o entregador
        try
        {
            var existingEmailOnAdmin = await DB.Find<Admin>()
                .Match(a => a.Email == entregadorDTO.email)
                .ExecuteFirstAsync();
            if (existingEmailOnAdmin != null)
            {
                throw new Exception("This email is already on use");
            }
                var entregador = new Entregador
            {
                Identifier = entregadorDTO.identificador,
                Email = entregadorDTO.email,
                Password = _authService.HashPassword(entregadorDTO.password),
                Name = entregadorDTO.nome,
                Cnpj = entregadorDTO.cnpj,
                Birthdate = entregadorDTO.data_nascimento,
                CnhNumber = entregadorDTO.numero_cnh,
                CnhType = entregadorDTO.tipo_cnh,
                CnhImagepath = string.Empty
            };

            await entregador.SaveAsync();

            return new EntregadorResponse
            {
                id = entregador.ID,
                identifier = entregador.Identifier,
                email = entregador.Email,
                name = entregador.Name,
                cnpj = entregador.Cnpj,
                cnh = entregador.CnhNumber,
                cnhType = entregador.CnhType,
                birthdate = entregador.Birthdate,
                cnhPath = entregador.CnhImagepath
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating entregador: {ex.Message}");
        }
    }

    public async Task<EntregadorResponse?> GetEntregadorByIdAsync(string id)
    {
        //metodo para buscar o entregador pelo identificador
        try
        {
            var entregador = await DB.Find<Entregador>()
                           .Match(u => u.Identifier == id)
                           .ExecuteFirstAsync();
            if (entregador == null)
            {
                throw new Exception("Entregador not found");
            }

            return entregador == null
            ? null
            : new EntregadorResponse
            {
                id = entregador.ID,
                identifier = entregador.Identifier,
                email = entregador.Email,
                name = entregador.Name,
                cnpj = entregador.Cnpj,
                cnh = entregador.CnhNumber,
                cnhType = entregador.CnhType,
                birthdate = entregador.Birthdate,
                cnhPath = entregador.CnhImagepath
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving entregador: {ex.Message}");
        }   
    }

    public async Task<List<EntregadorResponse>> ListAllEntregadores()
    {
        //funçao pra listar os entregadores
        try
        {
            var entregadors = await DB.Find<Entregador>().ExecuteAsync();

            return entregadors.Select(entregador => new EntregadorResponse
            {
                id = entregador.ID, 
                identifier = entregador.Identifier,
                email = entregador.Email,
                name = entregador.Name,
                cnpj = entregador.Cnpj,
                cnh = entregador.CnhNumber,
                cnhType = entregador.CnhType,
                birthdate = entregador.Birthdate,
                cnhPath = entregador.CnhImagepath
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error listing all entregadores: {ex.Message}");
        }
    }

    public async Task<string>UploadCnhAsync(string id, UploadCnhDTO uploadCnhDTO, string ext)
    {
        //metodo para receber a imagem em base64 e salvar no disco do container, vai ficar em app/Storage/
        // somente para desenvolvimento o salvamento no disco, recomendado usar um serviço de armazenamento como AWS S3, Azure Blob Storage, etc.
        try
        {
            var imagemBytes = Convert.FromBase64String(uploadCnhDTO.Imagem_Cnh);

            var pastaUploads = Path.Combine(Directory.GetCurrentDirectory(), "Storage", $"CNH_{id}");// Pasta para armazenar as imagens de CNH com id para melhor organização
            if (!Directory.Exists(pastaUploads))
                Directory.CreateDirectory(pastaUploads);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(pastaUploads, fileName);

            await System.IO.File.WriteAllBytesAsync(filePath, imagemBytes);

            await DB.Update<Entregador>()
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
}
