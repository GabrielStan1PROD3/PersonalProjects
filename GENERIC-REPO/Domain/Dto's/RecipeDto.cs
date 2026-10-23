using Domain.EfModels;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dto_s
{
    public class RecipeDto :IDto
    {
        public string Name { get; set; }


        public ICollection<string> ImagesPath { get; set; } = new List<string>();

        public DificultyEnum Dificulty { get; set; }
        public string Description { get; set; }

        public int RecipeCategoryId { get; set; }

   
        public static explicit operator RecipeDto(Recipe recipe)
        {
            return new RecipeDto
            {
                Name = recipe.Name,
                ImagesPath = recipe.ImagesPath,
                Dificulty = recipe.Dificulty,
                RecipeCategoryId = recipe.RecipeCategoryId,
                Description = recipe.Description
            };
        }
    }
}
