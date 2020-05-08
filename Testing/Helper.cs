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
    }
    static public class Extensions
    {
        public static string GetRandomString(this string str, int length)
        {
            Random r = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[r.Next(s.Length)]).ToArray());
        }
        public static void PopulateWithRandomFiles(this List<File> files, int count)
        {
            for (int i = 0; i < count; i++)
            {
                files.Add(new File()
                {
                    Id = new Random().Next(1, 100000000),
                    Name = "".GetRandomString(5),
                    CreationTime = DateTime.Now,
                    LastUpdatedTime = DateTime.Now,
                    Type = "json",
                    Hash = "".GetRandomString(5),
                    Version = "".GetRandomString(2),
                    Properties = new List<FileProperty>()
                    {
                        new FileProperty()
                        {
                            Name = "".GetRandomString(2),
                            Value = "".GetRandomString(2)
                        }
                    },
                    Path = "".GetRandomString(5)
                });
            }
        }
    }
}
