using BankInfinity.Api.DTOs;
using BankInfinity.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankInfinity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("{id}/balance")]
    public async Task<IActionResult> GetBalance(int id)
    {
        try
        {
            var result = await _accountService.GetBalanceAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Account not found.");
        }
    }

    [HttpPost("transaction")]
    public async Task<IActionResult> ProcessTransaction([FromBody] TransactionRequest request)
    {
        var result = await _accountService.ProcessTransactionAsync(request);

        if (result.IsSuccess)
        {
            return Ok(new { NewBalance = result.Data });
        }

        return BadRequest(result.ErrorMessage);
    }
}