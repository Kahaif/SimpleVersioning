using SimpleVersioning.Data;
using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Testing
{
    public static class Helper
    {
      
        public static List<File> GetRandomFiles(int length)
        {
            var res = new List<File>();
            res.PopulateWithRandomFiles(length);
            return res;
        }
        public static string GetRandomString(int length)
        {
            Random r = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[r.Next(s.Length)]).ToArray());
        }
    }
    static public class Extensions
    {
       
        public static void PopulateWithRandomFiles(this List<File> files, int count)
        {
            for (int i = 0; i < count; i++)
            {
                files.Add(new File()
                {
                    Name = Helper.GetRandomString(5),
                    Versions = new List<FileVersion>()
                    {
                        new FileVersion()
                        {
                            CreationTime = DateTime.Now,
                            LastUpdatedTime = DateTime.Now,
                            Type = "json",
                            Hash = Helper.GetRandomString(5),
                            Version = Helper.GetRandomString(2),
                            FileProperties = new List<FileProperty>()
                            {
                                    new FileProperty()
                                    {
                                        Name = Helper.GetRandomString(2),
                                        Value = Helper.GetRandomString(2)
                                    }
                            },
                            Path = Helper.GetRandomString(5)

                        }
                    }
                   
                });
            }
        }
        public static void DeleteAllFiles(this IStorageRepository storageRepo)
        {
            List<File> files = storageRepo.GetFiles().ToList();

            foreach (var file in files)
            {
                storageRepo.Delete<File>(file.Id);
            }
        }
    }
}
