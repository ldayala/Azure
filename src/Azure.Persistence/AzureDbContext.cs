
using Azure.Domain;
using Microsoft.EntityFrameworkCore;

namespace Azure.Persistence
{
    public class AzureDbContext(DbContextOptions<AzureDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your entities here
            //Realationships one to many between Category and Coffe
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Coffes)
                .WithOne(coffee => coffee.Category)
                .HasForeignKey(coffee => coffee.CategoryId)
                .IsRequired() // Ensure CategoryId is required in Coffe entity
                .OnDelete(DeleteBehavior.Cascade); // Optional: Define delete behavior, f deleting a Category will delete related Coffes
            modelBuilder.Entity<Coffe>()
                .Property(c => c.Price)
                .HasPrecision(10, 2); // Set precision for Price property in Coffe entity

            //Relationships many to many between Coffe and Ingredient
            modelBuilder.Entity<Coffe>()
                .HasMany(c => c.Ingredients)
                .WithMany(i => i.Coffes)
                .UsingEntity<CoffeIngredient>(
                    j => j
                        .HasOne(ci => ci.Ingredient)
                        .WithMany(i => i.CoffeIngredients)
                        .HasForeignKey(ci => ci.IngredientId),
                    j => j
                        .HasOne(ci => ci.Coffe)
                        .WithMany(c => c.CoffeIngredients)
                        .HasForeignKey(ci => ci.CoffeId),
                    j =>
                    {
                        j.HasKey(ci => new { ci.CoffeId, ci.IngredientId });
                    }
                );


            modelBuilder.Entity<Category>()
                .HasData(GetCategories()); // Seed initial categories from enum

        }
        // Define DbSet properties for your entities
        public required DbSet<Category> Categories { get; set; }
        public required DbSet<Coffe> Coffes { get; set; }
        public required DbSet<Ingredient> Ingredients { get; set; }

        private IEnumerable<Category> GetCategories()
        {
            return Enum.GetValues<CategoryEnum>().Select(p => Category.Create((int)p));
            
        }
    }
}
