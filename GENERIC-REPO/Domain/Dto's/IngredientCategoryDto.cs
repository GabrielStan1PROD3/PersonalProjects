using Domain.EfModels;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dto_s
{
    public class IngredientCategoryDto : IDto
    {
        public int Id { get; set; }
        public   IngredientCategoryEnum Name { get; set; }


        public static explicit operator IngredientCategoryDto(IngredientCategory entity)
        {
            return new IngredientCategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}
