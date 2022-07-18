using Microsoft.EntityFrameworkCore;
using WebBackup.Core;
using WebBackup.Core.Repositories;
using WebBackup.Infrastructure.Data;

namespace WebBackup.Infrastructure.Repositories
{
    public interface IWebsiteRepository : IGenericRepository<Website>
    {
        Task<int> CountAsync();
    }

    public class WebsiteRepository : GenericRepository<Website, WBContext>, IWebsiteRepository
    {
        public WebsiteRepository(IDbContextFactory<WBContext> contextFactory) : base(contextFactory)
        {

        }

        /// <summary>
        /// Returns number of websites.
        /// </summary>
        /// <returns>Number of websites.</returns>
        public async Task<int> CountAsync()
        {
            WBContext _context = await Context();
            return await _context.Websites.CountAsync();
        }
    }
}
