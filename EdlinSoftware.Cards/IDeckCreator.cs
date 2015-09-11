using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Cards
{
    /// <summary>
    /// Represents creator of one unshuffled deck of cards.
    /// </summary>
    public interface IDeckCreator
    {
        /// <summary>
        /// Creates unshuffled deck of cards.
        /// </summary>
        Deck CreateDeck();
    }

    public abstract class DeckCreator : IDeckCreator
    {
        public virtual Deck CreateDeck()
        {
            return new Deck(GetAllCardsInDeck());
        }

        protected abstract IEnumerable<Card> GetAllCardsInDeck();
    }

    public class FullDeckCreator : DeckCreator
    {
        protected override IEnumerable<Card> GetAllCardsInDeck()
        {
            return from Suits suit in Enum.GetValues(typeof(Suits))
                   from Ranks rank in Enum.GetValues(typeof(Ranks))
                   select rank.Of(suit);
        }
    }

    public class SmallDeckCreator : DeckCreator
    {
        private static readonly Ranks[] ProhibitedRanks =
        {
            Ranks.Two,
            Ranks.Three,
            Ranks.Four,
            Ranks.Five
        };

        protected override IEnumerable<Card> GetAllCardsInDeck()
        {
            return from Suits suit in Enum.GetValues(typeof(Suits))
                   from Ranks rank in Enum.GetValues(typeof(Ranks))
                   where !ProhibitedRanks.Contains(rank)
                   select rank.Of(suit);
        }
    }
}