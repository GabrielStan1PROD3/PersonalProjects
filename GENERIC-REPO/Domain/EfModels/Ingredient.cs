using Domain.Dto_s;
using Domain.Interfaces;
using Domain.OwnedTypes.Nutrition;

namespace Domain.EfModels
{
    public class Ingredient : IEntityModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string? Description { get; set; }
        public Nutrients? Nutrients { get; set; } = new Nutrients();


        // relation & nav

        public int IngredientCategoryId { get; set; }
        public IngredientCategory Category { get; set; }

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new HashSet<RecipeIngredient>();



        // mapping
        public static explicit operator Ingredient(IngredientDto dto)
        {
            return new Ingredient
            {

                Name = dto.Name,
                Description = dto.Description,
                ImagePath = dto.ImagePath,
                IngredientCategoryId = dto.IngredientCategoryId,

            };
        }

    }

}
