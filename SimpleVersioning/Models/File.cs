using System;
using System.Collections.Generic;

namespace SimpleVersioning.Models
{
    /// <summary>
    /// Represents a file in the storage system.
    /// </summary>
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string Version { get; set; }
        public List<FileProperty> Properties { get; set; }
        public string FileType { get; set; }
        public string Path { get; set; }
    }
}
