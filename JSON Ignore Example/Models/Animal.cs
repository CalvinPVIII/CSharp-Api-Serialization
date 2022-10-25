using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Park.Models
{
    public class Animal
    {
        public int AnimalId { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        [Required]
        public string Species { get; set; }
        [Required]
        [Range(0, 200, ErrorMessage = "Age must be between 0 and 200")]
        public int Age { get; set; }
        public int LocationId { get; set; }
        [JsonIgnore]
        // This will prevent infinite serialization
        public virtual Location Location { get; set; }
    }
}