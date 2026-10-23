using Domain.Common.CustomAttributes;
using Domain.EfModels;

namespace Domain.TimesAndSteps
{
    public class RecipeStepDto :IDto
    {
        [KeyIdentifier] public int StepNumber { get; set; }

        public string Instruction { get; set; }
        public string? ImagePath { get; set; } // opțional



        public static explicit operator RecipeStepDto(RecipeStep r)
        {

            return new RecipeStepDto
            {

                StepNumber = r.StepNumber,
                Instruction = r.Instruction,
                ImagePath = r.ImagePath
            };
        }


      
    }
}
