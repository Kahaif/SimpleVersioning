using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleVersioning.Models
{
    public class FileVersion
    {
        [Key]
        public int Id { get; set; }

        public string Hash { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        [Required]
        public DateTime LastUpdatedTime { get; set; }

        [Required]
        public string Version { get; set; }
        [Required]
        public string Type { get; set; }

        public string Path { get; set; }

        public string Description { get; set; }

        public byte[] Content { get; set; }
        
    }
}
