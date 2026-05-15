using BankInfinity.Api.DTOs;
using BankInfinity.Api.Models;

namespace BankInfinity.Api.Interfaces;

public interface IAccountService
{
    Task<Result<AccountBalanceResponse>> GetBalanceAsync(int accountId);
    Task<Result<decimal>> ProcessTransactionAsync(TransactionRequest request);
    Task<Result<bool>> TransferAsync(TransferRequest request);
    Task<Result<IEnumerable<TransactionResponse>>> GetTransactionsAsync(int accountId);
    
}