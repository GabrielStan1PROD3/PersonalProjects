using BlackJack.App.Command;
using BlackJack.App.ExtensionMethods;
using BlackJack.App.Models;
using BlackJack.App.Services;
using BlackJack.App.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace BlackJack.App.ViewModel;

public class GameViewModel : ViewModelBase
{
    private readonly IValidationService _validationService;
    private readonly BlackJackGameService _cardGameService;

    private IList<Card>? _cards = new List<Card>();
    private int _credits;
    private string _bet = "";

    private bool _isDeckVisible = false;
    private bool _isNewGameButtonPressed = true;
    private bool _canHit = true;
    private bool _canBet = true;

    private bool _playerWin = false;
    private bool _isDraw = false;
    private bool _botWin = false;

    private WinnerOfTable _winner;

    public GameViewModel(IValidationService validationService, BlackJackGameService cardService)
    {
        _validationService = validationService;
        _cardGameService = cardService;

        PlayerCards = new ObservableCollection<CardViewModel>();
        BotCards = new ObservableCollection<CardViewModel>();


        _cardGameService.CardsReceived += OnCardsReceived;

        PlayBetCommand = new BlackjackRelayCommand(PlaceBet, CanPlaceBet);
        HitCommand = new BlackjackRelayCommand(GetCard, CanGetCard);
        StopCommand = new BlackjackRelayCommand(OnStopButton, CanPressStop);
        NewGameCommand = new BlackjackRelayCommand(OnNewGameButton, null);
        NextButtonCommand = new BlackjackRelayCommand(OnNextButtonPressed, CanPressNext);
        StopButtonCommand = new BlackjackRelayCommand(OnStopButtonPressed, null);
    }

    private void OnCardsReceived(object? sender, CardsEventArgs e)
    {
        Cards = new ObservableCollection<Card>(e.Cards);
        OnPropertyChanged(nameof(NumberOfCards));
    }
    
