using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleVersioning.Data
{
    [Flags]
    public enum FileSort: short
    {
        Version = 1,
        Name = 2,
        CreationTime = 4,
        LastUpdatedTime = 8
    }

    public enum Comparators: byte
    {
        Greater = (byte)'>',
        Lesser = (byte)'<',
        Different = (byte)'!',
        Same = (byte)'='
    }

    /// <summary>
    /// Prive an interface to a storage unit which can operate CRUD operations asynchronously.
    /// </summary>
    public interface IStorageRepositoryAsync
    {
        /// <summary>
        /// Check whether a file version exists or not 
        /// </summary>
        public Task<bool> FileVersionExistsAsync(string name, string version);

        /// <summary>
        /// Check whether a file exists or not
        /// </summary>
        public Task<bool> FileExistsAsync(string name);

        /// <summary>
        /// Asynchronously add an entity in the storage system.
        /// </summary>
        /// <returns>A task that can be awaiten which returns true if the entity has been correctly added, false otherwise</returns>
        public Task<bool> AddAsync<T>(T entity) where T : class;

        /// <summary>
        /// Asynchronously add a bunch of entities.
        /// </summary>
        /// <returns>An awaitable task which returns the added entities</returns>
        public Task<bool> AddRangeAsync<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// Asynchronously update the entity that has the given Id by newValues.
        /// </summary>
        /// <returns>An awaitable task which returns true if the update has been made, false otherwise</returns>
        public Task<bool> UpdateAsync<T>(int id, T newValues) where T : class;


        /// <summary>
        /// Asynchronously delete the entity that has the given id
        /// </summary>
        /// <returns>An awaitable task that returns true if the deletion has been made, false otherwise</returns>
        public Task<bool> DeleteAsync<T>(int id) where T : class;


        /// <summary>
        /// Asynchronously returns a collection of every item of the given entity
        /// </summary>
        /// <returns>A collection of the asked items</returns>
        public IAsyncEnumerable<T> GetAsync<T>() where T : class;

        /// <summary>
        /// Return all the files which respects the parameters.
        /// </summary>
        /// <param name="from">Minimum creation date.</param>
        /// <param name="to">Maximum creation date.</param>
        /// <param name="name">Files names.</param>
        /// <param name="minVersion">Minimum version.</param>
        /// <param name="maxVersion">Maximum version.</param>
        /// <returns>A task that can be awaited wich returns a collection of Files.</returns>
        public IAsyncEnumerable<File> GetFilesAsync(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "", FileSort sort = FileSort.Name);

     
        /// Return all the files which respects the parameters.
        /// </summary>
        /// <param name="from">Minimum creation date.</param>
        /// <param name="to">Maximum creation date.</param>
        /// <param name="name">Files names.</param>
        /// <param name="minVersion">Minimum version.</param>
        /// <param name="maxVersion">Maximum version.</param>
        /// <returns>A task that can be awaited wich returns a collection of Files.</returns>
        public IAsyncEnumerable<File> GetFilesAsync(string name = "", string minVersion = "", string maxVersion = "", FileSort sort = FileSort.Name);

        /// <summary>
        /// Return all the files which respect the conditions of propertyAndConditions.
        /// </summary>
        /// <param name="propertyAndConditions">A list of 3-items tuples of properties and conditions.
        /// \nThe first item is the property name which will be tested. 
        /// \nThe second item is the comparator. 
        /// \n> : the value of the property has to be greater than Item3
        /// \n< : the value of the property has to be smaller than Item3
        /// \n= : the value of the property has to be the same value than Item3
        /// \n! : the value of the property has to be different than Item3
        /// \nItem3: the value which will be tested</param>
        /// <returns>An awaitable task which will return a collection of files.</returns>
        public IAsyncEnumerable<File> GetFilesAsync(List<Tuple<string, char, string>> propertyAndConditions, FileSort sort = FileSort.Name);

       
    }
  
}
