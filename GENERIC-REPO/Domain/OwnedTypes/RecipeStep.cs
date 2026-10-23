using Domain.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.TimesAndSteps
{
    public class RecipeStep
    {
        [KeyIdentifier] public int StepNumber { get; set; }
       
        public string Instruction { get; set; }
        public string? ImagePath { get; set; } // opțional

        public static explicit operator RecipeStep(RecipeStepDto dto)
        {
            return new RecipeStep
            {
                StepNumber = dto.StepNumber,
                Instruction = dto.Instruction,
                ImagePath = dto.ImagePath
            };
        }
    }
}
