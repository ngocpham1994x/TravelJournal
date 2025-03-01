using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace TravelJournal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        // add-migrations and update-database
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            try
            {
                var dbCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (dbCreator != null)
                {
                    if (!dbCreator.CanConnect()) dbCreator.Create();
                    if (!dbCreator.HasTables()) dbCreator.CreateTables();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // define composite keys
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.City>()
                .HasKey(c => new { c.CityName, c.CountryName});

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TravelJournal.Models.Location> Location { get; set; } = default!;
        public DbSet<TravelJournal.Models.City> City { get; set; } = default!;

    }
}
