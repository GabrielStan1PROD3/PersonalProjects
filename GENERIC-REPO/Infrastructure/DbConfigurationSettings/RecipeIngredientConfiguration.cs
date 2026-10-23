using Domain.EfModels;
using Domain.Enums;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbConfigurationSettings
{
    public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            builder.ToTable("RECIPE_INGREDIENT");
            builder.HasKey(ri => ri.Id);

            builder.HasOne(ri => ri.Recipe)
                   .WithMany(r => r.RecipeIngredients)
                   .HasForeignKey(ri => ri.RecipeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ri => ri.Ingredient)
                   .WithMany(i => i.RecipeIngredients)
                   .HasForeignKey(ri => ri.IngredientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(ri => ri.QuantityInUnit).HasPrecision(10, 2);
            builder.Property(ri=> ri.Unit).HasConversion<EnumToStringConverter<UnitEnum>>();

            builder.HasQueryFilter(ri => EF.Property<bool>(ri, "IsDeleted") == false);

          

        }
    }
}
