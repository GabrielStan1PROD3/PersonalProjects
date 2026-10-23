using BlackJack.App.Data;
using BlackJack.App.ExtensionMethods;
using BlackJack.App.Models;
using BlackJack.App.Services.Interfaces;
using BlackJack.App.ViewModel;

namespace BlackJack.App.Services;

public class BlackJackGameService 
{
    private readonly IDataContext? _context; 
    private readonly IDeckService? _deckService;
    private readonly IRuleService? _ruleService;

    public EventHandler<CardsEventArgs>? CardsReceived { get; set; }

    public BlackJackGameService(IDataContext context, IDeckService deckService,IRuleService ruleService)
    {
        _context = context;
        _deckService = deckService;
        _ruleService = ruleService;
    }


    public async Task LoadAndShuffleCardsAsync() 
    {
        var dataList = await _context!.GetAllAsync();

        InvokeCards(_deckService!.ShuffleDeck(dataList));

    }

  
    public void DealCards(IList<Card>deck,IList<CardViewModel> target) 
    {
        var card = new CardViewModel(deck.GetOneCard());

        var card2 = new CardViewModel(deck.GetOneCard());
        card2.IsCardVisible =  false;

        _deckService?.DealCardsOnStart(target!, card, card2);
    }

    public  int GetScore(IList<CardViewModel> target) 
    {
        return _ruleService!.CalculateScore(target);
    }

    public int GetWinBet(string bet , WinnerOfTable winner) 
    {
      return  _ruleService!.CalculatePlayerBet(bet ,2, winner);
    }

    private void InvokeCards(IEnumerable<Card> cards) 
    {
        CardsReceived?.Invoke(this,new CardsEventArgs(cards));
    }

    public WinnerOfTable GetWinnerOfTable(int playerScore, int botScore)
    {
        return _ruleService.WinnerCalculationLogic(playerScore, botScore);

    }

    public void BotTurn(IList<Card> deck, IList<CardViewModel> botCards)
    {
        while (_ruleService!.CalculateScore(botCards) < 17)
        {
            botCards.Add(new CardViewModel(deck.GetOneCard()));
        }
    }

}
