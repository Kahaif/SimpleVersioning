using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleVersioning.Data;
using SimpleVersioning.Data.SQLServer;
using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Testing
{
    [TestClass]
    public class DbTest
    {
        SqlServerStorageRepository sql;

        [TestInitialize]
        public void Initialize()
        {
            sql = new SqlServerStorageRepository(new DbContextOptionsBuilder<SqlServerContext>().UseInMemoryDatabase("test-db").Options);
        }

        [TestMethod]
        public void AssertAdd()
        {
            var files = Helper.GetRandomFiles(1);
            Assert.IsTrue(sql.Add(files[0]));
            Assert.ThrowsException<ArgumentNullException>(() => sql.Add<File>(null));
        }

        [TestMethod]
        public async Task AssertAddAsync()
        {
            var files = Helper.GetRandomFiles(1);
            Assert.IsTrue(await sql.AddAsync(files[0]));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await sql.AddAsync<File>(null));
        }


        [TestMethod]
        public async Task AssertAddRangeAsync()
        {
            Assert.IsTrue(await sql.AddRangeAsync(Helper.GetRandomFiles(10)));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await sql.AddRangeAsync<File>(null));
        }


        [TestMethod]
        public void  AssertAddRange()
        {
            Assert.ThrowsException<ArgumentNullException>(() => sql.AddRange<File>(null));
            Assert.IsTrue(sql.AddRange(Helper.GetRandomFiles(10)));
        }

        [TestMethod]
        public void AssertUpdate()
        {
            var files = Helper.GetRandomFiles(1);
            sql.Add(files[0]);

            Assert.IsTrue(sql.Update(files[0].Id, new File() { Name = "Test" }));
            Assert.IsTrue(sql.Get<File>(files[0].Id).Name == "Test");

        }
    }
}
