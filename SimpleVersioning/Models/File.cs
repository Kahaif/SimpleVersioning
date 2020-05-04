using System;
using System.Collections.Generic;

namespace SimpleVersioning.Models
{
    /// <summary>
    /// Represents a file in the storage system.
    /// </summary>
    public class File
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public Version Version { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public string FileType { get; set; }
    }
}
