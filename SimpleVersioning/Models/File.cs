using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleVersioning.Models
{
    /// <summary>
    /// Represents a file in the storage system.
    /// </summary>
    public class File
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Hash { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        [Required]
        public string Version { get; set; }

        public List<FileProperty> FileProperties { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Path { get; set; }
        public byte[] Content { get; set; }
    }
}
