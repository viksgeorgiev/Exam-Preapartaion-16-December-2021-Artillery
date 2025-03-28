using Artillery.Data.Models;

namespace Artillery.Data
{
    using Microsoft.EntityFrameworkCore;

    public class ArtilleryContext : DbContext
    {
        public ArtilleryContext()
        {
        }

        public ArtilleryContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<CountryGun> CountriesGuns { get; set; } = null!;
        public virtual DbSet<Gun> Guns { get; set; } = null!;
        public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;
        public virtual DbSet<Shell> Shells { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>()
                .HasIndex(e => e.ManufacturerName)
                .IsUnique();

            modelBuilder.Entity<CountryGun>(e =>
                e.HasKey(k => new { k.CountryId, k.GunId }));
        }
    }
}
