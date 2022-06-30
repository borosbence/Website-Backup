using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebBackup.Core;
using WebBackup.Core.Repositories;

namespace WebBackup.WPF.Repositories
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        protected TContext _context;

        public GenericRepository(TContext context)
        {
            _context = context;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Set<TEntity>().AnyAsync(x => x.Id == id);
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity, params string[] excludeProperties)
        {
            try
            {
                // TODO: WHY error???
                _context.Entry<TEntity>(entity).State = EntityState.Modified;
                // _context.Set<TEntity>().Update(entity);

            }
            catch (Exception ex)
            {

                throw;
            }
            foreach (var property in excludeProperties)
            {
                _context.Entry(entity).Property(property).IsModified = false;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
