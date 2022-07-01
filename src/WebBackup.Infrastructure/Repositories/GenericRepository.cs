using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebBackup.Core;
using WebBackup.Core.Repositories;

namespace WebBackup.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {

        protected IDbContextFactory<TContext> _contextFactory;

        public GenericRepository(IDbContextFactory<TContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private async Task<TContext> Context()
        {
            return await _contextFactory.CreateDbContextAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            TContext _context = await Context();
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            TContext _context = await Context();
            IQueryable<TEntity> query = _context.Set<TEntity>();
            foreach (var include in includes)
            {
                query = query.Include(include).AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            TContext _context = await Context();
            return await _context.Set<TEntity>().AnyAsync(x => x.Id == id);
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            TContext _context = await Context();
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            TContext _context = await Context();
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity, params string[] excludeProperties)
        {
            TContext _context = await Context();
            _context.Set<TEntity>().Update(entity);
            foreach (var property in excludeProperties)
            {
                _context.Entry(entity).Property(property).IsModified = false;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            TContext _context = await Context();
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
