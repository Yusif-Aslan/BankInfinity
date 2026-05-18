using BankInfinity.Api.DTOs;
using BankInfinity.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankInfinity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (result.IsSuccess)
        {
            return Ok(new { Token = result.Data });
        }

        return Unauthorized(new { Error = result.ErrorMessage });
    }
}