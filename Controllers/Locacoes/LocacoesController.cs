using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Services.Locacoes.Interface.ILocacoesInterface;
using Models.LocacoesModel;
using MongoDB.Bson;
using Models.Locacoes.Requests.CreateLocacaoDTO;
using Models.Locacoes.Requests.DevolucaoLocacaoDTO;

[ApiController]
[Route("api/[controller]")]

public class LocacoesController : ControllerBase
{
    private readonly ILocacoesService _locacoesService;
    public LocacoesController(ILocacoesService locacoesService)
    {
        _locacoesService = locacoesService;
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreateLocacao([FromBody] CreateLocacaoDTO locacaoDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var locacaoResponse = await _locacoesService.CreateLocacaoAsync(locacaoDTO);
            return CreatedAtAction(nameof(GetLocacaoById), new { id = locacaoResponse.identificador }, locacaoResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLocacaoById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("ID inválido");
        try
        {
            var locacao = await _locacoesService.GetLocacaoById(id);
            return locacao == null ? NotFound() : Ok(locacao);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpPut("{id}/update")]
    public async Task<IActionResult> UpdateDevolutionDate(string id, [FromBody] DevolucaoLocacaoDTO devolucaoLocacaoDTO)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("ID inválido");
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var updatedLocacao = await _locacoesService.UpdateDevolutionDate(id, devolucaoLocacaoDTO);
            return Ok(updatedLocacao);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}