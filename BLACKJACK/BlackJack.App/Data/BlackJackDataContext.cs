using BlackJack.App.Models;

namespace BlackJack.App.Data;

public class BlackJackDataContext : IDataContext
{
    private Card[] _deck;

    private BlackJackDataContext()
    {
        _deck = new Card[]
  {
    // CLUBS
    new Card { ImageUrl="/images/cards/clubs_A.png", Value=new[] {1,11}},
    new Card { ImageUrl="/images/cards/clubs_2.png", Value=new[] {2}},
    new Card { ImageUrl="/images/cards/clubs_3.png", Value=new[] {3}},
    new Card { ImageUrl="/images/cards/clubs_4.png", Value=new[] {4}},
    new Card { ImageUrl="/images/cards/clubs_5.png", Value=new[] {5}},
    new Card { ImageUrl="/images/cards/clubs_6.png", Value=new[] {6}},
    new Card { ImageUrl="/images/cards/clubs_7.png", Value=new[] {7}},
    new Card { ImageUrl="/images/cards/clubs_8.png", Value=new[] {8}},
    new Card { ImageUrl="/images/cards/clubs_9.png", Value=new[] {9}},
    new Card { ImageUrl="/images/cards/clubs_10.png", Value=new[] {10}},
    new Card { ImageUrl="/images/cards/clubs_J.png", Value=new[] {10}},
    new Card { ImageUrl="/images/cards/clubs_Q.png", Value=new[] {10}},
    new Card { ImageUrl="/images/cards/clubs_K.png", Value=new[] {10}},

    // DIAMONDS
    new Card { ImageUrl="/images/cards/diamonds_A.png", Value=new[] {1,11}},
    new Card { ImageUrl="/images/cards/diamonds_2.png", Value=new[] {2}},
    new Card { ImageUrl="/images/cards/diamonds_3.png", Value=new[] {3}},
    new Card { ImageUrl="/images/cards/diamonds_4.png", Value=new[] {4}},
    new Card { ImageUrl="/images/cards/diamonds_5.png", Value=new[] {5}},
    new Card { ImageUrl="/images/cards/diamonds_6.png", Value=new[] {6}},
    new Card { ImageUrl="/images/cards/diamonds_7.png", Value=new[] {7}},
    new Card { ImageUrl="/images/cards/diamonds_8.png", Value=new[] {8}},
    new Card { ImageUrl="/images/cards/diamonds_9.png", Value=new[] {9}},
    new Card { ImageUrl="/images/cards/diamonds_10.png", Value=new[] {10}},
    new Card { ImageUrl="/images/cards/diamonds_J.png", Value=new[] {10}},
    new Card { ImageUrl="/images/cards/diamonds_Q.png", Value=new[] {10}},
    new Card { ImageUrl="/images/cards/diamonds_K.png", Value=new[] {10}},

    // HEARTS
    new Card { ImageUrl="/images/cards/hearts_A.png", Value=new[] {1,11}},
    new Card { ImageUrl="/images/cards/hearts_2.png", Value=new[] {2}},
    new Card { ImageUrl="/images/cards/hearts_3.png", Value=new[] {3}},
    new Card { ImageUrl="/images/cards/hearts_4.png", Value=new[] {4}},
    new Card { ImageUrl="/images/cards/hearts_5.png", Value=new[] {5}},
    new Card { ImageUrl="/images/cards/hearts_6.png", Value=new[] {6}},
    new Card { ImageUrl="/images/cards/hearts_7.png", Value=new[] {7}},
    new Card { ImageUrl="/images/cards/hearts_8.png", Value=new[] {8}},
    new Card { ImageUrl="/images/cards/hearts_9.png", Value=new[] {9}},
    new Card { ImageUrl="/images/cards/hearts_10.png", Value=new[] {10}},
    new Card { ImageUrl="/images/cards/hearts_J.png", Value=new[] {10}},
    new Card { ImageUrl="/images/cards/hearts_Q.png", Value=new[] {10}},
    new Card { ImageUrl="/images/cards/hearts_K.png", Value=new[] {10}},

    // SPADES
    new Card { ImageUrl="/images/cards/spades_A.png", Value=new[] {1,11}},
    new Card { ImageUrl="/images/cards/spades_2.png", Value=new[] {2}},
    new Card { ImageUrl="/images/cards/spades_3.png", Value=new[] {3}},
    new Card { ImageUrl="/images/cards/spades_4.png", Value=new[] {4}},
    new Card { ImageUrl="/images/cards/spades_5.png", Value=new[] {5}},
    new Card { ImageUrl="/images/cards/spades_6.png", Value=new[] {6}},
    new Card { ImageUrl="/images/cards/spades_7.png", Value=new[] {7}},
    new Card { ImageUrl="/images/cards/spades_8.png", Value=new[] {8}},
    new Card { ImageUrl="/images/cards/spades_9.png", Value=new[] {9}},
    new Card { ImageUrl="/images/cards/spades_10.png", Value=new[] {10}},
    new Card { ImageUrl="/images/cards/spades_J.png", Value=new[] {10}},
    new Card { ImageUrl="/images/cards/spades_Q.png", Value=new[] {10}},
    new Card { ImageUrl="/images/cards/spades_K.png", Value=new[] {10}},
  };
    }


    public static BlackJackDataContext Instance { get; } =new BlackJackDataContext();

    public async Task<IEnumerable<Card>> GetAllAsync()
    {
        return await Task.FromResult<IEnumerable<Card>>(_deck);
    }


}
