using BankInfinity.Api.DTOs;
using BankInfinity.Api.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BankInfinity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserRequest> _createUserValidator;

    public UsersController(
        IUserService userService, 
        IValidator<CreateUserRequest> createUserValidator)
    {
        _userService = userService;
        _createUserValidator = createUserValidator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        if (id <= 0) return BadRequest("Invalid user ID.");

        var result = await _userService.GetUserAsync(id);
        
        if (result.IsSuccess) return Ok(result.Data);
        
        return NotFound(result.ErrorMessage);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        // 1. Manual Validation
        var validationResult = await _createUserValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        // 2. Business Logic
        var result = await _userService.CreateUserAsync(request);
        
        if (result.IsSuccess)
        {
            // Returns a 201 Created status code and a Location header pointing to the new resource
            return CreatedAtAction(nameof(GetUser), new { id = result.Data.Id }, result.Data);
        }

        return BadRequest(result.ErrorMessage);
    }
}