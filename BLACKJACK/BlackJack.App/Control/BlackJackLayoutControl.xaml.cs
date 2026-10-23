using BlackJack.App.Data;
using BlackJack.App.Services;
using BlackJack.App.ViewModel;
using System.Windows.Controls;

namespace BlackJack.App.Control
{
    /// <summary>
    /// Interaction logic for BlackJackLayoutControl.xaml
    /// </summary>
    public partial class BlackJackLayoutControl : UserControl
    {
        private GameViewModel _gameModel;

        public BlackJackLayoutControl()
        {



            InitializeComponent();
            BlackJackGameService service = new BlackJackGameService(BlackJackDataContext.Instance, new BlackJackDeckService(),new BlackJackRuleService());
            _gameModel = new GameViewModel(new BlackJackValidationService(), service );
            DataContext = _gameModel;
            DeckView.DataContext = _gameModel;
        }


    }
}
