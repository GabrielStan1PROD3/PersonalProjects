using BlackJack.App.ExtensionMethods;
using BlackJack.App.Models;
using BlackJack.App.Services.Interfaces;
using BlackJack.App.ViewModel;
using System.Numerics;

namespace BlackJack.App.Services;

public class BlackJackRuleService : IRuleService
{
    public int CalculateScore<TCard>(IList<TCard> card) where TCard : class

        => card switch
        {
            IList<CardViewModel> viewModel => viewModel.CalculateSum((x) => x == 1, (aceSum, sum) => aceSum > 0 && sum + 10 <= 21),
            IList<Card> c => c.CalculateSum((x) => x == 1, (aceSum, sum) => aceSum > 0 && sum + 10 <= 21),
            _ => 0
        };



    public TNumber CalculatePlayerBet<TNumber, TEnum>(string bet, TNumber multiplyer, TEnum isWinner) 
                                    where TNumber : INumber<TNumber>
                                    where TEnum : Enum

    {
        if (!TNumber.TryParse(bet, null, out TNumber? betAsNumber))
            return TNumber.Zero;

        return (Convert.ToInt32(isWinner)) switch
        {
            0 => betAsNumber * multiplyer,
            2 => betAsNumber,
            _ => TNumber.Zero
        };

    }
    public WinnerOfTable WinnerCalculationLogic(int playerScore, int botScore)
    {
        WinnerOfTable winner;
        if (playerScore <= 21 && botScore <= 21)
        {
            winner = playerScore > botScore ? WinnerOfTable.Player :
                                   playerScore == botScore ? WinnerOfTable.Push : WinnerOfTable.Bot;


        }
        else if (playerScore <= 21 && botScore > 21)
        {
            winner = WinnerOfTable.Player;
        }
        else
        {
            winner = WinnerOfTable.Bot;
        }
        return winner;
    }

}
