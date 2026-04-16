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
            _context = context; // Получаем доступ к базе данных
        }

        // Метод для получения баланса конкретного счета
        public Result<decimal> GetBalance(int accountId)
        {
            // 1. Фильтруем транзакции по accountId и суммируем их поле Amount
            decimal totalBalance = _context.Transactions
                .Where(t => t.AccountId == accountId) // Оставляем только нужные
                .Sum(t => t.Amount);                  // Складываем все Amount

            // 2. Возвращаем успешный результат с посчитанным балансом
            return Result<decimal>.Success(totalBalance);
        }
    }
}