using Domain.EfModels;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dto_s
{
    public class RecipeCategoryDto :IDto
    {
        public int RecipeCategoryId { get; set; }
        public MealTypeEnum MealType { get; set; }

        public DishTypeEnum DishType { get; set; }

        public DietTypeEnum DietType { get; set; }

        public CountryTypeEnum CountryType { get; set; }

        public LifestyleTypeEnum LifestyleType { get; set; }

        public static explicit operator RecipeCategoryDto(RecipeCategory entity)
        {
            return new RecipeCategoryDto
            {
                RecipeCategoryId = entity.Id,
                MealType = entity.MealType,
                DishType = entity.DishType,
                DietType = entity.DietType,
                CountryType = entity.CountryType,
                LifestyleType = entity.LifestyleType,
            };
        }
    }

}
