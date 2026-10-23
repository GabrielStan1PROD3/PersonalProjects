using Domain.EfModels;
using Domain.Enums;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dto_s
{
    public class RecipeIngredientDto :IDto
    {
        public int RecipeIngredientId { get; set; }
        public double QuantityInUnit { get; set; }
        public UnitEnum Unit { get; set; }
        public int RecipeId { get; set; }
        
        public int IngredientId { get; set; }


        public double QuantityInGrams => CalculationExtensions.ToGrams(this.Unit, this.QuantityInUnit);



        public static explicit operator RecipeIngredientDto(RecipeIngredient entity)
        {
            return new RecipeIngredientDto
            {
                RecipeIngredientId = entity.Id,
                QuantityInUnit = entity.QuantityInUnit,
                Unit = entity.Unit,
                RecipeId = entity.RecipeId,
                IngredientId = entity.IngredientId
            };
        }
    }
}
