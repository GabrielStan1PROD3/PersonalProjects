using Domain.EfModels;
using Domain.Enums;
using Domain.Extensions;

namespace Domain.OwnedTypes.Nutrition
{
    public class NutrientsDto : IDto
    {
        public int IngredientId;
        public double EnergyKj { get; set; }
        public double EnergyKcal { get; set; }
        public double Sugars { get; set; }
        public double SaturatedFats { get; set; }

        public double Sodium { get; set; }
        public double FruitPercentage { get; set; }
        public double Fibres { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Fats { get; set; }

        public NutriscoreEnum NutriScore => this.SetNutriscore();


        public static explicit operator NutrientsDto (Ingredient e)
        {
            return new NutrientsDto
            {
                IngredientId = e.Id,
                Carbohydrates = e.Nutrients.Carbohydrates,
                EnergyKcal = e.Nutrients.EnergyKcal,
                EnergyKj = e.Nutrients.EnergyKj,
                Fats = e.Nutrients.Fats,
                Fibres = e.Nutrients.Fibres,
                FruitPercentage = e.Nutrients.FruitPercentage,
                Protein = e.Nutrients.Protein,
                SaturatedFats = e.Nutrients.SaturatedFats,
                Sodium = e.Nutrients.Sodium,
                Sugars = e.Nutrients.Sugars
            };
        }


    }
}


