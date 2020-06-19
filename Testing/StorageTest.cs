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
    public class StorageTest
    {
        SqlServerStorageRepository storage;

        [TestInitialize]
        public void Initialize()
        {
            storage = new SqlServerStorageRepository(
                   new DbContextOptionsBuilder<SqlServerContext>()
                   .UseSqlServer(@"Data Source=PC-Kevin\SQLSERVER;Database=SimpleVersioning;Integrated Security=True;MultipleActiveResultSets=True;").Options);

            storage.Context.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable  \"ALTER TABLE ? NOCHECK CONSTRAINT all\"");
            
            storage.Context.Database.ExecuteSqlRaw("DELETE FROM dbo.Files");
            storage.Context.Database.ExecuteSqlRaw("DELETE FROM dbo.FileProperties");
            storage.Context.Database.ExecuteSqlRaw("DELETE FROM dbo.FileVersions");

            storage.Context.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable \"ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all\"");

        }

        [TestMethod]
        public void AssertGetAll()
        {
            var files = Helper.GetRandomFiles(10);
            Assert.IsTrue(storage.AddRange(files));
        }

        [TestMethod]
        public async Task AssertGetAllAsync()
        {
            var files = Helper.GetRandomFiles(10);
            storage.AddRange(files);
            var receivedFile = storage.GetAsync<File>();
            int count = 0;
            
            await foreach (var item in receivedFile)
            {
                count++;
            }

            Assert.IsTrue(count >= files.Count());
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
        public async Task AssertGetAsync()
        {
            var files = Helper.GetRandomFiles(1);
            storage.AddRange(files);

            var retrievedFile = storage.Get<File>(files[0].Id);
            Assert.IsNotNull(await storage.GetAsync<File>(files[0].Id));
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
        public async Task AssertAddAsync()
        {
            var files = Helper.GetRandomFiles(1);
            Assert.IsTrue(await storage.AddAsync(files[0]));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await storage.AddAsync<File>(null));
        }


        [TestMethod]
        public void AssertAddRange()
        {
            Assert.ThrowsException<ArgumentNullException>(() => storage.AddRange<File>(null));
            Assert.IsTrue(storage.AddRange(Helper.GetRandomFiles(10)));
        }
        [TestMethod]
        public async Task AssertAddRangeAsync()
        {
            Assert.IsTrue(await storage.AddRangeAsync(Helper.GetRandomFiles(10)));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await storage.AddRangeAsync<File>(null));
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await storage.UpdateAsync(-4, f));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await storage.UpdateAsync<File>(4, null));

        }

        [TestMethod]
        public void AssertGetFiles()
        {
            List<File> files = new List<File>()
            {
                new File()
                {
                    Name = "aaa",
                    Versions = new List<FileVersion>()
                    {
                        new FileVersion()
                        {

                            Version = "0.0.1",
                            CreationTime = new DateTime(2020, 08, 10),
                            LastUpdatedTime = new DateTime(2020, 08, 10),
                            Path = ".",
                            Type = "."
                        }
                    }
                    
                },
                new File()
                { 
                    Versions = new List<FileVersion>()
                    {
                        new FileVersion()
                        {

                            Version = "0.1.1",
                            CreationTime = new DateTime(2020, 08, 11),
                            LastUpdatedTime = new DateTime(2020, 08, 11),
                            Path = ".",
                            Type = "."
                        }
                    },
                    Name = "aba",

                },
                new File()
                {
                    Name = "aaa",

                   Versions = new List<FileVersion>()
                   { 
                       new FileVersion()
                       {
                            Version = "0.0.2",
                            CreationTime = new DateTime(2020, 08, 12),
                            LastUpdatedTime = new DateTime(2020, 08, 12),
                            Path = ".",
                            Type = "."
                       }
                   }
                },
                new File()
                {
                   Name = "zzz",

                   Versions = new List<FileVersion>()
                   { 
                       new FileVersion()
                       {
                        Version = "0.0.5",
                        CreationTime = new DateTime(2020, 08, 13),
                        LastUpdatedTime = new DateTime(2020, 08, 13),
                        Path = ".",
                        Type = "."
                       }
                   }
                   
                }

            };

            storage.AddRange(files);

            var retrievedFiles = storage.GetFiles(new DateTime(2020, 08, 10), new DateTime(2020, 08, 11));
            Assert.IsTrue(retrievedFiles.Count() == 2);

            retrievedFiles = storage.GetFiles(new DateTime(2020, 08, 10), new DateTime(2020, 08, 15));
            Assert.IsTrue(retrievedFiles.Count() == 4);
            Assert.IsTrue(retrievedFiles.ElementAt(0).Name == "aaa" 
                && retrievedFiles.ElementAt(1).Name == "aaa"
                && retrievedFiles.ElementAt(2).Name == "aba"
                && retrievedFiles.ElementAt(3).Name == "zzz");

          
            retrievedFiles = storage.GetFiles("aaa");
            Assert.IsTrue(retrievedFiles.Count() == 2);

            retrievedFiles = storage.GetFiles("aaa", "0.0.5", "0.0.6");
            Assert.IsTrue(retrievedFiles.Count() == 0);

            retrievedFiles = storage.GetFiles("aaa", "0.0.2");
            Assert.IsTrue(retrievedFiles.Count() == 1);

        }
        /*
        [TestMethod]
        public async Task AssertGetFilesAsync()
        {
            List<File> files = new List<File>()
            {
                new File()
                {
                    Name = "aaa",
                    Version = "0.0.1",
                    CreationTime = new DateTime(2020, 08, 10),
                    LastUpdatedTime = new DateTime(2020, 08, 10),
                    Path = ".",
                    Type = "."

                },
                new File()
                {
                    Name = "aba",
                    Version = "0.1.1",
                    CreationTime = new DateTime(2020, 08, 11),
                    LastUpdatedTime = new DateTime(2020, 08, 11),
                    Path = ".",
                    Type = "."

                },
                new File()
                {
                    Name = "aaa",
                    Version = "0.0.2",
                    CreationTime = new DateTime(2020, 08, 12),
                    LastUpdatedTime = new DateTime(2020, 08, 12),
                    Path = ".",
                    Type = "."

                },
                new File()
                {
                    Name = "zzz",
                    Version = "0.0.5",
                    CreationTime = new DateTime(2020, 08, 13),
                    LastUpdatedTime = new DateTime(2020, 08, 13),
                    Path = ".",
                    Type = "."

                }

            };
            await storage.AddRangeAsync(files);

            await foreach (var file in storage.GetF )
            {

            }
        }
        */
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
 