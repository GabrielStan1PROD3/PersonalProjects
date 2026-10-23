using Domain.Dto_s;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.EfModels
{
    public class RecipeCategory : IEntityModel
    {
        public RecipeCategory()
        {
            Recipes = new HashSet<Recipe>();
        }
        public int Id { get; set; }
        public MealTypeEnum MealType { get; set; }

        public DishTypeEnum DishType { get; set; }

        public DietTypeEnum DietType { get; set; }

        public CountryTypeEnum CountryType { get; set; }

        public  LifestyleTypeEnum LifestyleType { get; set; }
        public ICollection<Recipe> Recipes { get; set; }

        public static explicit operator RecipeCategory(RecipeCategoryDto dto)
        {
            return new RecipeCategory
            {
                MealType = dto.MealType,
                DishType = dto.DishType,
                DietType = dto.DietType,
                CountryType = dto.CountryType,
                LifestyleType = dto.LifestyleType,
            };
        }


    }
}

