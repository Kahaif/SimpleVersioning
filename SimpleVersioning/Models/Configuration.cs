namespace SimpleVersioning.Models
{
    /// <summary>
    /// Represents a configuration in the storage system.
    /// </summary>
    public class Configuration
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
