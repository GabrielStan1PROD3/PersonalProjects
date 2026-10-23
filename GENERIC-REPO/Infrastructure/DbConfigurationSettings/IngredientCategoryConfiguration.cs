using Domain.EfModels;
using Domain.Enums;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbConfigurationSettings
{
    public class IngredientCategoryConfiguration : IEntityTypeConfiguration<IngredientCategory>
    {
        public void Configure(EntityTypeBuilder<IngredientCategory> builder)
        {
            builder.ToTable("INGREDIENT_CATEGORY");
            builder.HasKey(x => x.Id);
            builder.HasMany(rc => rc.Ingredients).WithOne(i => i.Category).HasForeignKey(i => i.IngredientCategoryId);
            builder.Property(rc => rc.Name)
                      .HasConversion<EnumToStringConverter<IngredientCategoryEnum>>();
            builder
            .HasQueryFilter(q =>
                EF.Property<bool>(q, "IsDeleted") == false
                );
        }
    }
}
