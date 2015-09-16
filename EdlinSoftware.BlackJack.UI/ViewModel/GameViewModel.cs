using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using EdlinSoftware.Cards;

namespace EdlinSoftware.BlackJack.UI.ViewModel
{
    internal class GameViewModel : BaseViewModel
    {
        private Game _game;
        private readonly ObservableCollection<Card> _playerCards = new ObservableCollection<Card>();
        private readonly ObservableCollection<Card> _dealerCards = new ObservableCollection<Card>();
        private ICardImagesProvider _cardImagesProvider;

        public RoundResults RoundResult => _game.RoundResult;

        public RoundStates RoundState => _game.RoundState;

        public GameStates GameState => _game.GameState;

        public int PlayerMoney => _game.PlayerMoney;

        public int DealerMoney => _game.DealerMoney;

        public int CurrentBet => _game.CurrentBet;

        public ObservableCollection<Card> PlayersCards => _playerCards;

        public ObservableCollection<Card> DealersCards => _dealerCards;

        public ICardImagesProvider CardImagesProvider
        {
            [DebuggerStepThrough]
            get { return _cardImagesProvider; }
            set
            {
                if (_cardImagesProvider != value)
                {
                    _cardImagesProvider = value;
                    OnPropertyChanged();
                }
            }
        }

        public GameViewModel()
        {
            var options = new OptionsViewModel();

            StartNewGame(options);
        }

        private void RefreshCards()
        {
            _playerCards.Clear();
            _dealerCards.Clear();

            foreach (var card in _game.PlayersCards ?? new Card[0])
            {
                _playerCards.Add(card);
            }
            foreach (var card in _game.DealersCards ?? new Card[0])
            {
                _dealerCards.Add(card);
            }
        }

        private void OnGamePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_game.PlayersCards):
                case nameof(_game.DealersCards):
                    RefreshCards();
                    break;
            }

            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(e.PropertyName);
        }

        public ICommand NewGameCommand
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    StartNewGame(new OptionsViewModel());
                });
            }
        }

        private void StartNewGame(OptionsViewModel options)
        {
            if (_game != null)
            {
                _game.PropertyChanged -= OnGamePropertyChanged;
            }

            CardImagesProvider = new FileBasedCardImagesProvider(options.DeckFile,
                new FileBasedBackImageProvider(options.BackFile));

            _game = new Game(new EndlessCardsProvider(new FullDeckCreator()), new LeveledDealerStrategy(), options.InitialPlayerMoney, options.InitialDealerMoney);
            _game.PropertyChanged += OnGamePropertyChanged;

            RefreshCards();

            OnPropertyChanged(nameof(DealerMoney));
            OnPropertyChanged(nameof(PlayerMoney));
            OnPropertyChanged(nameof(CurrentBet));
            OnPropertyChanged(nameof(DealersCards));
            OnPropertyChanged(nameof(PlayersCards));
            OnPropertyChanged(nameof(RoundResult));
            OnPropertyChanged(nameof(RoundState));
            OnPropertyChanged(nameof(GameState));
        }

        public ICommand HitCommand
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    _game.Hit();
                });
            }
        }

        public ICommand StandCommand
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    _game.Stand();
                });
            }
        }

        public ICommand StartRoundCommand
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    int bet;

                    if (!int.TryParse((string) arg, out bet))
                    {
                        bet = _game.PlayerMoney;
                    }

                    _game.StartRound(bet);
                },
                arg =>
                {
                    int bet;

                    if (!int.TryParse((string)arg, out bet))
                    {
                        bet = _game.PlayerMoney;
                    }

                    return bet <= _game.PlayerMoney;
                });
            }
        }

        public ICommand OptionsCommand
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    var options = new OptionsViewModel();

                    var window = new View.OptionsWindow
                    {
                        Owner = Application.Current.MainWindow,
                        DataContext = options
                    };

                    if (window.ShowDialog() == true)
                    {
                        CardImagesProvider = new FileBasedCardImagesProvider(options.DeckFile,
                            new FileBasedBackImageProvider(options.BackFile));

                        if (options.GameRestartRequired)
                        {
                            StartNewGame(options);
                        }
                    }
                });
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    Application.Current.Shutdown();
                });
            }
        }
    }
}
