namespace SimpleVersioning.Models
{
    /// <summary>
    /// Represents a configuration in the storage system.
    /// </summary>
    public class ConfigurationModel
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
