using Domain.EfModels;
using Domain.OwnedTypes.Nutrition;

namespace Domain.Dto_s
{
    public class IngredientDto : IDto
    {
        
        public string Name { get; set; }
        public string? Description { get; set; }
        public string ImagePath { get; set; }
        public int IngredientCategoryId { get; set; }


 

        public static explicit operator IngredientDto(Ingredient entity)
        {
            return new IngredientDto
            {

                Name = entity.Name,
                Description = entity.Description,
                ImagePath = entity.ImagePath,
                IngredientCategoryId = entity.IngredientCategoryId

            };
        }


        }
    }



