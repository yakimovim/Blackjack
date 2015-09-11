using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EdlinSoftware.Cards;

namespace EdlinSoftware.BlackJack
{
    /// <summary>
    /// Represents one hand in BlackJack game.
    /// </summary>
    public class Hand
    {
        private readonly LinkedList<Card> _cards = new LinkedList<Card>();

        public IEnumerable<Card> Cards => _cards;

            /// <summary>
        /// Adds one card to the hand.
        /// </summary>
        /// <param name="card">Cardr to add.</param>
        [DebuggerStepThrough]
        public void AddCard(Card card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            _cards.AddLast(card);
        }

        /// <summary>
        /// Returns value of the hand.
        /// </summary>
        public int GetValue()
        {
            var value = _cards.Sum(c => BlackJack.CardValues[c.Rank]);

            return AceShouldBeEleven(value) ? value + BlackJack.AceIncrement : value;
        }

        private bool AceShouldBeEleven(int value)
        {
            return _cards.Any(c => c.Rank == Ranks.Ace)
                   && value + BlackJack.AceIncrement <= BlackJack.TwentyOne;
        }
    }
}