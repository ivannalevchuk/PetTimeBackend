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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=petrimedb.database.windows.net;" +
        "Database=PetTimeDB;" +
        "Trusted_Connection=False;" +
        "Encrypt=True;" +
        "User Id=PetTimeDB;" +
        "Password=sql!Admin");
        }

        public DbSet <User> Users { get; set; }
        public DbSet <Pet> Pets { get; set; }
        public DbSet <Breed> Breeds { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet <Event> Events { get; set; }
        public DbSet <Place> Places { get; set; }

    }
}
