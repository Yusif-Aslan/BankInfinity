namespace BankInfinity.Api.Models;

public enum UserStatus
{
    PendingEmailVerification = 0,
    PendingKYC = 1,
    Active = 2
}

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    public UserStatus Status { get; set; } = UserStatus.PendingEmailVerification;
    public string? EmailVerificationCode { get; set; } 
    public DateTime? VerificationCodeExpiry { get; set; }
}