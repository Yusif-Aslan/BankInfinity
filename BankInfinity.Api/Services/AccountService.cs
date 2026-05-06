using BankInfinity.Api.DTOs;
using BankInfinity.Api.Interfaces;
using BankInfinity.Api.Models;

namespace BankInfinity.Api.Services;

public class AccountService : IAccountService
{
    private readonly IRepository<Account> _accountRepository;
    private readonly IRepository<Transaction> _transactionRepository;

    public AccountService(IRepository<Account> accountRepository, IRepository<Transaction> transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<Result<AccountBalanceResponse>> GetBalanceAsync(int accountId)
    {
        BaseEntity entity = await _accountRepository.GetByIdAsync(accountId) ?? throw new KeyNotFoundException();
        
        if (entity is not Account account)
        {
            return Result<AccountBalanceResponse>.Failure("Invalid entity type.");
        }

        var response = new AccountBalanceResponse(account.AccountNumber, account.Balance);
        return Result<AccountBalanceResponse>.Success(response);
    }

    public async Task<Result<decimal>> ProcessTransactionAsync(TransactionRequest request)
    {
        if (!ValidateTransactionAmount(request.Amount, out string errorMessage))
        {
            return Result<decimal>.Failure(errorMessage);
        }

        var account = await _accountRepository.GetByIdAsync(request.AccountId);
        if (account == null)
        {
            return Result<decimal>.Failure("Account not found.");
        }

        decimal finalAmount = request.Amount;
        ApplyCommission(ref finalAmount);

        if (finalAmount < 0 && account.Balance + finalAmount < 0)
        {
            return Result<decimal>.Failure("Insufficient funds.");
        }

        var transaction = new Transaction
        {
            AccountId = request.AccountId,
            Amount = finalAmount,
            Category = request.Category,
            Date = DateTime.UtcNow
        };

        account.Balance += finalAmount;

        await _transactionRepository.AddAsync(transaction);
        await _accountRepository.UpdateAsync(account);
        
        await _transactionRepository.SaveChangesAsync();
        await _accountRepository.SaveChangesAsync();

        return Result<decimal>.Success(account.Balance);
    }

    private bool ValidateTransactionAmount(decimal amount, out string error)
    {
        if (amount == 0)
        {
            error = "Transaction amount cannot be zero.";
            return false;
        }

        error = string.Empty;
        return true;
    }

    private void ApplyCommission(ref decimal amount)
    {
        if (amount < 0)
        {
            amount -= 1.50m;
        }
    }
}