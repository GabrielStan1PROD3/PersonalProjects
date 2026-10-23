using BlackJack.App.Models;

namespace BlackJack.App.Services.Interfaces;

public interface IDeckService
{
   
    void DealCardsOnStart<TCard>(IList<TCard>? model, params TCard[] items);

    IEnumerable<Card> ShuffleDeck(IEnumerable<Card> cards);


}
