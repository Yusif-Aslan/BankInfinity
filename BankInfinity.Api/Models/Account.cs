namespace BankInfinity.Api.Models;

public class Account : BaseEntity
{
    public int UserId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}