using Domain.Enums;
using Domain.OwnedTypes.Nutrition;
using Domain.TimesAndSteps;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Extensions
{
    public static class CalculationExtensions
    {
        public static TimeSpan CalculateTotalTime(this RecipeTimeDto time) => time.CookTime + time.CoolTime + time.PrepTime;

        public static double CalculateCalories(this Nutrients model) =>
            (model.Protein * 4) + (model.Carbohydrates * 4) + (model.Fats * 4);


        public static NutriscoreEnum SetNutriscore(this NutrientsDto model)
            => CalculateNutriscore(model) switch
            {
                <= -1 => NutriscoreEnum.A,
                <= 2 => NutriscoreEnum.B,
                <= 10 => NutriscoreEnum.C,
                <= 18 => NutriscoreEnum.D,
                _ => NutriscoreEnum.E
            };

        public static double ToGrams(UnitEnum unit, double quantity)
        {

            double value = unit switch
            {
                // Weight
                UnitEnum.Milligram => 0.001,
                UnitEnum.Gram => 1.0,
                UnitEnum.Kilogram => 1000.0,

                // Volume (approximate 1 ml ~ 1 g)
                UnitEnum.Milliliter => 1.0,
                UnitEnum.Liter => 1000.0,

                // Spoon-based
                UnitEnum.Teaspoon => 5.0,
                UnitEnum.Tablespoon => 15.0,

                // Cups
                UnitEnum.Cup => 240.0,

                // Units 
                UnitEnum.Piece => 50.0,
                UnitEnum.Slice => 28.0,
                UnitEnum.Clove => 3.0,


                UnitEnum.Pinch => 0.36,
                UnitEnum.Dash => 0.6,

                _ => throw new ArgumentOutOfRangeException(nameof(unit), $"Unknown unit: {unit}")
            };

            return value * quantity;
        }

        public static NutrientsDto CalculatePerGram(this NutrientsDto model, double gram)
        {
            return new NutrientsDto
            {
                Protein = model.Protein / 100 * gram,
                Carbohydrates = model.Carbohydrates / 100 * gram,
                Fats = model.Fats / 100 * gram,
                SaturatedFats = model.SaturatedFats / 100 * gram,
                Sugars = model.Sugars / 100 * gram,
                FruitPercentage = model.FruitPercentage / 100 * gram,
                Sodium = model.Sodium / 100 * gram,
                Fibres = model.Fibres / 100 * gram,
                EnergyKcal = model.EnergyKcal / 100 * gram,
                EnergyKj = model.EnergyKj / 100 * gram,
            };
        }

        // Points calculator
        private static int CalculateNutriscore(NutrientsDto nutrients)
        {
            int negativePoints = EnergyPoints(nutrients.EnergyKcal) + SugarPoints(nutrients.Sugars) +
                                 SodiumPoints(nutrients.Sodium) +
                                 SaturatedFatPoints(nutrients.SaturatedFats);

            int positivePoints = FiberPoints(nutrients.Fibres) + ProteinPoints(nutrients.Protein) +
                                 FruitPercentage(nutrients.FruitPercentage);

            return negativePoints - positivePoints;
        }



        // good points
        private static int FruitPercentage(double fruit)
            => fruit switch
            {
                < 40 => 1,
                < 60 => 2,
                < 80 => 3,
                _ => 5,
            };
        private static int ProteinPoints(double protein)
            => protein switch
            {
                <= 1.6 => 0,
                <= 3.2 => 1,
                <= 4.8 => 2,
                <= 6.4 => 3,
                <= 8.0 => 4,
                _ => 5,
            };
        private static int FiberPoints(double fiber)
            => fiber switch
            {
                <= 0.9 => 0,
                <= 1.9 => 1,
                <= 2.8 => 2,
                <= 3.7 => 3,
                <= 4.7 => 4,
                _ => 5,
            };
        // #

        // bad points
        private static int SodiumPoints(double sodium)

            => sodium switch
            {
                <= 90 => 0,
                <= 180 => 1,
                <= 270 => 2,
                <= 360 => 3,
                <= 450 => 4,
                <= 540 => 5,
                <= 630 => 6,
                <= 720 => 7,
                <= 810 => 8,
                <= 900 => 9,
                _ => 10
            };

        private static int EnergyPoints(double kcal)
            => kcal switch
            {
                <= 80 => 0,
                <= 160 => 1,
                <= 240 => 2,
                <= 320 => 3,
                <= 400 => 4,
                <= 480 => 5,
                <= 560 => 6,
                <= 640 => 7,
                <= 720 => 8,
                <= 800 => 9,
                _ => 10
            };
        private static int SugarPoints(double sugar)

            => sugar switch
            {
                <= 4.5 => 0,
                <= 9 => 1,
                <= 13.5 => 2,
                <= 18 => 3,
                <= 22.5 => 4,
                <= 27 => 5,
                <= 31 => 6,
                <= 36 => 7,
                <= 40 => 8,
                <= 45 => 9,
                _ => 10
            };

        private static int SaturatedFatPoints(double saturated)
            => saturated switch
            {
                <= 1 => 0,
                <= 2 => 1,
                <= 3 => 2,
                <= 4 => 3,
                <= 5 => 4,
                <= 6 => 5,
                <= 7 => 6,
                <= 8 => 7,
                <= 9 => 8,
                <= 10 => 9,
                _ => 10
            };

        // #
    }

}
