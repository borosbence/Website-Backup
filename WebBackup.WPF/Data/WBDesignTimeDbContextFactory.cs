using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebBackup.WPF.Data
{
    public class WBDesignTimeDbContextFactory : IDesignTimeDbContextFactory<WBContext>
    {
        public WBContext CreateDbContext(string[] args)
        {
            string connectionString = @"Data Source=webbackup.db";
            var options = new DbContextOptionsBuilder<WBContext>().UseSqlite(connectionString).Options;

            return new WBContext(options);
        }
    }
}
