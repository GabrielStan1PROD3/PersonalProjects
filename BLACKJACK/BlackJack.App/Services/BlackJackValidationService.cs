using BlackJack.App.Services.Interfaces;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace BlackJack.App.Services;

public class BlackJackValidationService : IValidationService
{
    public bool ValidateInput(string input)
    {
        return Regex.IsMatch(input, @"^\d+$");
    }

 


 

}

