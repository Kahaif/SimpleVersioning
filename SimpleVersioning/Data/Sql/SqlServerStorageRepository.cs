using Microsoft.EntityFrameworkCore;
using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Context.Set<T>().Remove(Context.Set<T>().Find(id));
            return Context.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync<T>(int id) where T : class
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            Context.Set<T>().Remove(await Context.Set<T>().FindAsync(id));
            return (await Context.SaveChangesAsync()) > 0;
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


        public IEnumerable<File> GetFiles(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "", FileSort sort = FileSort.Name)
        {
            if (from == null || to == null || name == null || minVersion == null || maxVersion == null) throw new ArgumentNullException(); 

            try
            {
                var query = Context.Files.BuildQueryWithComparison(name, minVersion, maxVersion).Where(file => DateTime.Compare(file.CreationTime, from) >= 0 && DateTime.Compare(file.CreationTime, to) <= 0).Sort(sort);
                
                return query.AsEnumerable();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<File> GetFiles(string name = "", string minVersion = "", string maxVersion = "", FileSort sort = FileSort.Name)
        {
            if (name == null || minVersion == null || maxVersion == null) throw new ArgumentNullException();

            try
            {
                var query = Context.Files.BuildQueryWithComparison(name, minVersion, maxVersion).Sort(sort);
                return query.AsEnumerable();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<File> GetFiles(List<Tuple<string, char, string>> propertyAndConditions, FileSort sort = FileSort.Name)
        {
            if (propertyAndConditions == null) throw new ArgumentNullException();
            if (propertyAndConditions.Count == 0) throw new ArgumentException();

            try
            {
                var query = Context.Files.CompareFileProperties(propertyAndConditions).Sort(sort);
                return query.AsEnumerable();
            }
            catch (Exception)
            {
                throw;
            }
        }
    
        public IAsyncEnumerable<File> GetFilesAsync(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "", FileSort sort = FileSort.Name)
        {
            if (from == null || to == null || name == null || minVersion == null || maxVersion == null) throw new ArgumentNullException();

            try
            {
                var query = Context.Files.BuildQueryWithComparison(name, minVersion, maxVersion).Where(file => file.CreationTime >= from && file.CreationTime <= to).Sort(sort);
                
                return query.AsAsyncEnumerable();
            }
            catch
            {
                throw;
            }
        }

        public IAsyncEnumerable<File> GetFilesAsync(string name = "", string minVersion = "", string maxVersion = "", FileSort sort = FileSort.Name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(minVersion) || string.IsNullOrEmpty(maxVersion)) throw new ArgumentNullException();

            try
            {
                var query = Context.Files.BuildQueryWithComparison(name, minVersion, maxVersion).Sort(sort);
;
                return query.AsAsyncEnumerable();
                
            }
            catch
            {
                throw;
            }
        }

        public IAsyncEnumerable<File> GetFilesAsync(List<Tuple<string, char, string>> propertyAndConditions, FileSort sort = FileSort.Name)
        {

            if (propertyAndConditions == null) throw new ArgumentNullException();
            if (propertyAndConditions.Count == 0) throw new ArgumentException();

            try
            {
                var query = Context.Files.CompareFileProperties(propertyAndConditions).Sort(sort);
                return query.AsAsyncEnumerable();
            }
            catch
            {
                throw;
            }
        }


        #endregion
    }
}
