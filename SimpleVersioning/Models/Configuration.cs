﻿namespace SimpleVersioning.Models
{
    /// <summary>
    /// Represents a configuration in the storage system.
    /// </summary>
    public class Configuration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
