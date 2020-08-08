using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleVersioning.Models
{
    /// <summary>
    /// Represents a file in the storage system.
    /// </summary>
    public class File
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string ResourceName { get; set; }
        [Required]
        public List<FileVersion> Versions { get; set; }
        public List<FileProperty> Properties { get; set; }


    }
}
