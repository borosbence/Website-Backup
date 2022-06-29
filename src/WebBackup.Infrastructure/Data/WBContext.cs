using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebBackup.Core;

namespace WebBackup.Infrastructure.Data
{
    public class WBContext : DbContext
    {
        public WBContext(DbContextOptions<WBContext> options) : base(options)
        {
        }

        public DbSet<Website> Websites => Set<Website>();
        public DbSet<FTPConnection> FTPConnections => Set<FTPConnection>();
        public DbSet<SQLConnection> SQLConnections => Set<SQLConnection>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FTPConnection>()
                .HasIndex(x => x.WebsiteId)
                .IsUnique();
            modelBuilder.Entity<SQLConnection>()
                .HasIndex(x => x.WebsiteId)
                .IsUnique();
            modelBuilder.Seed();
        }
    }
}
