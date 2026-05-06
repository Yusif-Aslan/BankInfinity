using BankInfinity.Api.Data;
using BankInfinity.Api.Interfaces;
using BankInfinity.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BankInfinity.Api.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly BankDbContext Context;
    private readonly DbSet<T> _dbSet;

    public Repository(BankDbContext context)
    {
        Context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync();
    }
}