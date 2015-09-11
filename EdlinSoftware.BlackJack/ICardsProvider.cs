using System;
using System.Diagnostics;
using EdlinSoftware.Cards;

namespace EdlinSoftware.BlackJack
{
    /// <summary>
    /// Provider of endless stream of cards.
    /// </summary>
    public interface ICardsProvider
    {
        /// <summary>
        /// Returns one card.
        /// </summary>
        Card Deal();
    }

    public class CardsProvider : ICardsProvider
    {
        private readonly IDeckCreator _deckCreator;

        private Deck _deck;

        [DebuggerStepThrough]
        public CardsProvider(IDeckCreator deckCreator)
        {
            if (deckCreator == null) throw new ArgumentNullException(nameof(deckCreator));
            _deckCreator = deckCreator;
        }

        public Card Deal()
        {
            while (_deck == null || _deck.IsEmpty)
            {
                _deck = _deckCreator.CreateDeck();
                _deck.Shuffle();
            }

            return _deck.Deal();
        }
    }
}