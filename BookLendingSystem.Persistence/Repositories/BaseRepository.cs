using BookLendingSystem.Application.Interfaces.IRepositories;
using BookLendingSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        protected DbSet<T> DbSet => _dbContext.Set<T>();

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> Create(T model)
        {
            await DbSet.AddAsync(model);
            await SaveChangesAsync();
            return model;
        }

        public async Task Delete(T model)
        {
            DbSet.Remove(model);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll() => await DbSet.AsNoTracking().ToListAsync();

        public async Task<T> GetById<IdType>(IdType id)
        {
            var data = await DbSet.FindAsync(id);
            return data is null ? throw new InvalidOperationException("No data Found") : data;
        }

        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public async Task Update(T model)
        {
            DbSet.Update(model);
            await SaveChangesAsync();
        }
    }
}
