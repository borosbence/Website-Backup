using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebBackup.WPF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebBackup.WPF.Data;

namespace WebBackup.Tests
{
    [TestClass()]
    public class WebsiteRepositoryTests
    {
        private WebsiteRepository _repo;

        public WebsiteRepositoryTests()
        {
            string connectionString = @"Data Source=D:\Bence\Dokumentumok\Visual Studio 2022\Repos\borosbence\Website-Backup\Database\webbackup.db";
            var options = new DbContextOptionsBuilder<WBContext>().UseSqlite(connectionString).Options;
            var context = new WBContext(options);
            _repo = new WebsiteRepository(context);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var list = _repo.GetAll().Result;
            int result = list.Count;

            Assert.AreEqual(2, result);
        }
    }
}