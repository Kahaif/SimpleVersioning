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
    }
}
