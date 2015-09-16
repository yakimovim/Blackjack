using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using EdlinSoftware.Cards;

namespace EdlinSoftware.BlackJack
{
    /// <summary>
    /// Represents one round of blackjack game.
    /// </summary>
    public class Round : INotifyPropertyChanged
    {
        private const int InitialNumberOfCards = 2;

        private readonly IEndlessCardsProvider _cardsProvider;
        private readonly IDealerStrategy _dealerStrategy;

        private Hand _playersHand;
        private Hand _dealersHand;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RoundResults _roundResult = RoundResults.RoundNotStarted;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RoundStates _roundState = RoundStates.RoundIsOver;

        public Round(
            IEndlessCardsProvider cardsProvider, 
            IDealerStrategy dealerStrategy)
        {
            _cardsProvider = cardsProvider;
            _dealerStrategy = dealerStrategy;
        }

        /// <summary>
        /// Gets cards of player.
        /// </summary>
        public IEnumerable<Card> PlayersCards => _playersHand?.Cards;

        /// <summary>
        /// Gets cards of dealer.
        /// </summary>
        public IEnumerable<Card> DealersCards => _dealersHand?.Cards;

        public RoundResults RoundResult
        {
            [DebuggerStepThrough]
            get
            { return _roundResult; }
            private set
            {
                if (value == _roundResult) return;

                _roundResult = value;
                OnPropertyChanged();
            }
        }

        public RoundStates RoundState
        {
            [DebuggerStepThrough]
            get
            { return _roundState; }
            private set
            {
                if (value == _roundState) return;

                _roundState = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Starts new round of game.
        /// </summary>
        public void StartRound()
        {
            DealInitialCards();

            if (_playersHand.GetValue() == BlackJack.TwentyOne)
            {
                if (_dealersHand.GetValue() == BlackJack.TwentyOne)
                {
                    RoundState = RoundStates.RoundIsOver;
                    RoundResult = RoundResults.Push;
                }
                else
                {
                    RoundState = RoundStates.RoundIsOver;
                    RoundResult = RoundResults.BlackJack;
                }
            }
            else
            {
                RoundState = RoundStates.PlayersTurn;
                RoundResult = RoundResults.RoundIsInProgress;
            }
        }

        private void DealInitialCards()
        {
            _playersHand = new Hand();
            _dealersHand = new Hand();

            for (int i = 0; i < InitialNumberOfCards; i++)
            {
                _playersHand.AddCard(_cardsProvider.Deal());
                _dealersHand.AddCard(_cardsProvider.Deal());
            }

            OnPropertyChanged(nameof(PlayersCards));
            OnPropertyChanged(nameof(DealersCards));
        }

        /// <summary>
        /// Player "hits" (asks for another card).
        /// </summary>
        public void Hit()
        {
            if (RoundState != RoundStates.PlayersTurn)
                return;

            _playersHand.AddCard(_cardsProvider.Deal());
            OnPropertyChanged(nameof(PlayersCards));

            if (_playersHand.GetValue() > BlackJack.TwentyOne)
            {
                RoundResult = RoundResults.PlayerHasBusted;
                RoundState = RoundStates.RoundIsOver;
            }
        }

        /// <summary>
        /// Playes "stands" (no more cards required).
        /// </summary>
        public void Stand()
        {
            if (RoundState != RoundStates.PlayersTurn)
                return;

            RoundState = RoundStates.DealersTurn;

            _dealerStrategy.Play(_dealersHand, _cardsProvider);
            OnPropertyChanged(nameof(DealersCards));

            if (_dealersHand.GetValue() > BlackJack.TwentyOne)
            {
                RoundResult = RoundResults.DealerHasBusted;
            }
            else if (_dealersHand.GetValue() > _playersHand.GetValue())
            {
                RoundResult = RoundResults.DealerHasWon;
            }
            else if (_dealersHand.GetValue() < _playersHand.GetValue())
            {
                RoundResult = RoundResults.PlayerHasWon;
            }
            else
            {
                RoundResult = RoundResults.Push;
            }

            RoundState = RoundStates.RoundIsOver;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}