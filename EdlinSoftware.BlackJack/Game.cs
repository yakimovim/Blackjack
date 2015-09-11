using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using EdlinSoftware.BlackJack.Properties;
using EdlinSoftware.Cards;

namespace EdlinSoftware.BlackJack
{
    /// <summary>
    /// Represents game of blackjack.
    /// </summary>
    public class Game : INotifyPropertyChanged
    {
        private readonly ICardsProvider _cardsProvider;
        private readonly IDealerStrategy _dealerStrategy;

        private Round _round;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private GameStates _gameState = GameStates.GameIsInProgress;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _playerMoney;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _dealerMoney;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _roundBet;

        public GameStates GameState
        {
            [DebuggerStepThrough]
            get
            { return _gameState; }
            private set
            {
                if (value == _gameState) return;

                _gameState = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets result of current round.
        /// </summary>
        public RoundResults RoundResult => _round?.RoundResult ?? RoundResults.RoundNotStarted;

        /// <summary>
        /// Gets state of current round.
        /// </summary>
        public RoundStates RoundState => _round?.RoundState ?? RoundStates.RoundIsOver;

        /// <summary>
        /// Gets cards of player.
        /// </summary>
        public IEnumerable<Card> PlayersCards => _round?.PlayersCards;

        /// <summary>
        /// Gets cards of dealer.
        /// </summary>
        public IEnumerable<Card> DealersCards => _round?.DealersCards;

        /// <summary>
        /// Gets player's money.
        /// </summary>
        public int PlayerMoney
        {
            [DebuggerStepThrough]
            get
            { return _playerMoney; }
            private set
            {
                if (value != _playerMoney)
                {
                    _playerMoney = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets dealer's money.
        /// </summary>
        public int DealerMoney
        {
            [DebuggerStepThrough]
            get
            { return _dealerMoney; }
            private set
            {
                if (value != _dealerMoney)
                {
                    _dealerMoney = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets bet for current round.
        /// </summary>
        public int CurrentBet
        {
            [DebuggerStepThrough]
            get
            { return _roundBet; }
            private set
            {
                if (value != _roundBet)
                {
                    _roundBet = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// This constructor is for test only.
        /// </summary>
        [DebuggerStepThrough]
        public Game(ICardsProvider cardsProvider, IDealerStrategy dealerStrategy)
            : this(cardsProvider, dealerStrategy, 100, 100)
        {}

        [DebuggerStepThrough]
        public Game(ICardsProvider cardsProvider, IDealerStrategy dealerStrategy, 
            int playerMoney, int dealerMoney)
        {
            if (cardsProvider == null) throw new ArgumentNullException(nameof(cardsProvider));
            if (dealerStrategy == null) throw new ArgumentNullException(nameof(dealerStrategy));
            if (playerMoney <= 0)
                throw new ArgumentOutOfRangeException(nameof(playerMoney), Resources.AmountOfMoneyMustBePositive);
            if (dealerMoney <= 0)
                throw new ArgumentOutOfRangeException(nameof(dealerMoney), Resources.AmountOfMoneyMustBePositive);

            _cardsProvider = cardsProvider;
            _dealerStrategy = dealerStrategy;

            PlayerMoney = playerMoney;
            DealerMoney = dealerMoney;
        }

        /// <summary>
        /// Starts new round of game.
        /// </summary>
        public void StartRound(int bet)
        {
            if(bet <= 0)
                throw new ArgumentOutOfRangeException(nameof(bet), Resources.BetMustBePositive);

            if(GameState != GameStates.GameIsInProgress)
                return;

            CurrentBet = bet;

            if (_round != null)
            {
                _round.PropertyChanged -= OnRoundPropertyChanged;
            }
            _round = new Round(_cardsProvider, _dealerStrategy);
            _round.PropertyChanged += OnRoundPropertyChanged;
            _round.StartRound();

            if (RoundResult != RoundResults.RoundIsInProgress)
            {
                RedistributeBet();
            }
        }

        private void OnRoundPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// Player "hits" (asks for another card).
        /// </summary>
        public void Hit()
        {
            if (GameState != GameStates.GameIsInProgress)
                return;
            if (RoundState != RoundStates.PlayersTurn)
                return;

            _round.Hit();

            if (RoundResult != RoundResults.RoundIsInProgress)
            {
                RedistributeBet();
            }
        }

        /// <summary>
        /// Playes "stands" (no more cards required).
        /// </summary>
        public void Stand()
        {
            if (GameState != GameStates.GameIsInProgress)
                return;
            if (RoundState != RoundStates.PlayersTurn)
                return;

            _round.Stand();

            RedistributeBet();
        }

        private void RedistributeBet()
        {
            if(GameState != GameStates.GameIsInProgress)
                return;

            switch (RoundResult)
            {
                case RoundResults.PlayerHasWon:
                case RoundResults.BlackJack:
                case RoundResults.DealerHasBusted:
                    PlayerMoney += CurrentBet;
                    DealerMoney -= CurrentBet;
                    break;
                case RoundResults.PlayerHasBusted:
                case RoundResults.DealerHasWon:
                    PlayerMoney -= CurrentBet;
                    DealerMoney += CurrentBet;
                    break;
            }

            CheckIfGameIsOver();
        }

        private void CheckIfGameIsOver()
        {
            if (PlayerMoney <= 0)
            {
                GameState = GameStates.DealerHasWon;
            }
            else if (DealerMoney <= 0)
            {
                GameState = GameStates.PlayerHasWon;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}