    public IList<Card>? Cards
    {
        get => _cards;
        set
        {
            _cards = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(NumberOfCards));
        }
    }

    public ObservableCollection<CardViewModel> PlayerCards { get; set; }
    public ObservableCollection<CardViewModel> BotCards { get; set; }

    public int NumberOfCards => Cards?.Count ?? 0;
    
    public int PlayerScore { get; set; }     
    public int BotScore { get; set; }
    
    public int Credits
    {
        get => _credits;
        set { _credits = value; OnPropertyChanged(); }
    }

    public string Bet
    {
        get => _bet;
        set
        {
            _bet = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ValidationInput));
        }
    }

    public bool ValidationInput =>
        _validationService.ValidateInput(Bet);

    public bool IsDeckVisible
    {
        get => _isDeckVisible;
        set { _isDeckVisible = value; OnPropertyChanged(); }
    }

    public bool IsNewGameButtonPressed
    {
        get => _isNewGameButtonPressed;
        set { _isNewGameButtonPressed = value; OnPropertyChanged(); }
    }

    public bool CanHit
    {
        get => _canHit;
        set { _canHit = value; OnPropertyChanged(); }
    }

    public bool CanBet
    {
        get => _canBet;
        set { _canBet = value; OnPropertyChanged(); }
    }

    public WinnerOfTable Winner
    {
        get => _winner;
        set { _winner = value; OnPropertyChanged(); }
    }

    public bool PlayerWin
    {
        get => _playerWin;
        set { _playerWin = value; OnPropertyChanged(); }
    }

    public bool BotWin
    {
        get => _botWin;
        set { _botWin = value; OnPropertyChanged(); }
    }

    public bool IsDraw
    {
        get => _isDraw;
        set { _isDraw = value; OnPropertyChanged(); }
    }


    public ICommand NewGameCommand { get; }
    public ICommand PlayBetCommand { get; }
    public ICommand HitCommand { get; }
    public ICommand StopCommand { get; }
    public ICommand NextButtonCommand { get; }
    public ICommand StopButtonCommand { get; }
    private async void ResetGame()
    {
        Cards!.Clear();
        PlayerCards.Clear();
        BotCards.Clear();
        BotScore = 0;

        await _cardGameService.LoadAndShuffleCardsAsync();

        CanBet = true;
        CanHit = true;
        PlayerWin = false;
        BotWin = false;
        IsDraw = false;

        Notify(nameof(CanBet)
            ,nameof(PlayerScore)
            ,nameof(BotScore)
            ,nameof(PlayerWin)
            ,nameof(BotWin)
            ,nameof(IsDraw));
    }
    private async void OnNewGameButton(object? parameters)
    {
        await _cardGameService.LoadAndShuffleCardsAsync(); 

        StartGameCards(); 

        Credits = 500;
        IsDeckVisible = true;
        IsNewGameButtonPressed = false;

        Notify(nameof(IsDeckVisible)
            ,nameof(IsNewGameButtonPressed));

    }

    private void StartGameCards()
    {
        _cardGameService.DealCards(Cards!, PlayerCards);
        _cardGameService.DealCards(Cards!, BotCards);
        PlayerScore = PlayerCards[0].Value!.Length >1 ? 
            PlayerCards[0].Value![1] : PlayerCards[0].Value![0];
        BotScore = BotCards[0].Value!.Length > 1 ? 
            BotCards[0].Value![1] : BotCards[0].Value![0];


        Notify(nameof(NumberOfCards)
            , nameof(PlayerScore)
            , (nameof(BotScore)));
    }

    private void GetCard(object? parameters)
    {
        PlayerCards.Add(new CardViewModel(Cards!.GetOneCard()));
           PlayerScore = _cardGameService.GetScore(PlayerCards);
        if (PlayerScore >= 21)
        {
            OnStopButton(null);
        }

        Notify(nameof(NumberOfCards)
            , nameof(PlayerScore));
      
    }

    private bool CanGetCard(object? parameters)
        => !CanBet && PlayerScore < 21 && CanHit;

    private void OnStopButton(object? parameter)
    {
        CanHit = false;
        BotCards[1].IsCardVisible = true;
        _cardGameService.BotTurn(Cards!, BotCards);
        BotScore = _cardGameService.GetScore(BotCards);

        Winner = _cardGameService.GetWinnerOfTable(PlayerScore, BotScore);

        PlayerWin = Winner == WinnerOfTable.Player;
        BotWin = Winner == WinnerOfTable.Bot;
        IsDraw = Winner == WinnerOfTable.Push;

        Credits += _cardGameService.GetWinBet(Bet, Winner);

        Bet = "";
        Notify(nameof(Credits)
            ,nameof(PlayerWin)
            ,nameof(BotWin)
            ,nameof(IsDraw)
            ,nameof(NumberOfCards)
            ,nameof(BotScore));

    }
    private bool CanPressStop(object? parameter) 
    {
        return !CanBet;
    }
    private void PlaceBet(object? param)
    {
        CanBet = false;
        PlayerCards[1].IsCardVisible = true;
         PlayerScore = _cardGameService.GetScore(PlayerCards);
        Credits -= int.Parse(Bet);

        OnPropertyChanged(nameof(PlayerScore));
    }

    private bool CanPlaceBet(object? param)
    {
        if (!int.TryParse(Bet, out int bet))
            return false;

        return bet > 0 && Credits >= bet && CanBet;
    }

    private  void OnNextButtonPressed(object? param)
    {
        ResetGame();
        StartGameCards();
    }
    private  bool CanPressNext(object? param) 
    {
        return Credits > 0;
    }

    private void OnStopButtonPressed(object? param) 
    {
        IsDeckVisible = false;
        IsNewGameButtonPressed = true;
        Credits = 0;
        ResetGame();
       

        Notify(nameof(IsDeckVisible),
        nameof(IsNewGameButtonPressed));
 
    }
}
