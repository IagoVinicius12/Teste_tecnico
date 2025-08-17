using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Services.Entregador.Interface.IEntregadorService;
using Models.EntregadorModel;
using MongoDB.Bson;
using Models.Entregadores.Requests.CreateEntregadorDTO;
using Models.Entregadores.Requests.UploadCnhDTO;


[ApiController]
[Route("api/[controller]")]

public class EntregadorController : ControllerBase
{

    private readonly IWebHostEnvironment _env;
    private readonly IEntregadorService _entregadorService;

    public EntregadorController(IEntregadorService entregadorService, IWebHostEnvironment env)
    {
        _env = env;
        _entregadorService = entregadorService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create_entregador([FromBody] CreateEntregadorDTO entregadorDTO)
    {
        Console.WriteLine("Recebido POST em /api/entregador/create");
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var entregadorResponse = await _entregadorService.Create_entregador(entregadorDTO);
            return CreatedAtAction(// Nome do método de consulta
                    nameof(GetEntregador), // Agora existe
                    new { id = entregadorResponse.id },
                    entregadorResponse);// Corpo da resposta
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetEntregadores()
    {
        var entregadors = await _entregadorService.ListAllEntregadores();
        return entregadors == null
            ? NotFound()
            : Ok(entregadors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEntregador(string id)
    {
        // Validação básica do ID
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("ID inválido");

        var entregador = await _entregadorService.GetEntregadorByIdAsync(id);
    
        return entregador == null 
            ? NotFound() 
            : Ok(entregador);
    }

    [HttpPost("upload_cnh/{id}")]
    public async Task<IActionResult> UploadCnh(string id, UploadCnhDTO uploadCnhDTO)
    {

        if (string.IsNullOrEmpty(uploadCnhDTO.Imagem_Cnh))
            return BadRequest("Imagem não enviada.");

        string base64 = uploadCnhDTO.Imagem_Cnh;

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
            var filePath = await _entregadorService.UploadCnhAsync(id,uploadCnhDTO,ext);

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