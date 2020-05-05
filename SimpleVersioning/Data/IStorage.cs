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
        #region GetFile
        /// <summary>
        /// Return a single file asynchronously.
        /// </summary>
        /// <param name="Id">Id of the file to be searched </param>
        /// <returns>A task that can be awaited wich returns a File object (from Simpleversioning.Models)</returns>
        public Task<File> GetFileAsync(int Id);

        /// <summary>
        /// Return a single file synchronously.
        /// </summary>
        /// <param name="Id">Id of the file to be returned</param>
        /// <returns>Returns a File object (from Simpleversioning.Models)</returns>
        public File GetFile(int Id);

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
        public Task AddFileAsync(File file);

        /// <summary>
        /// Add a file in the storage system .
        /// </summary>
        /// <param name="file">File to add</param>
        public void AddFile(File file);

        /// <summary>
        /// Asynchronously a bunch of files in the storage system.
        /// </summary>
        /// <param name="files">Collection of files to add</param>
        public void AddFilesAsync(IEnumerable<File> files);
        /// <summary>
        /// Add a bunch of files n the storage system.
        /// </summary>
        /// <param name="files"></param>
        public void AddFiles(IEnumerable<File> files);
        #endregion

        #region AddConfiguration

        /// <summary>
        /// Asynchronously add a configuration in the storage system.
        /// </summary>
        /// <param name="configuraton">configuration to add.</param>
        public Task AddConfigurationAsync(Configuration configuraton);

        /// <summary>
        /// Add a configuration in the storage system
        /// </summary>
        /// <param name="configuration"></param>
        public void AddConfiguration(Configuration configuration);

        /// <summary>
        /// Asynchronously add a collection of configuration in the storage system
        /// </summary>
        /// <param name="configurations">Collection of configurations to add</param>
        public Task AddConfigurationsAsync(IEnumerable<Configuration> configurations);

        /// <summary>
        /// Asynchronously add a collection of configuration in the storage system
        /// </summary>
        /// <param name="configurations">Collection of configurations to add</param>
        public void AddConfigurations(IEnumerable<Configuration> configurations);
        #endregion

        #region UpdateFile
        /// <summary>
        /// Asynchronously update a file with newFile
        /// </summary>
        /// <param name="id">File's Id</param>
        /// <param name="newFile">File's new values</param>
        public Task UpdateFileAsync(int id, File newFile);

        /// <summary>
        /// Update a file with newFile.
        /// </summary>
        /// <param name="id">File's Id</param>
        /// <param name="newFile">File's new values</param>
        public void UpdateFile(int id, File newFile);

        /// <summary>
        /// Asynchronously update a file with newFile.
        /// </summary>
        /// <param name="name">File's name</param>
        /// <param name="version">File's version</param>
        /// <param name="newFile">File's new values</param>
        /// <returns></returns>
        public Task UpdateFileAsync(string name, string version, File newFile);

        /// <summary>
        /// Update a file with newFile.
        /// </summary>
        /// <param name="name">File's name</param>
        /// <param name="version">File's version</param>
        /// <param name="newFile">File's new values</param>
        public void UpdateFile(string name, string version, File newFile);

        #endregion

        #region UpdateConfiguration

        /// <summary>
        /// Asynchronously update a configuration
        /// </summary>
        /// <param name="id">Id of the configuration which will be updated.</param>
        /// <param name="newValue">New value of the configuration.</param>
        /// <returns></returns>
        public Task UpdateConfigurationAsync(int id, string newValue);

        /// <summary>
        /// Update a configuration
        /// </summary>
        /// <param name="id">Name of the configuration which will be updated.</param>
        /// <param name="newValue">New value of the configuration.</param>
        public void UpdateConfiguration(int id, string newValue);


        /// <summary>
        /// Asynchronously update a configuration.
        /// </summary>
        /// <param name="name">Name of the configuration which will be updated.</param>
        /// <param name="newValue">New value of the configuration</param>
        /// <returns></returns>
        public Task UpdateConfigurationAsync(string name, string newValue);

        /// <summary>
        /// Update a configuration.
        /// </summary>
        /// <param name="name">Name of the configuration which will be updated.</param>
        /// <param name="newValue">New value of the configuration.</param>
        public void UpdateConfiguration(string name, string newValue);

        #endregion

        #region Delete
        /// <summary>
        /// Delete the file that has the given ID.
        /// </summary>
        /// <param name="id">File's id</param>
        public void DeleteFile(int id);

        /// <summary>
        /// Delete the file that has the given name and version.
        /// </summary>
        /// <param name="name">File's name</param>
        /// <param name="version">File's version.</param>
        public void DeleteFile(string name, string version);

        /// <summary>
        /// Delete the configuration that has the given id.
        /// </summary>
        /// <param name="id">Configuration's id.</param>
        public void DeleteConfiguration(int id);

        /// <summary>
        /// Delete the configuration that has the given name.
        /// </summary>
        /// <param name="name">Configuration's name.</param>
        public void DeleteConfiguration(string name);

        #endregion

    }
}
