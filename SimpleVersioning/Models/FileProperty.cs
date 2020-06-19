using System.ComponentModel.DataAnnotations;

namespace SimpleVersioning.Models
{
  
    public class FileProperty
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }

        public File File { get; set; }
    }
}
