using Park.Models;

// The same as our Regular animal class, except it doesn't have a location property

namespace Park.DataTransferObjects
{
    public class AnimalDto
    {
        public int AnimalId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }


        //      Constructor takes in an Animal and converts it to AnimalDto by removing Location
        public AnimalDto(Animal animal)
        {
            AnimalId = animal.AnimalId;
            Name = animal.Name;
            Species = animal.Species;

        }
    }
}