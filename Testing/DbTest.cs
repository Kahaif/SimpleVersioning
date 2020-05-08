using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleVersioning.Data;
using SimpleVersioning.Data.SQLServer;
using SimpleVersioning.Models;
using System;
using System.Threading.Tasks;

namespace Testing
{
    [TestClass]
    public class DbTest
    {
        IStorageRepository storage;

        [TestInitialize]
        public void Initialize()
        {
            storage = new SqlServerStorageRepository(new DbContextOptionsBuilder<SqlServerContext>().UseInMemoryDatabase("test-db").Options);
        }

        [TestMethod]
        public void AssertGet()
        {
            var files = Helper.GetRandomFiles(1);
            storage.AddRange(files);

            var retrievedFile = storage.Get<File>(files[0].Id);
            Assert.IsNotNull(storage.Get<File>(files[0].Id));
            Assert.AreEqual(retrievedFile, files[0]);
        }

        [TestMethod]
        public void AssertAdd()
        {
            var files = Helper.GetRandomFiles(1);
            Assert.IsTrue(storage.Add(files[0]));
            Assert.ThrowsException<ArgumentNullException>(() => storage.Add<File>(null));
        }

        [TestMethod]
        public async Task AssertGetAsync()
        {

            var files = Helper.GetRandomFiles(1);
            storage.AddRange(files);

            var retrievedFile = storage.Get<File>(files[0].Id);
            Assert.IsNotNull(await storage.GetAsync<File>(files[0].Id));
            Assert.AreEqual(retrievedFile, files[0]);
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
        public void  AssertAddRange()
        {
            Assert.ThrowsException<ArgumentNullException>(() => storage.AddRange<File>(null));
            Assert.IsTrue(storage.AddRange(Helper.GetRandomFiles(10)));
        }

        [TestMethod]
        public void AssertUpdate()
        {
            var files = Helper.GetRandomFiles(1);
            storage.Add(files[0]);
            File f = files[0];
            f.Name = "Test";

            Assert.IsTrue(storage.Update(files[0].Id, f));
            Assert.IsTrue(storage.Get<File>(files[0].Id).Name == f.Name);
            Assert.ThrowsException<ArgumentException>(() => storage.Update(-4, f));
            Assert.ThrowsException<ArgumentNullException>(() => storage.Update<File>(4, null));
        }

        [TestMethod]
        public async Task AssertUpdateAsync()
        {
            var files = Helper.GetRandomFiles(1);
            storage.AddRange(files);
            var f = files[0];
            f.Name = "Test";

            Assert.IsTrue(await storage.UpdateAsync(files[0].Id, f));
            Assert.IsTrue((await storage.GetAsync<File>(files[0].Id)).Name == f.Name);
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await storage.UpdateAsync<File>(-4, f));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await storage.UpdateAsync<File>(4, null));

        }

        [TestMethod]
        public void AssertDelete()
        {
            var files = Helper.GetRandomFiles(1);
            storage.AddRange(files);

            Assert.IsTrue(storage.Delete<File>(files[0].Id));
            Assert.IsNull(storage.Get<File>(files[0].Id));
        }

        [TestMethod]
        public async Task AssertDeleteAsync()
        {
            var files = Helper.GetRandomFiles(1);
            storage.AddRange(files);

            Assert.IsTrue(await storage.DeleteAsync<File>(files[0].Id));
            Assert.IsNull(await storage.GetAsync<File>(files[0].Id));
        }
    }
}
