using BankInfinity.Api.DTOs;
using BankInfinity.Api.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BankInfinity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IValidator<TransactionRequest> _transactionValidator;
    private readonly IValidator<TransferRequest> _transferValidator;

    public AccountsController(
        IAccountService accountService, 
        IValidator<TransactionRequest> transactionValidator,
        IValidator<TransferRequest> transferValidator)
    {
        _accountService = accountService;
        _transactionValidator = transactionValidator;
        _transferValidator = transferValidator;
    }

    [HttpGet("{id}/balance")]
    public async Task<IActionResult> GetBalance(int id)
    {
        if (id <= 0) return BadRequest("Invalid account ID.");

        try
        {
            var result = await _accountService.GetBalanceAsync(id);
            if (result.IsSuccess) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Account not found.");
        }
    }

    [HttpGet("{id}/transactions")]
    public async Task<IActionResult> GetTransactions(int id)
    {
        if (id <= 0) return BadRequest("Invalid account ID.");

        var result = await _accountService.GetTransactionsAsync(id);
        if (result.IsSuccess) return Ok(result.Data);
        
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("transaction")]
    public async Task<IActionResult> ProcessTransaction([FromBody] TransactionRequest request)
    {
        // 1. Manual Validation
        var validationResult = await _transactionValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        // 2. Business Logic
        var result = await _accountService.ProcessTransactionAsync(request);
        if (result.IsSuccess) return Ok(new { NewBalance = result.Data });
        
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
    {
        // 1. Manual Validation
        var validationResult = await _transferValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        // 2. Business Logic
        var result = await _accountService.TransferAsync(request);
        if (result.IsSuccess) return Ok(new { Message = "Transfer completed successfully." });
        
        return BadRequest(result.ErrorMessage);
    }
}