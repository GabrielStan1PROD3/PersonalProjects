using BlackJack.App.Models;

namespace BlackJack.App.Services;

public class CardsEventArgs : EventArgs 
{
    public IEnumerable<Card> Cards { get; set; }
    public CardsEventArgs(IEnumerable<Card> cards ) 
    {
        Cards = cards;
    } 
}
