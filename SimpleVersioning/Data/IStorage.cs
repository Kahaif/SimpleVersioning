using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleVersioning.Data
{
    /// <summary>
    /// Classes which implement this interface will be able to store and retrieve Files and Configurations.
    /// </summary>
    interface IStorage
    {
        public Task<File> GetFileAsync(int ID);
        public Task<IEnumerable<File>> GetFilesAsync(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "");
        public Task<IEnumerable<File>> GetFilesAsync(string name = "", string minVersion = "", string maxVersion = "");
        public Task<IEnumerable<File>> GetFilesAsync(List<Tuple<string, string>> propertyAndConditions);
        public Task<IEnumerable<Configuration>> GetConfigurationsAsync();
        public Task<Configuration> GetConfigurationAsync(string name);
        public Task<IEnumerable<Configuration>> GetConfigurationsAsync(object value);
        public Task<Configuration> GetConfigurationAsync(int id);
    }
}
