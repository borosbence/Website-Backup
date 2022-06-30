using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebBackup.Infrastructure.Data
{
    public class WBDesignTimeDbContextFactory : IDesignTimeDbContextFactory<WBContext>
    {
        public WBContext CreateDbContext(string[] args)
        {
            const string CONNECTION_STRING = "Data Source=webbackup.db";
            var options = new DbContextOptionsBuilder<WBContext>().UseSqlite(CONNECTION_STRING).Options;
            return new WBContext(options);
        }
    }
}
