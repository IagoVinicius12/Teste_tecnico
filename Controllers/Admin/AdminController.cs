using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Admin.Interface.IAdminService;
using Models.AdminModel;
using Models.Admin.Requests.CreateAdminDTO;
using Models.Admin.Responses.AdminResponse;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    [HttpPost("create_admin")]
    public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDTO adminDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var createdAdmin = await _adminService.CreateAdminAsync(adminDTO);
            return CreatedAtAction(nameof(GetAdminById), new { id = createdAdmin.id }, createdAdmin);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("get_admin_by_id/{id}")]
    public async Task<IActionResult> GetAdminById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("ID inválido");
        try
        {
            var admin = await _adminService.GetAdminById(id);
            return admin == null ? NotFound() : Ok(admin);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}