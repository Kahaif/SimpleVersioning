using System.ComponentModel.DataAnnotations;

namespace SimpleVersioning.Models
{
  
    public class FileProperty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
