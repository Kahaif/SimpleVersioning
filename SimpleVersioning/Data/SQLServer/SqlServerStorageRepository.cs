using Microsoft.EntityFrameworkCore;
using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleVersioning.Data.SQLServer
{
    public class SqlServerStorageRepository : IStorageRepository
    {
        readonly DbContextOptions<SqlServerContext> options;
        private readonly SqlServerContext context;

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
                switch (item.Item2)
                {
                    case '>':
                        query = query.Where(file => file.Properties.Where(prop => prop.Name == item.Item1 && string.Compare(item.Item3, prop.Value) > 0).Count() > 0);
                        break;
                    case '<':
                        query = query.Where(file => file.Properties.Where(prop => prop.Name == item.Item1 && string.Compare(item.Item3, prop.Value) < 0).Count() > 0);
                        break;
                    case '=':
                        query = query.Where(file => file.Properties.Where(prop => prop.Name == item.Item1 && string.Compare(item.Item3, prop.Value) == 0).Count() > 0);
                        break;

                    case '!':
                        query = query.Where(file => file.Properties.Where(prop => prop.Name == item.Item1 && prop.Value != item.Item3).Count() > 0);
                        break;

                    default:
                        throw new ArgumentNullException(nameof(propertyAndConditions), $"Item2 of property name :  {item.Item1} and value : {item.Item3} incorrect");
                }
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

            context.Entry(Get<File>(id)).CurrentValues.SetValues(newValues);

            /*
            T entity = context.Set<T>().Find(id);
            entity = newValues;
            context.Set<T>().Update(entity);*/
            //context.Entry(context.Set<T>().Find(id)).CurrentValues.SetValues(newValues);
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



        public List<File> GetFiles(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "")
        {
            if (from == null || to == null || name == null || minVersion == null || maxVersion == null) throw new ArgumentNullException(); 

            try
            {
                var query =  context.Files.Where(file => file.CreationTime >= from && file.CreationTime <= to);

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

            try
            {
                var query = context.Files;
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
            if (propertyAndConditions.Count == 0) throw new ArgumentException();

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

            try
            {
                var query = context.Files.Where(file => file.CreationTime >= from && file.CreationTime <= to);

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
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(minVersion) || string.IsNullOrEmpty(maxVersion)) throw new ArgumentNullException();

            try
            {
                var query = context.Files;

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
            if (propertyAndConditions.Count == 0) throw new ArgumentException();

            try
            {
                var query = context.Files;
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
    }
}
