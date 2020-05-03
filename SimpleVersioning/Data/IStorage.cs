using SimpleVersioning.Models;
using System;
using System.Collections.Generic;

namespace SimpleVersioning.Data
{
    /// <summary>
    /// Classes which implement this interface will be able to store and retrieve Files and Configurations.
    /// </summary>
    interface IStorage
    {
        public File GetFile(int ID);
        public IEnumerable<File> GetFiles(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "");
        public IEnumerable<Configuration> GetConfigurations();
        public Configuration GetConfiguration(string name);
        public Configuration GetConfiguration(object value);
        public Configuration GetConfiguration(int id);
    }
}
