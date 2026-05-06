namespace BankInfinity.Api.Models;

public class Transaction : BaseEntity
{
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}