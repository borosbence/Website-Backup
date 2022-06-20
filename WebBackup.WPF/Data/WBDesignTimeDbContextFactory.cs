using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebBackup.WPF.Data
{
    public class WBDesignTimeDbContextFactory : IDesignTimeDbContextFactory<WBContext>
    {
        public WBContext CreateDbContext(string[] args)
        {
            // TODO: change connstring
            string connectionString = @"Data Source=D:\Bence\Dokumentumok\Visual Studio 2022\Repos\borosbence\Website-Backup\Database\webbackup.db";
            var options = new DbContextOptionsBuilder<WBContext>().UseSqlite(connectionString).Options;

            return new WBContext(options);
        }
    }
}
