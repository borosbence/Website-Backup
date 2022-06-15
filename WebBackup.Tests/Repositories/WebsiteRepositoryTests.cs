using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebBackup.WPF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebBackup.WPF.Data;
using Microsoft.Extensions.Configuration;
using WebBackup.WPF.Models;

namespace WebBackup.Tests
{
    [TestClass()]
    public class WebsiteRepositoryTests
    {
        private GenericRepository<Website, WBContext> _repository;

        public WebsiteRepositoryTests()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string connectionString = config.GetConnectionString("WebBackupDB");

            var options = new DbContextOptionsBuilder<WBContext>().UseSqlite(connectionString).Options;
            var context = new WBContext(options);
            _repository = new GenericRepository<Website, WBContext>(context);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var list = _repository.GetAll().Result;
            int result = list.Count;

            Assert.AreEqual(2, result);
        }
    }
}