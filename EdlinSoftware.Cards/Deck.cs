using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace EdlinSoftware.Cards
{
    /// <summary>
    /// Represents deck of cards.
    /// </summary>
    public class Deck
    {
        private readonly List<Card> _cards;

        [DebuggerStepThrough]
        public Deck(IEnumerable<Card> cards)
        {
            if (cards == null) throw new ArgumentNullException(nameof(cards));

            _cards = new List<Card>(cards);
        }

        /// <summary>
        /// Gets if the deck is empty.
        /// </summary>
        public bool IsEmpty => _cards.Count == 0;

        /// <summary>
        /// Shuffles the deck.
        /// </summary>
        public void Shuffle()
        {
            using (var rndGen = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[_cards.Count];
                rndGen.GetBytes(bytes);

                int[] ints = bytes.Select(b => b - byte.MaxValue / 2).ToArray();

                var rnd = new Random((int)DateTime.UtcNow.Ticks);

                _cards.Sort((c1, c2) => ints[rnd.Next(0, ints.Length)]);
            }
        }

        /// <summary>
        /// Deals one card from the deck.
        /// </summary>
        /// <returns>One card from the deck.</returns>
        public Card Deal()
        {
            if(IsEmpty)
                throw new InvalidOperationException("Deck is empty.");

            var lastIndex = _cards.Count - 1;

            var card = _cards[lastIndex];

            _cards.RemoveAt(lastIndex);

            return card;
        }
    }
}
