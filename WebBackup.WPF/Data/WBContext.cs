using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebBackup.WPF.Models;

namespace WebBackup.WPF.Data
{
    public class WBContext : DbContext
    {
        public WBContext(DbContextOptions<WBContext> options) : base(options)
        {
        }

        public DbSet<Website> Websites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["WebBackupDB"].ConnectionString;
                optionsBuilder.UseSqlite(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }
    }
}
