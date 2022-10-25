using Microsoft.EntityFrameworkCore;
namespace Park.Models
{
    public class ParkContext : DbContext
    {


        public DbSet<Animal> Animals { get; set; }
        public DbSet<Location> Locations { get; set; }
        public ParkContext(DbContextOptions<ParkContext> options)
           : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Location>()
            .HasData(
                new Location { LocationId = 1, LocationName = "The Forest" },
                new Location { LocationId = 2, LocationName = "The Desert" }
            );
            builder.Entity<Animal>()
                .HasData(
                    new Animal { AnimalId = 1, Name = "Matilda", Species = "Woolly Mammoth", Age = 7, LocationId = 1 },
                    new Animal { AnimalId = 2, Name = "Rexie", Species = "Dinosaur", Age = 10, LocationId = 1 },
                    new Animal { AnimalId = 3, Name = "Matilda", Species = "Dinosaur", Age = 2, LocationId = 2 },
                    new Animal { AnimalId = 4, Name = "Pip", Species = "Shark", Age = 4, LocationId = 1 },
                    new Animal { AnimalId = 5, Name = "Bartholomew", Species = "Dinosaur", Age = 22, LocationId = 2 }
                );
        }

    }

}