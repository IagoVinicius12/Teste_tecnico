using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Services.Rentals.Interface.IRentalsService;
using Models.Rentals;
using MongoDB.Bson;
using Models.Rentals.Requests.CreateRentalDTO;
using Models.Rentals.Responses.DevolutionRentalResponse;
using Models.Rentals.Responses.RentalResponse;
using Models.Rentals.Requests.DevolutionRentalDTO;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("rentals")]
public class RentalsController : ControllerBase
{
    private readonly IRentalsService _rentalService;

    public RentalsController(IRentalsService rentalService)
    {
        _rentalService = rentalService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,DeliveryPerson")]
    [SwaggerOperation(
        Summary = "Cria uma nova locacao",
        Description = "Cria uma nova locacao no sistema. Acessivel para administradores e entregadores.",
        OperationId = "CreateRental")]
    [SwaggerResponse(201, "Locacao criada com sucesso", typeof(RentalResponse))]
    [SwaggerResponse(400, "Dados da requisicao invalidos")]
    [SwaggerResponse(500, "Erro interno no servidor")]
    public async Task<IActionResult> CreateRental([FromBody] CreateRentalDTO rentalDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = await _rentalService.CreateRentalAsync(rentalDto);
            return CreatedAtAction(
                nameof(GetRentalById),
                new { id = response.identifier },
                response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,DeliveryPerson")]
    [SwaggerOperation(
        Summary = "Obtem locacao pelo identificador",
        Description = "Recupera os detalhes de uma locacao específica pelo seu identificador. Acessivel para administradores e entregadores.",
        OperationId = "GetRental")]
    [SwaggerResponse(200, "Detalhes da locacao", typeof(RentalResponse))]
    [SwaggerResponse(404, "Locacao nao encontrada")]
    public async Task<IActionResult> GetRentalById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Identificador invalido");

        try
        {
            var rental = await _rentalService.GetRentalById(id);
            return rental == null ? NotFound() : Ok(rental);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpPut("{id}/return")]
    [Authorize(Roles = "Admin,DeliveryPerson")]
    [SwaggerOperation(
        Summary = "Atualiza devolucao da locacao",
        Description = "Atualiza os detalhes de devolução de uma locacao existente. Acessivel para administradores e entregadores.")]
    [SwaggerResponse(200, "Locacao atualizada com sucesso", typeof(RentalResponse))]
    [SwaggerResponse(400, "Dados da requisicao invalidos")]
    public async Task<IActionResult> UpdateDevolutionDate(
        string id,
        [FromBody] DevolutionRentalDTO returnDto)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Identificador invalido");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedRental = await _rentalService.UpdateDevolutionDate(id, returnDto);
            return Ok(updatedRental);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}