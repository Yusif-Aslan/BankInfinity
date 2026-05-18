namespace BankInfinity.Api.DTOs;

public record CreateAccountRequest(int UserId, string AccountNumber);
public record TransactionRequest(int AccountId, decimal Amount, string Category);
public record AccountBalanceResponse(string AccountNumber, decimal Balance);
public record CreateUserRequest(string FullName, string Email, string PhoneNumber, string Password);
public record LoginRequest(string Email, string Password);
public record UserResponse(int Id, string FullName, string Email);

public record TransferRequest(int SenderAccountId, int ReceiverAccountId, decimal Amount);
public record TransactionResponse(int Id, decimal Amount, string Category, DateTime Date);
public record VerifyEmailRequest(string Email, string Code);
public record KycWebhookPayload(int UserId, string KycStatus, string DocumentId);
