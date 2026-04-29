using System.Linq;
using BankInfinity.Api.Models;
using BankInfinity.Api.Data;

namespace BankInfinity.Api.Services
{
    public class AccountService
    {
        private readonly BankDbContext _context;

        public AccountService(BankDbContext context)
        {
            _context = context;
        }
        
        public Result<decimal> GetBalance(int accountId)
        {
            decimal totalBalance = _context.Transactions
                .Where(t => t.AccountId == accountId) 
                .Sum(t => t.Amount);                  
            
            return Result<decimal>.Success(totalBalance);
        }
    }
}