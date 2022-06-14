using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebBackup.WPF.Data
{
    public class WBContextFactory : IDesignTimeDbContextFactory<WBContext>
    {
        public WBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WBContext>();
            //string connectionString = ConfigurationManager.ConnectionStrings["WebBackupDB"].ConnectionString;
            string connectionString = @"Data Source=D:\Bence\Dokumentumok\Visual Studio 2022\Repos\borosbence\Website-Backup\Database\webbackup.db";
            optionsBuilder.UseSqlite(connectionString);

            //return Ioc.Default.GetService<WebBackupContext>();
            return new WBContext(optionsBuilder.Options);
        }
    }
}
