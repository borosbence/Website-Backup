using System.Linq.Expressions;
namespace WebBackup.Core.Repositories
{
    public interface IGenericRepository<T> where T : IEntity
    {
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Get All data with included foreign table records.
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

        Task<T?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task InsertAsync(T entity);

        /// <summary>
        /// Updates and exlcudes properties.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task UpdateAsync(T entity, params string[] includeProperties);

        Task DeleteAsync(T entity);
    }
}
