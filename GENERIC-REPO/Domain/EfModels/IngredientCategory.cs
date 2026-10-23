using Domain.Dto_s;
using Domain.Enums;
using Domain.Interfaces;
using System.Runtime.CompilerServices;

namespace Domain.EfModels
{
    public class IngredientCategory : IEntityModel
    {
        public IngredientCategory()
        {
            Ingredients = new HashSet<Ingredient>();
        }

        public int Id { get; set; }
        public IngredientCategoryEnum Name { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }



        public static explicit operator IngredientCategory(IngredientCategoryDto dto) 
        {
            return new IngredientCategory
            {
                Name = dto.Name
            };
        }

    }
}
