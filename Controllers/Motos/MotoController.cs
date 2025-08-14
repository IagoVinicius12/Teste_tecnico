using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using MongoDB.Bson;
using Services.Moto.Interfaces.IMotoService;
using Models.MotoModel;
using Models.Motos.Requests.CreateMotoDTO;
using Models.Motos.Requests.UpdateMotoPlateDTO;
using Models.Motos.Responses.MotoResponse;

[ApiController]
[Route("api/[controller]")]

public class MotoController : ControllerBase
{
    private readonly IMotoService _motoService;
    public MotoController(IMotoService motoService)
    {
        _motoService = motoService;
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreateMoto([FromBody] CreateMotoDTO motoDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var motoResponse = await _motoService.CreateMotoAsync(motoDTO);
            return CreatedAtAction(nameof(GetMoto), new { id = motoResponse.Id }, motoResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetMotos()
    {
        var motos = await _motoService.ListAllMotosAsync();
        return motos == null ? NotFound() : Ok(motos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMoto(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("ID inválido");
        var moto = await _motoService.GetMotoByIdAsync(id);
        return moto == null ? NotFound() : Ok(moto);
    }

    [HttpPut("update-plate")]
    public async Task<IActionResult> UpdateMotoPlate([FromBody] UpdateMotoPlateDTO updateMotoDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var updatedMoto = await _motoService.UpdateMotoPlate(updateMotoDTO);
            return updatedMoto == null ? NotFound() : Ok(updatedMoto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}