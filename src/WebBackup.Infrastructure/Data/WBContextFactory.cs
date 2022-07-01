using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebBackup.Infrastructure.Data
{
    public class WBDbContextFactory : IDesignTimeDbContextFactory<WBContext>, IDbContextFactory<WBContext>
    {
        private const string CONNECTION_STRING = "Data Source=webbackup.db";

        public WBContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<WBContext>().UseSqlite(CONNECTION_STRING).Options;
            return new WBContext(options);
        }

        public WBContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<WBContext>().UseSqlite(CONNECTION_STRING).Options;
            return new WBContext(options);
        }
    }
}
