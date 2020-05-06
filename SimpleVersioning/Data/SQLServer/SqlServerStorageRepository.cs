using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SimpleVersioning.Data.SQLServer
{
    public class SqlServerStorageRepository : IStorageRepository
    {
        readonly DbContextOptions<SqlServerContext> options;
        readonly SqlServerContext context;

        public SqlServerStorageRepository(DbContextOptions<SqlServerContext> options)
        {
            this.options = options;
            context = new SqlServerContext(options);
        }

        public SqlServerContext Context { get => context; }

        ~SqlServerStorageRepository()
        {
            context.Dispose();
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

        #region IStorageRepository Implementation

        #region Generic Add
        public bool Add<T>(T entity) where T : class
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            context.Set<T>().Add(entity);
            return context.SaveChanges() > 0;
        }

        public async Task<bool> AddAsync<T>(T entity) where T : class
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await context.Set<T>().AddAsync(entity);
            return (await context.SaveChangesAsync()) > 0;
        }

        public bool AddRange<T>(IEnumerable<T> entities) where T : class
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            int count = entities.Count();
            if (count == 0) throw new ArgumentException("can't be empty", nameof(entities));

            context.Set<T>().AddRange(entities);
            return context.SaveChanges() > 0;
        }

        public async Task<bool> AddRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            int count = entities.Count();
            if (count == 0) throw new ArgumentException("can't be empty", nameof(entities));

            await context.Set<T>().AddRangeAsync(entities);
            return (await context.SaveChangesAsync()) > 0;
        }

        #endregion

        #region Generic Get All
        public IEnumerable<T> Get<T>() where T : class
        {
            return context.Set<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAsync<T>() where T : class
        {
            return await context.Set<T>().ToListAsync();
        }
        #endregion

        #region Generic Delete
        public bool Delete<T>(int id) where T : class
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            return context.Database.ExecuteSqlInterpolated($"DELETE FROM {typeof(T).Name} WHERE Id = {id}") > 0;
        }

        public async Task<bool> DeleteAsync<T>(int id) where T : class
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            return (await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM {typeof(T).Name} WHERE Id = {id}")) > 0;
        }
        #endregion
        #region Generic Get
        public T Get<T>(int id) where T : class
        {
           if (id < 1) throw new ArgumentNullException(nameof(id));

            return context.Set<T>().Find(id);
        }

        public async Task<T> GetAsync<T>(int id) where T : class
        {
            if (id < 1) throw new ArgumentNullException(nameof(id));
            return await context.Set<T>().FindAsync(id);
        }
        #endregion

        #region Generic Update

        public bool Update<T>(int id, T newValues) where T : class
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            if (newValues == null) throw new ArgumentNullException(nameof(newValues));

            context.Entry(context.Set<T>().Find(id)).CurrentValues.SetValues(newValues);
            return context.SaveChanges() > 0;
        }

        public async Task<bool> UpdateAsync<T>(int id, T newValues) where T : class
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            if (newValues == null) throw new ArgumentNullException(nameof(newValues));

            context.Entry(await context.Set<T>().FindAsync(id)).CurrentValues.SetValues(newValues);
            return (await context.SaveChangesAsync()) > 0;
        }
        #endregion



        /*
        #region GetConfiguration


        public Configuration GetConfiguration(string name)
        {
            if (name == null || name.Length < 1) throw new ArgumentException("Can't be null or empty", nameof(name));


            using var context = CreateContext();
            
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

        public async Task<Configuration> GetConfigurationAsync(string name)
        {
            if (name == null || name.Length < 1) throw new ArgumentException("Can't be null or empty", nameof(name));

            using var context = CreateContext();
            try
            {
                return await context.Configurations.AsNoTracking().SingleAsync(config => config.Name == name);
            }
            catch
            {
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public List<Configuration> GetConfigurations()
        {
            using var context = CreateContext();
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

        public async Task<List<Configuration>> GetConfigurationsAsync()
        {
            using var context = CreateContext();
            return await context.Configurations.AsNoTracking().ToListAsync();
        }

        #endregion

        #region GetFile
        public File GetFile(int Id)
        {
            using var context = CreateContext();
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

        public async Task<File> GetFileAsync(int Id)
        {
            using var context = CreateContext();
            try
            {
                return await context.Files.AsNoTracking().SingleAsync(file => file.Id == Id);
            }
            catch 
            {
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public List<File> GetFiles(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "")
        {
            if (from == null || to == null || name == null || minVersion == null || maxVersion == null) throw new ArgumentNullException(); 

            using var context = CreateContext();
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

            using var context = CreateContext();
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

            using var context = CreateContext();
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

            using var context = CreateContext();
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

            using var context = CreateContext();
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

            using var context = CreateContext();
            try
            {
                var query = context.Files.AsNoTracking();
                AddFileQuery(query, propertyAndConditions);
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

        #endregion

        #region AddFile
        public async Task<bool> AddFileAsync(File file)
        {
            if (file == null) throw new ArgumentNullException();
            using var context = CreateContext();
            try
            {
                await context.Files.AddAsync(file);
                return (await context.SaveChangesAsync()) > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public bool AddFile(File file)
        {
            if (file == null) throw new ArgumentNullException();
            using var context = CreateContext();
            try
            {
                context.Files.Add(file);
                return context.SaveChanges() > 0;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> AddFilesAsync(IEnumerable<File> files)
        {
            if (files == null) throw new ArgumentNullException();
            using var context = CreateContext();
            try
            {
                await context.Files.AddRangeAsync(files);
                return (await context.SaveChangesAsync()) == files.Count();
            }
            catch
            {
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public bool AddFiles(IEnumerable<File> files)
        {
            if (files == null) throw new ArgumentNullException();
            using var context = CreateContext();
            try
            {
                context.Files.AddRange(files);
                return context.SaveChanges() == files.Count();
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
        #endregion

        #region AddConfiguration
        public Task<bool> AddConfigurationAsync(Configuration configuraton)
        {
            if (configuraton == null) throw new ArgumentNullException();
            throw new Exception();
        }

        public bool AddConfiguration(Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException();
            throw new Exception();
        }

        public Task<bool> AddConfigurationsAsync(IEnumerable<Configuration> configurations)
        {
            if (configurations == null) throw new ArgumentNullException();
            throw new Exception();
        }

        public bool AddConfigurations(IEnumerable<Configuration> configurations)
        {
            if (configurations == null) throw new ArgumentNullException();
            throw new Exception();
        }
        #endregion
        #region UpdateFile

        public Task<bool> UpdateFileAsync(int id, File newFile)
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            if (newFile == null) throw new ArgumentNullException(nameof(newFile));
            throw new Exception();
        }

        public bool UpdateFile(int id, File newFile)
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            if (newFile == null) throw new ArgumentNullException(nameof(newFile));
            throw new Exception();
        }

        public Task<bool> UpdateFileAsync(string name, string version, File newFile)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            if (string.IsNullOrEmpty(version)) throw new ArgumentException(nameof(name));
            if (newFile == null) throw new ArgumentNullException(nameof(newFile));
            throw new Exception();
        }

        public bool UpdateFile(string name, string version, File newFile)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            if (string.IsNullOrEmpty(version)) throw new ArgumentException(nameof(version));
            if (newFile == null) throw new ArgumentNullException(nameof(newFile));
            throw new Exception();
        }

        #endregion

        #region UpdateConfiguration
        public Task<bool> UpdateConfigurationAsync(int id, string newValue)
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            if (string.IsNullOrEmpty(newValue)) throw new ArgumentException(nameof(newValue));
            throw new Exception();
        }

        public bool UpdateConfiguration(int id, string newValue)
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            if (string.IsNullOrEmpty(newValue)) throw new ArgumentException(nameof(newValue));
            throw new Exception();
        }

        public Task<bool> UpdateConfigurationAsync(string name, string newValue)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            if (string.IsNullOrEmpty(newValue)) throw new ArgumentException(nameof(newValue));
            throw new Exception();
        }

        public bool UpdateConfiguration(string name, string newValue)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            if (string.IsNullOrEmpty(newValue)) throw new ArgumentException(nameof(newValue));
            throw new Exception();
        }
        #endregion

        */
        #endregion
    }
}
