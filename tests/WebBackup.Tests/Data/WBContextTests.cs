using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebBackup.Infrastructure.Data;

namespace WebBackup.Tests
{
    [TestClass()]
    public class WBContextTests
    {
        [TestMethod()]
        public void CanConnectToDb()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string connectionString = config.GetConnectionString("WebBackupDB");

            var options = new DbContextOptionsBuilder<WBContext>().UseSqlite(connectionString).Options;
            var context = new WBContext(options);

            bool result = context.Database.CanConnect();

            Assert.IsTrue(result);
        }
    }
}