using Domain.EfModels;
using Domain.Enums;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbConfigurationSettings
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("INGREDIENT");
            builder.HasKey(x => x.Id);

            builder
                .HasMany(r => r.RecipeIngredients)
                .WithOne(r => r.Ingredient)
                .HasForeignKey(r => r.IngredientId);
            builder.HasIndex(i => i.Name).IsUnique();
            builder
                .HasOne(i => i.Category)
                .WithMany(i => i.Ingredients)
                .HasForeignKey(i => i.IngredientCategoryId);


            builder.OwnsOne(i => i.Nutrients,
            nutrients =>
            {
                 nutrients.ToJson();

            });
            builder.Property(i => i.Name)
                .HasMaxLength(150)
                .IsRequired();
            builder
                .HasQueryFilter(q =>
                    EF.Property<bool>(q, "IsDeleted") == false
                    );
        }

    }
}
