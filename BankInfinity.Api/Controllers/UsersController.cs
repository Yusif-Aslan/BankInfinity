using BankInfinity.Api.DTOs;
using BankInfinity.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankInfinity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _userService.GetUserAsync(id);
        
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        
        return NotFound(result.ErrorMessage);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _userService.CreateUserAsync(request);
        
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetUser), new { id = result.Data!.Id }, result.Data);
        }
        
        return BadRequest(result.ErrorMessage);
    }
}