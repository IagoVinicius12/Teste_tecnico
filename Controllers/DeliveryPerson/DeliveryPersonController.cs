using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Services.DeliveryPerson.Interface.IDeliveryPersonService;
using Models.DeliveryPersonModel;
using MongoDB.Bson;
using Models.DeliveryPerson.Requests.CreateDeliveryPersonDTO;
using Models.DeliveryPerson.Requests.UploadCnhDTO;
using Swashbuckle.AspNetCore.Annotations;
using Models.DeliveryPerson.Responses.DeliveryPersonResponse;


[ApiController]
[Route("deliveryperson")]

public class DeliveryPersonController : ControllerBase
{

    private readonly IWebHostEnvironment _env;
    private readonly IDeliveryPersonService _deliveryPersonService;

    public DeliveryPersonController(IDeliveryPersonService deliveryPersonService, IWebHostEnvironment env)
    {
        _env = env;
        _deliveryPersonService = deliveryPersonService;
    }

    [HttpPost("create")]
    [SwaggerOperation(Summary="Cria um novo deliveryPerson", Description="Cria um novo deliveryPerson caso não exista com o mesmo Email")]
    [SwaggerResponse(201, "DeliveryPerson criado com sucesso!", typeof(DeliveryPersonResponse))]
    public async Task<IActionResult> Create_deliveryPerson([FromBody] CreateDeliveryPersonDTO deliveryPersonDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var deliveryPersonResponse = await _deliveryPersonService.Create_deliveryPerson(deliveryPersonDTO);
            return CreatedAtAction(
                    nameof(GetDeliveryPerson), 
                    new { id = deliveryPersonResponse.id },
                    deliveryPersonResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("list")]
    [SwaggerOperation(Summary = "Lista todos os deliveryPerson", Description = "Retorna uma lista de todos os deliveryPerson cadastrados.")]
    [SwaggerResponse(200, "Lista de deliveryPersones retornada com sucesso.", typeof(List<DeliveryPersonResponse>))]
    public async Task<IActionResult> GetDeliveryPerson()
    {
        var deliveryPersons = await _deliveryPersonService.ListAllDeliveryPersons();
        return deliveryPersons == null
            ? NotFound()
            : Ok(deliveryPersons);
    }

    [HttpGet("get/{id}")]
    [SwaggerOperation(Summary = "Obtem um deliveryPerson pelo Identificador", Description = "Retorna os detalhes de um deliveryPerson específico pelo seu Identificador.")]
    [SwaggerResponse(200, "DeliveryPerson encontrado com sucesso.", typeof(DeliveryPersonResponse))]
    public async Task<IActionResult> GetDeliveryPerson(string id)
    {
        // Validação básica do ID
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Identificador inválido");

        var deliveryPerson = await _deliveryPersonService.GetDeliveryPersonByIdAsync(id);
    
        return deliveryPerson == null 
            ? NotFound() 
            : Ok(deliveryPerson);
    }

    [HttpPost("upload_cnh/{id}")]
    [SwaggerOperation(Summary= "Faz o upload da CNH do deliveryPerson", Description = "Permite que um deliveryPerson envie sua CNH em formato Base64. Apenas arquivos PNG ou BMP sao permitidos.")]
    [SwaggerResponse(200, "CNH salva com sucesso")]
    public async Task<IActionResult> UploadCnhAsync(string id, UploadCnhDTO uploadCnhDTO)
    {

        if (string.IsNullOrEmpty(uploadCnhDTO.Cnh_Image))
            return BadRequest("Imagem não enviada.");

        string base64 = uploadCnhDTO.Cnh_Image;

        if (base64.Contains(","))
            base64 = base64.Substring(base64.IndexOf(",") + 1);

        byte[] bytes;
        try
        {
            bytes = Convert.FromBase64String(base64);
        }
        catch
        {
            return BadRequest("Imagem inválida (não está em Base64).");
        }

        string ext;

        // PNG -> começa com 89 50 4E 47
        if (bytes.Take(4).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47 }))
            ext = ".png";
        // BMP -> começa com 42 4D
        else if (bytes.Take(2).SequenceEqual(new byte[] { 0x42, 0x4D }))
            ext = ".bmp";
        else
            return BadRequest("Apenas arquivos PNG ou BMP são permitidos.");

        try
        {
            var filePath = await _deliveryPersonService.UploadCnhAsync(id,uploadCnhDTO,ext);

            return Ok(new
            {
                Message = "CNH salva com sucesso!",
                FilePath = filePath
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

}