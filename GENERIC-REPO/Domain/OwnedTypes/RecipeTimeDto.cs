using Domain.EfModels;
using Domain.TimesAndSteps;

public class RecipeTimeDto : IDto
{
    
    public int RecipeId { get; set; }
    public TimeSpan PrepTime { get; set; }
    public TimeSpan CookTime { get; set; }
    public TimeSpan CoolTime { get; set; }

    public TimeSpan? ServeTime { get; set; }

    public TimeSpan TotalTime => CalculateTotalTime();


    public TimeSpan CalculateTotalTime()
    {
        TimeSpan totalTime = PrepTime + CookTime + CoolTime;

        return totalTime;
    }


    public static explicit operator RecipeTimeDto(Recipe recipe) {
        return new RecipeTimeDto
        {
            RecipeId = recipe.Id,
            PrepTime = recipe.Times.PrepTime,
            CookTime = recipe.Times.CookTime,
            CoolTime = recipe.Times.CoolTime,
            ServeTime = recipe.Times.ServeTime
        };
    }



    


}