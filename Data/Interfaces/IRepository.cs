using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryFinalsProject.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> CountAsync(Func<T, bool> predicate);
        Task SaveChangesAsync();
    }
}
