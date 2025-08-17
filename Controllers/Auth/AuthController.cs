using System;
using Microsoft.AspNetCore.Mvc;
using Services.Auth.Interface.IAuthService;
using Models.Auth.Requests.LoginDTO;

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    //[HttpPost("hash_password")]
    public IActionResult HashPassword([FromBody] string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return BadRequest("Password cannot be empty.");
        try
        {
            var hashedPassword = _authService.HashPassword(password);
            return Ok(new { HashedPassword = hashedPassword });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    //[HttpPost("verify_password")]
    public IActionResult VerifyPassword([FromBody] string password, string stored_hash)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var isValid = _authService.VerifyPassword(password, stored_hash);
            return Ok(new { IsValid = isValid });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpPost("auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var token = await _authService.GenerateTokenAsync(loginDTO);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}