namespace Domain.OwnedTypes.Nutrition
{
    public class Nutrients
    {
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


        public static explicit operator Nutrients (NutrientsDto e) 
        {
            return new Nutrients
            {
                Carbohydrates = e.Carbohydrates,
                EnergyKcal = e.EnergyKcal,
                EnergyKj = e.EnergyKj,
                Fats = e.Fats,
                Fibres = e.Fibres,
                FruitPercentage = e.FruitPercentage,
                Protein = e.Protein,
                SaturatedFats = e.SaturatedFats,
                Sodium = e.Sodium,
                Sugars = e.Sugars
            };
        }
    }
}