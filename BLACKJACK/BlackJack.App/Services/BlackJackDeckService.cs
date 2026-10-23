using BlackJack.App.ExtensionMethods;
using BlackJack.App.Models;
using BlackJack.App.Services.Interfaces;

namespace BlackJack.App.Services;

public class BlackJackDeckService :IDeckService
{
    public IEnumerable<Card> ShuffleDeck(IEnumerable<Card> cards)
    {
        return cards.ShuffleBy<Card, Guid>(() => Guid.NewGuid())
         .ShuffleData<Card>();
    }
    public void DealCardsOnStart<TCard>(IList<TCard>? model, params TCard[] items)
    {
        if (model is null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        foreach (TCard item in items)
        {
            model.Add(item);
        }
    }
}
