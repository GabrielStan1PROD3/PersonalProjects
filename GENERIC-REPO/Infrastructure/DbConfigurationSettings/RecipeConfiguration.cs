using Domain.EfModels;
using Domain.Enums;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DbConfigurationSettings
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("RECIPE");

            builder.HasKey(x => x.Id);
            builder.HasMany(r=>r.RecipeIngredients).WithOne(r=> r.Recipe);
            builder.HasOne(r=> r.RecipeCategory).WithMany(r=>r.Recipes).HasForeignKey(r=>r.RecipeCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            ;

            builder.OwnsOne(r => r.Times, t => t.ToJson());
            builder.OwnsMany(r=>r.Steps,s=>s.ToJson());
            builder.Property(r => r.ImagesPath).HasConversion<ValueToListConverter<string>>();
            builder.Property(r => r.Dificulty).HasConversion<EnumToStringConverter<DificultyEnum>>();
            builder
            .HasQueryFilter(q =>
                EF.Property<bool>(q, "IsDeleted") == false
                );

        }
    }
}
