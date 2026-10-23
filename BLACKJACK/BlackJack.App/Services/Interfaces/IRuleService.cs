using System.Numerics;

namespace BlackJack.App.Services.Interfaces;

public interface IRuleService
    {
        int CalculateScore<TCard>(IList<TCard> card) where TCard : class;

        TNumber CalculatePlayerBet<TNumber, TEnum>(string bet, TNumber multiplyer, TEnum isWinner)
                                            where TNumber : INumber<TNumber> where TEnum : Enum;


    WinnerOfTable WinnerCalculationLogic(int playerScore, int botScore);
    }
