namespace BankInfinity.Api.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string AccountNumber { get; set; } 
        public decimal Balance { get; set; } 
    }
}