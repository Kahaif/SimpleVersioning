using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SimpleVersioning.Data.SQLServer
{
    public class SqlServerStorage : IStorage
    {
        DbContextOptions options;

        public SqlServerStorage(DbContextOptions options)
        {
            this.options = options;
        }

        private void AddFileQuery(IQueryable<File> query, string name, string minVersion, string maxVersion)
        {
            if (name != "")
                query = query.Where(file => file.Name == name);

            if (minVersion != "")
                query = query.Where(file => string.Compare(file.Version, minVersion, true) != -1);

            if (maxVersion != "")
                query = query.Where(file => string.Compare(file.Version, maxVersion, true) != 1);
        } 

        private void AddFileQuery(IQueryable<File> query, List<Tuple<string, char, string>> propertyAndConditions)
        {
            foreach (var item in propertyAndConditions)
            {

                if (item.Item2 == '>')
                    query = query.Where(file => file.Properties.Where(prop => prop.Name == item.Item1 && string.Compare(item.Item3, prop.Value) > 0).Count() > 0);

                if (item.Item2 == '<')
                    query = query.Where(file => file.Properties.Where(prop => prop.Name == item.Item1 && string.Compare(item.Item3, prop.Value) < 0).Count() > 0);

                if (item.Item2 == '=')
                    query = query.Where(file => file.Properties.Where(prop => prop.Name == item.Item1 && string.Compare(item.Item3, prop.Value) == 0).Count() > 0);

                if (item.Item2 == '!')
                    query = query.Where(file => file.Properties.Where(prop => prop.Name == item.Item1 && prop.Value != item.Item3).Count() > 0);

            }
        }

            
        public Configuration GetConfiguration(string name)
        {
            if (name == null || name.Length < 1) throw new ArgumentException("Can't be null or empty", nameof(name));

            using var context = new SqlServerContext(options);
            try
            {
                return context.Configurations.AsNoTracking().Single(config => config.Name == name);
            }
            catch
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public Task<Configuration> GetConfigurationAsync(string name)
        {
            if (name == null || name.Length < 1) throw new ArgumentException("Can't be null or empty", nameof(name));

            using var context = new SqlServerContext(options);
            try
            {
                return context.Configurations.AsNoTracking().SingleAsync(config => config.Name == name);
            }
            catch
            {
                throw;
            }
            finally
            {
                context.DisposeAsync();
            }
        }

        public List<Configuration> GetConfigurations()
        {
            using var context = new SqlServerContext(options);
            try
            {
                return context.Configurations.AsNoTracking().ToList();
            }
            catch
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

       
        public Task<List<Configuration>> GetConfigurationsAsync()
        {
            using var context = new SqlServerContext(options);
            return context.Configurations.AsNoTracking().ToListAsync();
        }

        public File GetFile(int Id)
        {
            using var context = new SqlServerContext(options);
            try
            {
                return context.Files.AsNoTracking().Single(file => file.Id == Id);
            }
            catch
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public Task<File> GetFileAsync(int Id)
        {
            using var context = new SqlServerContext(options);
            try
            {
                return context.Files.AsNoTracking().SingleAsync(file => file.Id == Id);
            }
            catch 
            {
                throw;
            }
            finally
            {
                context.DisposeAsync();
            }
        }

        public List<File> GetFiles(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "")
        {
            if (from == null || to == null || name == null || minVersion == null || maxVersion == null) throw new ArgumentNullException(); 

            using var context = new SqlServerContext(options);
            try
            {
                var query =  context.Files.AsNoTracking().Where(file => file.CreationTime >= from && file.CreationTime <= to);

                AddFileQuery(query, name, minVersion, maxVersion);

                return query.ToList();
            }
            catch
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<File> GetFiles(string name = "", string minVersion = "", string maxVersion = "")
        {
            if (name == null || minVersion == null || maxVersion == null) throw new ArgumentNullException();

            using var context = new SqlServerContext(options);
            try
            {
                var query = context.Files.AsNoTracking();
                AddFileQuery(query, name, minVersion, maxVersion);
                return query.ToList();
            }
            catch
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<File> GetFiles(List<Tuple<string, char, string>> propertyAndConditions)
        {
            if (propertyAndConditions == null) throw new ArgumentNullException();

            using var context = new SqlServerContext(options);
            try
            {
                var query = context.Files.AsNoTracking();
                AddFileQuery(query, propertyAndConditions);
                return query.ToList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }
    

        public Task<List<File>> GetFilesAsync(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "")
        {
            if (from == null || to == null || name == null || minVersion == null || maxVersion == null) throw new ArgumentNullException();

            using var context = new SqlServerContext(options);
            try
            {
                var query = context.Files.AsNoTracking().Where(file => file.CreationTime >= from && file.CreationTime <= to);

                AddFileQuery(query, name, minVersion, maxVersion);

                return query.ToListAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                context.DisposeAsync();
            }
        }

        public Task<List<File>> GetFilesAsync(string name = "", string minVersion = "", string maxVersion = "")
        {
            if (name == null || minVersion == null || maxVersion == null) throw new ArgumentNullException();

            using var context = new SqlServerContext(options);
            try
            {
                var query = context.Files.AsNoTracking();

                AddFileQuery(query, name, minVersion, maxVersion);

                return query.ToListAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                context.DisposeAsync();
            }
        }

        public Task<List<File>> GetFilesAsync(List<Tuple<string, char, string>> propertyAndConditions)
        {
            if (propertyAndConditions == null) throw new ArgumentNullException();

            using var context = new SqlServerContext(options);
            try
            {
                var query = context.Files.AsNoTracking();
                AddFileQuery(query, propertyAndConditions);
                return query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                context.DisposeAsync();
            }
        }
    }
}
