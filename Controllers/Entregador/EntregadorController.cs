using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Services.Entregador.Interfaces.EntregadorInterface;
using Models.EntregadorModel;
using MongoDB.Bson;
using Models.Entregadores.Requests.CreateEntregadorDTO;


[ApiController]
[Route("api/[controller]")]

public class EntregadorController : ControllerBase
{

    private readonly IEntregadorService _entregadorService;

    public EntregadorController(IEntregadorService entregadorService)
    {
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
            return CreatedAtAction(// Nome do m�todo de consulta
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
        // Valida��o b�sica do ID
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("ID inv�lido");

        var entregador = await _entregadorService.GetEntregadorByIdAsync(id);
    
        return entregador == null 
            ? NotFound() 
            : Ok(entregador);
    }

}