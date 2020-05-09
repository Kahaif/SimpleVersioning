using Microsoft.EntityFrameworkCore;
using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SimpleVersioning.Data.Sql
{
    public class SqlServerStorageRepository : IStorageRepository
    {
        readonly DbContextOptions<SqlServerContext> options;

        public SqlServerStorageRepository(DbContextOptions<SqlServerContext> options)
        {
            this.options = options;
            Context = new SqlServerContext(options);
        }

        public SqlServerContext Context { get; }

        ~SqlServerStorageRepository()
        {
            Context.Dispose();
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
                query = item.Item2 switch
                {
                    '>' => query.Where(file => file.Properties.Where(prop => prop.Name == item.Item1 && string.Compare(item.Item3, prop.Value) > 0).Count() > 0),
                    '<' => query.Where(file => file.Properties.Where(prop => prop.Name == item.Item1 && string.Compare(item.Item3, prop.Value) < 0).Count() > 0),
                    '=' => query.Where(file => file.Properties.Where(prop => prop.Name == item.Item1 && string.Compare(item.Item3, prop.Value) == 0).Count() > 0),
                    '!' => query.Where(file => file.Properties.Where(prop => prop.Name == item.Item1 && prop.Value != item.Item3).Count() > 0),
                    _ => throw new ArgumentNullException(nameof(propertyAndConditions), $"Item2 of property name :  {item.Item1} and value : {item.Item3} incorrect"),
                };
            }
        }

        private void AddFileSorts(IQueryable<File> query, FileSort sort)
        {
            if ((sort & FileSort.Name) == FileSort.Name) query = query.OrderBy(x => x.Name);
            if ((sort & FileSort.Version) == FileSort.Version) query = query.OrderBy(x => x.Version);
            if ((sort & FileSort.LastUpdatedTime) == FileSort.LastUpdatedTime) query = query.OrderBy(x => x.LastUpdatedTime);
            if ((sort & FileSort.CreationTime) == FileSort.CreationTime) query = query.OrderBy(x => x.CreationTime);
        }

        #region IStorageRepository Implementation

        #region Generic Add
        public bool Add<T>(T entity) where T : class
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Context.Set<T>().Add(entity);
            return Context.SaveChanges() > 0;
        }

        public async Task<bool> AddAsync<T>(T entity) where T : class
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await Context.Set<T>().AddAsync(entity);
            return (await Context.SaveChangesAsync()) > 0;
        }

        public bool AddRange<T>(IEnumerable<T> entities) where T : class
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            int count = entities.Count();
            if (count == 0) throw new ArgumentException("can't be empty", nameof(entities));

            Context.Set<T>().AddRange(entities);
            return Context.SaveChanges() > 0;
        }

        public async Task<bool> AddRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            int count = entities.Count();
            if (count == 0) throw new ArgumentException("can't be empty", nameof(entities));

            await Context.Set<T>().AddRangeAsync(entities);
            return (await Context.SaveChangesAsync()) > 0;
        }

        #endregion

        #region Generic Get All
        public IEnumerable<T> Get<T>() where T : class
        {
            return Context.Set<T>().AsEnumerable();
        }

        public  IAsyncEnumerable<T> GetAsync<T>() where T : class
        {
            return Context.Set<T>().AsAsyncEnumerable();
        }
        #endregion

        #region Generic Delete
        public bool Delete<T>(int id) where T : class
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            return Context.Database.ExecuteSqlInterpolated($"DELETE FROM {typeof(T).Name} WHERE Id = {id}") > 0;
        }

        public async Task<bool> DeleteAsync<T>(int id) where T : class
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            return (await Context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM {typeof(T).Name} WHERE Id = {id}")) > 0;
        }
        #endregion
        #region Generic Get
        public T Get<T>(int id) where T : class
        {
           if (id < 1) throw new ArgumentNullException(nameof(id));

            return Context.Set<T>().Find(id);
        }

        public async Task<T> GetAsync<T>(int id) where T : class
        {
            if (id < 1) throw new ArgumentNullException(nameof(id));
            return await Context.Set<T>().FindAsync(id);
        }
        #endregion

        #region Generic Update

        public bool Update<T>(int id, T newValues) where T : class
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            if (newValues == null) throw new ArgumentNullException(nameof(newValues));

            Context.Entry(Get<File>(id)).CurrentValues.SetValues(newValues);
            return Context.SaveChanges() > 0;
        }

        public async Task<bool> UpdateAsync<T>(int id, T newValues) where T : class
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            if (newValues == null) throw new ArgumentNullException(nameof(newValues));

            Context.Entry(await Context.Set<T>().FindAsync(id)).CurrentValues.SetValues(newValues);
            return (await Context.SaveChangesAsync()) > 0;
        }
        #endregion


        public IEnumerable<File> GetFiles(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "", FileSort sort = FileSort.Name, bool orderByAsc = true)
        {
            if (from == null || to == null || name == null || minVersion == null || maxVersion == null) throw new ArgumentNullException(); 

            try
            {
                var query =  Context.Files.Where(file => file.CreationTime >= from && file.CreationTime <= to);

                AddFileQuery(query, name, minVersion, maxVersion);
                AddFileSorts(query, sort);
                
                return query.AsEnumerable();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<File> GetFiles(string name = "", string minVersion = "", string maxVersion = "", FileSort sort = FileSort.Name, bool orderByAsc = true)
        {
            if (name == null || minVersion == null || maxVersion == null) throw new ArgumentNullException();

            try
            {
                var query = Context.Files;
                AddFileQuery(query, name, minVersion, maxVersion);
                AddFileSorts(query, sort);
                return query.AsEnumerable();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<File> GetFiles(List<Tuple<string, char, string>> propertyAndConditions, FileSort sort = FileSort.Name, bool orderByAsc = true)
        {
            if (propertyAndConditions == null) throw new ArgumentNullException();
            if (propertyAndConditions.Count == 0) throw new ArgumentException();

            try
            {
                var query = Context.Files.AsNoTracking();
                AddFileQuery(query, propertyAndConditions);
                AddFileSorts(query, sort);
                return query.AsEnumerable();
            }
            catch (Exception)
            {
                throw;
            }
        }
    
        public IAsyncEnumerable<File> GetFilesAsync(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "", FileSort sort = FileSort.Name, bool orderByAsc = true)
        {
            if (from == null || to == null || name == null || minVersion == null || maxVersion == null) throw new ArgumentNullException();

            try
            {
                var query = Context.Files.Where(file => file.CreationTime >= from && file.CreationTime <= to);

                AddFileQuery(query, name, minVersion, maxVersion);
                AddFileSorts(query, sort);

                return query.AsAsyncEnumerable<File>();
            }
            catch
            {
                throw;
            }
        }

        public IAsyncEnumerable<File> GetFilesAsync(string name = "", string minVersion = "", string maxVersion = "", FileSort sort = FileSort.Name, bool orderByAsc = true)
        {

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(minVersion) || string.IsNullOrEmpty(maxVersion)) throw new ArgumentNullException();

            try
            {
                var query = Context.Files;

                AddFileQuery(query, name, minVersion, maxVersion);
                AddFileSorts(query, sort);
                return query.AsAsyncEnumerable<File>();
                
            }
            catch
            {
                throw;
            }
        }

        public IAsyncEnumerable<File> GetFilesAsync(List<Tuple<string, char, string>> propertyAndConditions, FileSort sort = FileSort.Name, bool orderByAsc = true)
        {

            if (propertyAndConditions == null) throw new ArgumentNullException();
            if (propertyAndConditions.Count == 0) throw new ArgumentException();

            try
            {
                var query = Context.Files;
                AddFileQuery(query, propertyAndConditions);
                AddFileSorts(query, sort);
                return query.AsAsyncEnumerable<File>();
            }
            catch
            {
                throw;
            }
        }


        #endregion
    }
}
