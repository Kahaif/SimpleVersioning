using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleVersioning.Models
{
    public class FileVersion
    {
        public int Id { get; set; }
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

        public File File { get; set; }

    }
}
