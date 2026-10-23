using Domain.Dto_s;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Domain.EfModels
{
    public class RecipeIngredient : IEntityModel
    {
        public int Id { get; set; }

        public double QuantityInUnit { get; set; }
        public UnitEnum Unit { get; set; }


        // nav & relationships
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }


        public static explicit operator RecipeIngredient(RecipeIngredientDto dto)
        {
            return new RecipeIngredient
            {
                QuantityInUnit = dto.QuantityInUnit,
                Unit = dto.Unit,
                RecipeId = dto.RecipeId,
                IngredientId = dto.IngredientId
            };
        }
      
    }
}