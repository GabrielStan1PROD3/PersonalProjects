using Domain.EfModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.TimesAndSteps
{
    public class RecipeTime
    {
        public TimeSpan PrepTime { get; set; }
        public TimeSpan CookTime { get; set; }
        public TimeSpan CoolTime { get; set; }
        public TimeSpan? ServeTime { get; set; }


        public static explicit operator RecipeTime(RecipeTimeDto dto)
        {
            return new RecipeTime
            {
                PrepTime = dto.PrepTime,
                CookTime = dto.CookTime,
                CoolTime = dto.CoolTime,
                ServeTime = dto.ServeTime
            };
        }
        }

}
