using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using WebBackup.Core.Repositories;
using WebBackup.Core;
using WebBackup.Infrastructure.Data;
using WebBackup.Infrastructure.Repositories;

namespace WebBackup.Tests
{
    [TestClass()]
    public class WebsiteRepositoryTests
    {
        private readonly IGenericRepository<Website> _repository;

        public WebsiteRepositoryTests()
        {
            IDbContextFactory<WBContext> contextFactory = new WBDbContextFactory();
            _repository = new GenericRepository<Website, WBContext>(contextFactory);
        }

        [TestMethod()]
        public void GetAllCountTest()
        {
            List<Website> list = _repository.GetAllAsync().Result;
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