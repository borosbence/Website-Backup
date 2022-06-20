using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebBackup.WPF.Models;

namespace WebBackup.WPF.Repositories
{
    public interface IGenericRepository<T> where T : IEntity
    {
        Task<List<T>> GetAllAsync();
        /// <summary>
        /// Get All data with included foreign table records
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(object id);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
