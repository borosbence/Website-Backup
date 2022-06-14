using Microsoft.EntityFrameworkCore;
using WebBackup.WPF.Models;

namespace WebBackup.WPF.Data
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // Test Connections
            modelBuilder.Entity<Website>().HasData(
                   new Website { Id = 1, Name = "TestSite1" },
                   new Website { Id = 2, Name = "TestSite2" }
            );
        }
    }
}
