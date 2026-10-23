using Domain.EfModels;
using Domain.Enums;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DbConfigurationSettings
{
    public class RecipeCategoryConfiguration : IEntityTypeConfiguration<RecipeCategory>
    {
        public void Configure(EntityTypeBuilder<RecipeCategory> builder)
        {
            builder.ToTable("RECIPE_CATEGORY");
         
            builder.HasKey(x => x.Id);
           
            builder.HasMany(rc => rc.Recipes)
                .WithOne(r => r.RecipeCategory);

            builder.Property(ri => ri.DietType)
                .HasConversion<EnumToStringConverter<DietTypeEnum>>();
           
            builder.Property(ri => ri.CountryType)
                .HasConversion<EnumToStringConverter<CountryTypeEnum>>();
           
            builder.Property(ri => ri.LifestyleType)
                .HasConversion<EnumToStringConverter<LifestyleTypeEnum>>();
           
            builder.Property(ri => ri.DishType)
                .HasConversion<EnumToStringConverter<DishTypeEnum>>();
           
            builder.Property(ri => ri.MealType)
                .HasConversion<EnumToStringConverter<MealTypeEnum>>();
            builder
            .HasQueryFilter(q =>
                EF.Property<bool>(q, "IsDeleted") == false
                );

        }
    }
}
