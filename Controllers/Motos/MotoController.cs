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

namespace Controllers.Motos;

[ApiController]
[Route("api/[controller]")]

//rotas de criação de update de motos podem ser usadas somente pelos administradores, para a listagem de motos podem ser utilizadas ambas as contas
public class MotoController : ControllerBase
{
    private readonly IMotoService _motoService;
    public MotoController(IMotoService motoService)
    {
        _motoService = motoService;
    }
    [HttpPost("create")]
    [Authorize(Roles ="Admin")]
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
    [Authorize(Roles ="Admin,Entregador")]
    public async Task<IActionResult> GetMotos()
    {
        var motos = await _motoService.ListAllMotosAsync();
        return motos == null ? NotFound() : Ok(motos);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Entregador")]
    public async Task<IActionResult> GetMoto(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("ID inválido");
        var moto = await _motoService.GetMotoByIdAsync(id);
        return moto == null ? NotFound() : Ok(moto);
    }

    [HttpPut("update-plate")]
    [Authorize(Roles = "Admin")]
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

    [HttpDelete("delete/{id}")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> DeleteMoto(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("ID inválido");
        }
        try {        
            bool deleted=await _motoService.DeleteMotoAsync(id);
            if (!deleted)
            {
                return NotFound("Moto not found or cannot be deleted due to existing rental history.");
            }
            return Ok(new { Message = "Moto deleted successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }

    }

    [HttpGet("getmotobyplate/{plate}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetMotoByPlate(string plate)
    {
        if (string.IsNullOrEmpty(plate))
        {
            return BadRequest("Plate cannot be empty.");
        }
        try
        {
            var moto =await _motoService.GetMotoByPlateAsync(plate);
            return moto==null ? NotFound() : Ok(moto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}