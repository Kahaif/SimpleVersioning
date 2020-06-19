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
      
        public List<FileVersion> Versions { get; set; }
    }
}
