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

    public async Task<Result<bool>> TransferAsync(TransferRequest request)
    {
        if (request.Amount <= 0)
        {
            return Result<bool>.Failure("Transfer amount must be greater than zero.");
        }

        if (request.SenderAccountId == request.ReceiverAccountId)
        {
            return Result<bool>.Failure("Cannot transfer funds to the same account.");
        }

        var sender = (Account)await _accountRepository.GetByIdAsync(request.SenderAccountId);
        var receiver = (Account)await _accountRepository.GetByIdAsync(request.ReceiverAccountId);

        if (sender == null || receiver == null)
        {
            return Result<bool>.Failure("One or both accounts not found.");
        }

        if (sender.Balance < request.Amount)
        {
            return Result<bool>.Failure("Insufficient funds on the sender's account.");
        }
        
        sender.Balance -= request.Amount;
        var senderTransaction = new Transaction
        {
            AccountId = sender.Id, 
            Amount = -request.Amount,
            Category = "Transfer (Debit)",
            Date = DateTime.UtcNow
        };
        
        receiver.Balance += request.Amount;
        var receiverTransaction = new Transaction
        {
            AccountId = receiver.Id,
            Amount = request.Amount,
            Category = "Transfer (Credit)",
            Date = DateTime.UtcNow
        };
        
        await _transactionRepository.AddAsync(senderTransaction);
        await _transactionRepository.AddAsync(receiverTransaction);
        await _accountRepository.UpdateAsync(sender);
        await _accountRepository.UpdateAsync(receiver);

      
        await _accountRepository.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
    
    public async Task<Result<IEnumerable<TransactionResponse>>> GetTransactionsAsync(int accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account == null)
        {
            return Result<IEnumerable<TransactionResponse>>.Failure("Account not found.");
        }
        
        var allTransactions = await _transactionRepository.GetAllAsync(); 
        
        var transactionHistory = allTransactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Date)
            .Select(t => new TransactionResponse(t.Id, t.Amount, t.Category, t.Date))
            .ToList();

        return Result<IEnumerable<TransactionResponse>>.Success(transactionHistory);
    }
    
    private void ApplyCommission(ref decimal amount)
    {
        if (amount < 0)
        {
            amount -= 1.50m;
        }
    }
}