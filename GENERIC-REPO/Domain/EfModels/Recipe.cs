using Domain.Dto_s;
using Domain.Enums;
using Domain.Interfaces;
using Domain.TimesAndSteps;

namespace Domain.EfModels
{
    public class Recipe : IEntityModel
    {
        public Recipe()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }


        public ICollection<string> ImagesPath { get; set; } = new List<string>();

        public DificultyEnum Dificulty { get; set; }
        public string Description { get; set; }

        public RecipeTime Times { get; set; } = new RecipeTime();
        public ICollection<RecipeStep> Steps { get; set; } = new List<RecipeStep>();



        // nav & relationships

        public int RecipeCategoryId { get; set; }
        public RecipeCategory RecipeCategory { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();



        public static explicit operator Recipe(RecipeDto dto)
        {
            return new Recipe
            {
                Name = dto.Name,
                ImagesPath = dto.ImagesPath,
                Dificulty = dto.Dificulty,
                RecipeCategoryId = dto.RecipeCategoryId,
                Description = dto.Description
            };
        }


    } 
}
