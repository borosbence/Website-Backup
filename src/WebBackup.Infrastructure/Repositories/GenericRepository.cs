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
        // https://docs.microsoft.com/en-us/ef/core/dbcontext-configuration/ <3
        protected IDbContextFactory<TContext> _contextFactory;

        public GenericRepository(IDbContextFactory<TContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            using TContext context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            using TContext context = await _contextFactory.CreateDbContextAsync();
            IQueryable<TEntity> query = context.Set<TEntity>();
            foreach (var include in includes)
            {
                query = query.Include(include).AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using TContext context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<TEntity>().AnyAsync(x => x.Id == id);
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            using TContext context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            using TContext context = await _contextFactory.CreateDbContextAsync();
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity, params string[] excludeProperties)
        {
            using TContext context = await _contextFactory.CreateDbContextAsync();
            context.Set<TEntity>().Update(entity);
            foreach (var property in excludeProperties)
            {
                context.Entry(entity).Property(property).IsModified = false;
            }
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            using TContext context = await _contextFactory.CreateDbContextAsync();
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
