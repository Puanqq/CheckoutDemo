using Checkout.UnitOfWork.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.UnitOfWork.Repositories
{
    public class GenericRepository<T, TContext> : IGenericRepository<T>
        where T : class
        where TContext : DbContext
    {
        protected readonly TContext context;
        protected readonly DbSet<T> dbSet;

        public GenericRepository(TContext context)
        {
            this.context = context;
            dbSet = this.context.Set<T>();
        }
        public virtual async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            if (dbSet is null)
                return null;
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public bool IsExist(int id)
        {
            var entity = GetAsync(id);
            if (entity == null)
                return false;
            return true;
        }

        public virtual async Task<bool> RemoveAsync(int id)
        {
            var entity = await GetAsync(id);
            if (entity is null)
                return false;
            dbSet.Remove(entity);
            return true;
        }

        public virtual bool Remove(int id)
        {
            var entity = dbSet.Find(id);
            if (entity == null)
            {
                return false;
            }
            dbSet.Remove(entity);
            return true;
        }

        public Task UpdateAsync(T entity)
        {
            return Task.Run(() => dbSet.Update(entity));
        }
    }
}
