using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleVersioning.Data.Sql;
using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Testing
{

    [TestClass]
    public class SqlServerStorageTest
    {
        readonly MariaDBStorageRepository storage;
        public SqlServerStorageTest()
        {
            storage = new MariaDBStorageRepository(
                   new DbContextOptionsBuilder<MariaDBServerContext>().UseMySql("Server=80.219.88.57;Port=3306;Database=SimpleVersioning;User=simple-versioning;Password=McdlMp$123;").Options);

        }
        [TestInitialize]
        public void Initialize()
        {
            storage.Context.Database.ExecuteSqlRaw("DELETE FROM FileProperties");
            storage.Context.Database.ExecuteSqlRaw("DELETE FROM FileVersions");
            storage.Context.Database.ExecuteSqlRaw("DELETE FROM Files");
        }


        [TestMethod]
        public async Task AssertGetAllAsync()
        {
            var files = Helper.GetRandomFiles(2);
            await storage.AddRangeAsync(files);
            var receivedFile = storage.GetAsync<File>();
            int count = 0;
            
            await foreach (var item in receivedFile)
            {
                count++;
            }

            Assert.IsTrue(count >= files.Count());
        }


        [TestMethod]
        public async Task AssertAddAsync()
        {
            var files = Helper.GetRandomFiles(1);
            Assert.IsTrue(await storage.AddAsync(files[0]));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await storage.AddAsync<File>(null));
        }


        [TestMethod]
        public async Task AssertAddRangeAsync()
        {
            Assert.IsTrue(await storage.AddRangeAsync(Helper.GetRandomFiles(10)));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await storage.AddRangeAsync<File>(null));
        }

        [TestMethod]
        public async Task AssertUpdateAsync()
        {
            var files = Helper.GetRandomFiles(1);
            await storage.AddRangeAsync(files);
            var f = files[0];
            f.Name = "Test";

            Assert.IsTrue(await storage.UpdateAsync(files[0].Id, f));
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await storage.UpdateAsync(-4, f));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await storage.UpdateAsync<File>(4, null));

        }


        [TestMethod]
        public async Task AssertDeleteAsync()
        {
            var files = Helper.GetRandomFiles(1);
            await storage.AddRangeAsync(files);

            Assert.IsTrue(await storage.DeleteAsync<File>(files[0].Id));
        }
    }
}
 