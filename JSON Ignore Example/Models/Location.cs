using System.Collections.Generic;

namespace Park.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }

        public Location()
        {
            this.Animals = new HashSet<Animal>() { };
        }
    }

}