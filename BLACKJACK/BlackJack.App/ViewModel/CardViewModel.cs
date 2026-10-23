using BlackJack.App.Models;

namespace BlackJack.App.ViewModel;

public class CardViewModel : ViewModelBase
{
    private readonly Card _card;
    private bool _isCardVisible = true;


    public CardViewModel(Card card)
    {
        _card = card;
    }
    public string? ImageUrl => _card.ImageUrl;
    public int[]? Value => _card.Value;

    public string? CardImage => IsCardVisible ? ImageUrl : "/images/cards/back.png";


    public bool IsCardVisible
    {
        get
        {
            return _isCardVisible;
        }
        set
        {
            _isCardVisible = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CardImage));
        }
    }

}
