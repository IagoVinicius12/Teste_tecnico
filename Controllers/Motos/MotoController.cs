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
using Swashbuckle.AspNetCore.Annotations;

namespace Controllers.Motos;

[ApiController]
[Route("motos")]
[SwaggerTag("Criação de motos, update, verificações")]
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
    [SwaggerOperation(Summary = "Cria uma nova moto", Description = "Cria uma nova moto no sistema. Somente administradores podem acessar esta rota.")]
    [SwaggerResponse(201,"Moto criada com sucesso!",typeof(MotoResponse))]
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
    [SwaggerOperation(Summary = "Lista todas as motos", Description = "Retorna uma lista de todas as motos cadastradas. Acesso permitido para administradores e entregadores.")]
    [SwaggerResponse(200,"Lista de motos retornada com sucesso.", typeof(List<MotoResponse>))]
    public async Task<IActionResult> GetMotos()
    {
        var motos = await _motoService.ListAllMotosAsync();
        return motos == null ? NotFound() : Ok(motos);
    }

    [HttpGet("get/{id}")]
    [Authorize(Roles = "Admin,Entregador")]
    [SwaggerOperation(Summary = "Obtém uma moto pelo Identificador", Description = "Retorna os detalhes de uma moto específica pelo seu ID. Acesso permitido para administradores e entregadores.")]
    [SwaggerResponse(200,"Moto encontrada com sucesso.",typeof(MotoResponse))]
    public async Task<IActionResult> GetMoto(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Identificador invalido");
        var moto = await _motoService.GetMotoByIdAsync(id);
        return moto == null ? NotFound() : Ok(moto);
    }

    [HttpPut("update-plate")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary= "Atualiza a placa de uma moto", Description = "Atualiza a placa de uma moto existente. Somente administradores podem acessar esta rota.")]
    [SwaggerResponse(200, "Placa atualizada com sucesso.", typeof(MotoResponse))]
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
    [SwaggerOperation(Summary= "Deleta uma moto", Description = "Deleta uma moto do sistema caso nao haja nenhum registro de locacao. Somente administradores podem acessar esta rota.")]
    [SwaggerResponse(200, "Moto deletada com sucesso.")]
    public async Task<IActionResult> DeleteMoto(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Identificador invalido");
        }
        try {        
            bool deleted=await _motoService.DeleteMotoAsync(id);
            if (!deleted)
            {
                return NotFound("Moto nao foi encontrada ou não pode ser deletada por causa de um historico de locacao existente");
            }
            return Ok(new { Message = "Moto deletada com sucesso" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }

    }

    [HttpGet("getmotobyplate/{plate}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Busca uma moto por placa", Description = "Busca uma moto específica pelo numero da placa. Somente administradores podem acessar esta rota.")]
    [SwaggerResponse(200,"Moto encontrada com sucesso.", typeof(MotoResponse))] 
    public async Task<IActionResult> GetMotoByPlate(string plate)
    {
        if (string.IsNullOrEmpty(plate))
        {
            return BadRequest("Placa nao pode estar vazia.");
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