using Microsoft.EntityFrameworkCore;
using BankInfinity.Api.Models;

namespace BankInfinity.Api.Data
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) { }

        // Это таблица пользователей
        public DbSet<User> Users { get; set; }
        
        // TODO: ЗАДАНИЕ 3. Добавь DbSet для Account и Transaction, чтобы EF Core создал для них таблицы.
        // Пример: public DbSet<ИмяКласса> ИмяТаблицы { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        
    }
}