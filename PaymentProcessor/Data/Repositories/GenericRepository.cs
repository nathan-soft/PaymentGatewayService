using Microsoft.EntityFrameworkCore;
using PaymentProcessor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LifeLongApi.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await entities.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await entities.Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            //return entities.SingleOrDefault(s => s.Id == id);
            return await entities.FindAsync(id);
        }

        public async Task InsertAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            await entities.AddAsync(entity);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity)
        {
            //T entity = entities.SingleOrDefault(s => s.Id == id);
            //T entity = await entities.FindAsync(id);
            entities.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}