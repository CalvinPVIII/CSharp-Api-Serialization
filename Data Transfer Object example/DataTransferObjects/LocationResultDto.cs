using System.Collections.Generic;
using Park.Models;

// This class is essentially the same as our Location class, except we convert all Animals in the List to AnimalDtos

namespace Park.DataTransferObjects
{
    public class LocationResultDto
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public List<AnimalDto> Animals { get; set; }

        public LocationResultDto(Location location)
        {
            // Taking the relevant data from a location and reshaping it to our DTO
            LocationId = location.LocationId;
            LocationName = location.LocationName;
            Animals = new List<AnimalDto>() { };
            //  Converts all animals to AnimalDtos
            foreach (Animal animal in location.Animals)
            {
                Animals.Add(new AnimalDto(animal));
            }

        }
    }
}