using Microsoft.EntityFrameworkCore;
using SimpleVersioning.Extensions;
using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleVersioning.Data.Sql
{

    public class MariaDBStorageRepository : IStorageRepositoryAsync
    {

        public MariaDBStorageRepository(DbContextOptions<MariaDBServerContext> options) => Context = new MariaDBServerContext(options);

        public MariaDBServerContext Context { get; }

        ~MariaDBStorageRepository() => Context.Dispose();


        #region IStorageRepository Implementation

        #region Generic Add
       

        public async Task<bool> AddAsync<T>(T entity) where T : class
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            await Context.Set<T>().AddAsync(entity);
            return (await Context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> AddRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            _ = entities ?? throw new ArgumentNullException(nameof(entities));

            int count = entities.Count();
            if (count == 0) throw new ArgumentException("can't be empty", nameof(entities));
            try
            {
                await Context.Set<T>().AddRangeAsync(entities);

                return (await Context.SaveChangesAsync()) > 0;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Generic Get All

        public IAsyncEnumerable<T> GetAsync<T>() where T : class => Context.Set<T>().AsAsyncEnumerable();
        #endregion

        #region Generic Delete
   
        public async Task<bool> DeleteAsync<T>(int id) where T : class
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            try
            {
                Context.Set<T>().Remove(await Context.Set<T>().FindAsync(id));
                return (await Context.SaveChangesAsync()) > 0;

            }
            catch 
            {
                throw;
            }
        }
        #endregion
    

        #region Generic Update

 
        public async Task<bool> UpdateAsync<T>(int id, T newValues) where T : class
        {
            if (id < 1) throw new ArgumentException(nameof(id));
            if (newValues == null) throw new ArgumentNullException(nameof(newValues));

            Context.Entry(await Context.Set<T>().FindAsync(id)).CurrentValues.SetValues(newValues);
            return (await Context.SaveChangesAsync()) > 0;
        }
        #endregion

        public IAsyncEnumerable<File> GetFilesAsync(DateTime from, DateTime to, string resourceName = "", string minVersion = "", string maxVersion = "", FileSort sort = FileSort.Name)
        {
            _ = resourceName ?? throw new ArgumentNullException(nameof(resourceName));
            _ = minVersion ?? throw new ArgumentNullException(nameof(minVersion));
            _ = maxVersion ?? throw new ArgumentNullException(nameof(maxVersion));

            try
            {
                var query = Context.Files.BuildQueryWithComparison(resourceName, minVersion, maxVersion)
                    .Where(file => file.Versions.Where(version => DateTime.Compare(version.CreationTime, from) >= 0 && DateTime.Compare(version.CreationTime, to) <= 0).Count() > 0)
                    .Sort(sort);
                
                return query.AsAsyncEnumerable(); 
            }
            catch
            {
                throw;
            }
        }

        public IAsyncEnumerable<File> GetFilesAsync(string resourceName = "", string minVersion = "", string maxVersion = "", FileSort sort = FileSort.Name)
        {
            _ = resourceName ?? throw new ArgumentNullException(nameof(resourceName));
            _ = minVersion ?? throw new ArgumentNullException(nameof(minVersion));
            _ = maxVersion ?? throw new ArgumentNullException(nameof(maxVersion));
           
            try
            {
                var query = Context.Files.BuildQueryWithComparison(resourceName, minVersion, maxVersion).Sort(sort);
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

        #region Exists

        public async Task<bool> FileVersionExistsAsync(string name, string version)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(name);
            if (string.IsNullOrWhiteSpace(version)) throw new ArgumentNullException(version);

            try
            {
                return await Context.Files.Where(f => f.Name == name && f.Versions.Where(v => v.Version == version).Any()).AnyAsync();

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> FileExistsAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(name);

            try
            {
                return await Context.Files.Where(f => f.Name == name).AnyAsync();
            }
            catch
            {
                throw;
            }
        }
        #endregion
        #endregion
    }
}
