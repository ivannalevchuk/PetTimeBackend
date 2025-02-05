using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetTimeBackend.Entities;

namespace PetTimeBackend.Contexts
{
    public class PetTimeContext : DbContext
    {
        public PetTimeContext()
        {

        }
        public PetTimeContext(DbContextOptions<PetTimeContext> options) : base(options)
        {

        }

        public DbSet <User> Users { get; set; }
        public DbSet <Pet> Pets { get; set; }
        public DbSet <Breed> Breeds { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet <Event> Events { get; set; }
        public DbSet <Place> Places { get; set; }
        public DbSet <City> Cities { get; set; }
        public DbSet <Country> Countries { get; set; }
        public DbSet <Vaccination> Vaccinations { get; set; }
        public DbSet <Report> Reports { get; set; }


    }
}
