using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebBackup.WPF.Data;
using Microsoft.EntityFrameworkCore;

namespace WebBackup.Tests
{
    [TestClass()]
    public class WBContextTests
    {
        [TestMethod()]
        public void CanConnectToDb()
        {
            // TODO: app.config file read
            string connectionString = @"Data Source=D:\Bence\Dokumentumok\Visual Studio 2022\Repos\borosbence\Website-Backup\Database\webbackup.db";
            var options = new DbContextOptionsBuilder<WBContext>().UseSqlite(connectionString).Options;
            var context = new WBContext(options);

            bool result = context.Database.CanConnect();

            Assert.IsTrue(result);
        }
    }
}