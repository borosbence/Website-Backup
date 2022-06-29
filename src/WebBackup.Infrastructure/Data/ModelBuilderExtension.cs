using Microsoft.EntityFrameworkCore;
using WebBackup.Core;

namespace WebBackup.Infrastructure.Data
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Website>().HasData(
                new Website { Id = 1, Name = "TestSite1", Url = "ABC" },
                new Website { Id = 2, Name = "TestSite2" }
            );

            modelBuilder.Entity<FTPConnection>().HasData(
               new FTPConnection { Id = 1, HostName = "ftp.dlptest.com", Username = "dlpuser", Password = "rNrKYTX9g7z3RgJRmxWuGHbeu", WebsiteId = 1 }
               );

            modelBuilder.Entity<SQLConnection>().HasData(
                new SQLConnection { Id = 1, HostName = "localhost", Username = "root", DatabaseName = "test", WebsiteId = 1 }
                );
        }
    }
}
