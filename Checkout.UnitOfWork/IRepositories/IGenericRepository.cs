using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.UnitOfWork.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task CreateAsync(T entity);
        void Add(T entity);
        Task UpdateAsync(T entity);
        Task<bool> RemoveAsync(int id);
        bool Remove(int id);
        bool IsExist(int id);
    }
}
