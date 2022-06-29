using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebBackup.WPF.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebBackup.Domain;
using WebBackup.Infrastructure;
using WebBackup.Domain.Repositories;

namespace WebBackup.Tests
{
    [TestClass()]
    public class WebsiteRepositoryTests
    {
        private readonly IGenericRepository<Website> _repository;

        public WebsiteRepositoryTests()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string connectionString = config.GetConnectionString("WebBackupDB");

            var options = new DbContextOptionsBuilder<WBContext>().UseSqlite(connectionString).Options;
            var context = new WBContext(options);
            _repository = new GenericRepository<Website, WBContext>(context);
        }

        [TestMethod()]
        public void GetAllCountTest()
        {
            var list = _repository.GetAllAsync().Result;
            int result = list.Count;

            Assert.AreEqual(2, result);
        }

        [TestMethod()]
        public void GetAllWithNavPropertyTest()
        {
            var first = _repository.GetAllAsync(x => x.FTPConnection).Result.First();

            Assert.IsNotNull(first.FTPConnection);
        }
    }
}