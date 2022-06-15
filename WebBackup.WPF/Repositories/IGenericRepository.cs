using System.Collections.Generic;
using System.Threading.Tasks;
using WebBackup.WPF.Models;

namespace WebBackup.WPF.Repositories
{
    public interface IGenericRepository<T> where T : IEntity
    {
        Task<List<T>> GetAll();
        Task<T?> GetById(int id);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
