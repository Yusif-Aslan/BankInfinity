namespace BankInfinity.Api.DTOs;

public record CreateAccountRequest(int UserId, string AccountNumber);
public record TransactionRequest(int AccountId, decimal Amount, string Category);
public record AccountBalanceResponse(string AccountNumber, decimal Balance);
public record CreateUserRequest(string FullName, string Email, string PhoneNumber);
public record UserResponse(int Id, string FullName, string Email);