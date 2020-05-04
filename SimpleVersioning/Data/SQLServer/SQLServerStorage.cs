using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleVersioning.Data.SQLServer
{
    public class SQLServerStorage : IStorage
    {
        public Task<Configuration> GetConfigurationAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Configuration> GetConfigurationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Configuration>> GetConfigurationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Configuration>> GetConfigurationsAsync(object value)
        {
            throw new NotImplementedException();
        }

        public Task<File> GetFileAsync(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<File>> GetFilesAsync(List<Tuple<string, string>> propertyAndConditions)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<File>> GetFilesAsync(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "")
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<File>> GetFilesAsync(string name = "", string minVersion = "", string maxVersion = "")
        {
            throw new NotImplementedException();
        }
    }
}
