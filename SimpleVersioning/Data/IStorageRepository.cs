using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleVersioning.Data
{
    /// <summary>
    /// Classes which implement this interface will be able to store and retrieve Files and Configurations.
    /// </summary>
    public interface IStorageRepository
    {

        /// <summary>
        /// Asynchronously returns an instance of T.
        /// </summary>
        /// <param name="id">Entity's Id</param>
        /// <returns>A task that can be awaiten which returns an instance of T, null otherwise</returns>
        public Task<T> GetAsync<T>(int id) where T: class;

        /// <summary>
        /// Returns a instance of T.
        /// </summary>
        /// <param name="id">Entity's Id</param>
        /// <returns></returns>
        /// <returnsAn instance of T, null if it hasn't been found</returns>
        public T Get<T>(int id) where T: class;

        /// <summary>
        /// Asynchronously add an entity in the storage system.
        /// </summary>
        /// <returns>A task that can be awaiten which returns true if the entity has been correctly added, false otherwise</returns>
        public Task<bool> AddAsync<T>(T entity) where T : class;

        /// <summary>
        /// Add an entity in the storage system.
        /// </summary>
        /// <returns>True if the entity has been correctly added, false otherwise</returns>
        public bool Add<T>(T entity) where T : class;

        /// <summary>
        /// Asynchronously add a bunch of entities.
        /// </summary>
        /// <returns>An awaitable task which returns true if the same number of entities has been added, false otherwise</returns>
        public Task<bool> AddRangeAsync<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// Add a bunch of entities.
        /// </summary>
        /// <returns>True if the same number of entities has been added, false otherwise</returns>
        public bool AddRange<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// Asynchronously update the entity that has the given Id by newValues.
        /// </summary>
        /// <returns>An awaitable task which returns true if the update has been made, false otherwise</returns>
        public Task<bool> UpdateAsync<T>(int id, T newValues) where T : class;

        /// <summary>
        /// Update the entitty that has the given Id by newValues.
        /// </summary>
        /// <returns>True if the update has been made, false otherwise</returns>
        public bool Update<T>(int id, T newValues) where T : class;

        /// <summary>
        /// Asynchronously delete the entity that has the given id
        /// </summary>
        /// <returns>An awaitable task that returns true if the deletion has been made, false otherwise</returns>
        public Task<bool> DeleteAsync<T>(int id) where T : class;

        /// <summary>
        /// Delete the entity that has the given Id
        /// </summary>
        /// <returns>True if the deletion has been made, false otherwise</returns>
        public bool Delete<T>(int id) where T : class;

        /// <summary>
        /// Return a collection of every item of the given entity
        /// </summary>
        /// <returns>A collection of the asked items</returns>
        public IEnumerable<T> Get<T>() where T : class;

        /// <summary>
        /// Asynchronously returns a collection of every item of the given entity
        /// </summary>
        /// <returns>A collection of the asked items</returns>
        public Task<IEnumerable<T>> GetAsync<T>() where T : class;




        /*
         * 
  
        
        /// <summary>
        /// Return all the files which respects the parameters.
        /// </summary>
        /// <param name="from">Minimum creation date.</param>
        /// <param name="to">Maximum creation date.</param>
        /// <param name="name">Files names.</param>
        /// <param name="minVersion">Minimum version.</param>
        /// <param name="maxVersion">Maximum version.</param>
        /// <returns>A task that can be awaited wich returns a collection of Files.</returns>
        public Task<List<File>> GetFilesAsync(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "");

        /// <summary>
        /// Return all the files which respects the parameters.
        /// </summary>
        /// <param name="from">Minimum creation date.</param>
        /// <param name="to">Maximum creation date.</param>
        /// <param name="name">Files names.</param>
        /// <param name="minVersion">Minimum version.</param>
        /// <param name="maxVersion">Maximum version.</param>
        /// <returns>A collection of files.</returns>
        public List<File> GetFiles(DateTime from, DateTime to, string name = "", string minVersion = "", string maxVersion = "");

        /// <summary>
        /// Return all the files which respects the parameters.
        /// </summary>
        /// <param name="from">Minimum creation date.</param>
        /// <param name="to">Maximum creation date.</param>
        /// <param name="name">Files names.</param>
        /// <param name="minVersion">Minimum version.</param>
        /// <param name="maxVersion">Maximum version.</param>
        /// <returns>A task that can be awaited wich returns a collection of Files.</returns>
        public Task<List<File>> GetFilesAsync(string name = "", string minVersion = "", string maxVersion = "");

        /// <summary>
        /// Return all the files which respects the parameters.
        /// </summary>
        /// <param name="from">Minimum creation date.</param>
        /// <param name="to">Maximum creation date.</param>
        /// <param name="name">Files names.</param>
        /// <param name="minVersion">Minimum version.</param>
        /// <param name="maxVersion">Maximum version.</param>
        /// <returns>A collection of files.</returns>
        public List<File> GetFiles(string name = "", string minVersion = "", string maxVersion = "");

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
        public Task<List<File>> GetFilesAsync(List<Tuple<string, char, string>> propertyAndConditions);

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
        /// \The third item is the value which will be tested with the value of the property.</param>
        /// <returns>A collection of files.</returns>
        public List<File> GetFiles(List<Tuple<string, char, string>> propertyAndConditions);
        #endregion

        #region GetConfiguration

    /// <summary>
    /// Retrieve all the configurations asynchronously.
    /// </summary>
    /// <returns>An awaitable task which returns a collection of all the configurations.</returns>
    public Task<List<Configuration>> GetConfigurationsAsync();

    /// <summary>
    /// Return a collection of all the configurations.
    /// </summary>
    /// <returns>A collection of all the configurations.</returns>
    public List<Configuration> GetConfigurations();

    /// <summary>
    /// Return the configuration that has the specified name asynchronously.
    /// </summary>
    /// <param name="name">Configuration's name.</param>
    /// <returns>An awaitable task which returns the searched configuration.</returns>
    public Task<Configuration> GetConfigurationAsync(string name);

    /// <summary>
    /// Return the configuration that has the specified name.
    /// </summary>
    /// <param name="name">Configuration's name.</param>
    /// <return></return>sThe searched configuration</returns>
    public Configuration GetConfiguration(string name);
    #endregion

    #region AddFile
    /// <summary>
    /// Asynchronously add a file.
    /// </summary>
    /// <param name="file">New file to be added.</param>
    /// <returns>A task which can be awaited</returns>
    public Task<bool> AddFileAsync(File file);

    /// <summary>
    /// Add a file in the storage system .
    /// </summary>
    /// <param name="file">File to add</param>
    public bool AddFile(File file);

    /// <summary>
    /// Asynchronously a bunch of files in the storage system.
    /// </summary>
    /// <param name="files">Collection of files to add</param>
    public Task<bool> AddFilesAsync(IEnumerable<File> files);
    /// <summary>
    /// Add a bunch of files n the storage system.
    /// </summary>
    /// <param name="files"></param>
    public bool AddFiles(IEnumerable<File> files);
    #endregion

    #region AddConfiguration

    /// <summary>
    /// Asynchronously add a configuration in the storage system.
    /// </summary>
    /// <param name="configuraton">configuration to add.</param>
    public Task<bool> AddConfigurationAsync(Configuration configuraton);

    /// <summary>
    /// Add a configuration in the storage system
    /// </summary>
    /// <param name="configuration"></param>
    public bool AddConfiguration(Configuration configuration);

    /// <summary>
    /// Asynchronously add a collection of configuration in the storage system
    /// </summary>
    /// <param name="configurations">Collection of configurations to add</param>
    public Task<bool> AddConfigurationsAsync(IEnumerable<Configuration> configurations);

    /// <summary>
    /// Add a collection of configuration in the storage system
    /// </summary>
    /// <param name="configurations">Collection of configurations to add</param>
    public bool AddConfigurations(IEnumerable<Configuration> configurations);
    #endregion

    #region UpdateFile

    /// <summary>
    /// Asynchronously update a file with newFile.
    /// </summary>
    /// <param name="name">File's name</param>
    /// <param name="version">File's version</param>
    /// <param name="newFile">File's new values</param>
    /// <returns></returns>
    public Task<bool> UpdateFileAsync(string name, string version, File newFile);

    /// <summary>
    /// Update a file with newFile.
    /// </summary>
    /// <param name="name">File's name</param>
    /// <param name="version">File's version</param>
    /// <param name="newFile">File's new values</param>
    public bool UpdateFile(string name, string version, File newFile);

    #endregion

    #region UpdateConfiguration


    /// <summary>
    /// Asynchronously update a configuration.
    /// </summary>
    /// <param name="name">Name of the configuration which will be updated.</param>
    /// <param name="newValue">New value of the configuration</param>
    /// <returns></returns>
    public Task<bool> UpdateConfigurationAsync(string name, string newValue);

    /// <summary>
    /// Update a configuration.
    /// </summary>
    /// <param name="name">Name of the configuration which will be updated.</param>
    /// <param name="newValue">New value of the configuration.</param>
    public bool UpdateConfiguration(string name, string newValue);

    #endregion

    */
    }
}
