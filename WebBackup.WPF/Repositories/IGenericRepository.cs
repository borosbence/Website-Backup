using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebBackup.WPF.Models;

namespace WebBackup.WPF.Repositories
{
    public interface IGenericRepository<T> where T : IEntity
    {
        Task<List<T>> GetAll();
        /// <summary>
        /// Get All data with included foreign table records
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<List<T>> GetAll(params Expression<Func<T, object>>[] includes);
        Task<T?> GetById(object id);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
