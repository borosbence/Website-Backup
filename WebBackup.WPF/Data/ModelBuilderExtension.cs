using Microsoft.EntityFrameworkCore;
using WebBackup.WPF.Models;

namespace WebBackup.WPF.Data
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Website>().HasData(
                new Website { Id = 1, Name = "TestSite1" },
                new Website { Id = 2, Name = "TestSite2" }
            );

            modelBuilder.Entity<FTPConnection>().HasData(
               new FTPConnection { Id = 1, Hostname = "ftp.dlptest.com", Username = "dlpuser", Password = "rNrKYTX9g7z3RgJRmxWuGHbeu", WebsiteId = 1 }
               );

            modelBuilder.Entity<SQLConnection>().HasData(
                new SQLConnection { Id = 1, Hostname = "localhost", Username = "root", Databasename = "test", WebsiteId = 1 }
                );
        }
    }
}
