using Domain.EfModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbConnection
{
    public class CookingAppContext : DbContext
    {
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeCategory> RecipeCategory { get; set; }
        public DbSet<IngredientCategory> IngredientCategory { get; set; }

        public CookingAppContext()
        {

        }
        public CookingAppContext(DbContextOptions options) : base(options)
        {

        }
   
        public override Task<int> SaveChangesAsync( CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CookingAppContext).Assembly);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(x => x.ClrType != null && !x.IsOwned()))
            {
                var builder = modelBuilder.Entity(entityType.ClrType);

                builder
                    .Property<bool>("IsDeleted")
                    .HasDefaultValue(false);

                builder
                    .Property<DateTime>("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");

                builder
                    .Property<DateTime>("UpdatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");

                builder
                    .Property<byte[]>("Version")
                    .IsRowVersion();

            }
            
        }
        private void ApplyAuditInfo()
        {
            var entries = ChangeTracker.Entries()
                .Where(e =>
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                // 1. IGNORĂ owned types (ex: NutrientsModel)
                if (entry.Metadata.IsOwned())
                {
                    continue;
                }
                // 2. IGNORĂ entitățile fără aceste proprietăți
                if (!entry.Properties.Any(p => p.Metadata.Name == "CreatedAt"))
                {
                    continue;
                }
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Property("IsDeleted").CurrentValue = true;
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }



    }

}
   

