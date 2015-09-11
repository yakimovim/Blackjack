using System.Diagnostics;

namespace EdlinSoftware.Cards
{
    /// <summary>
    /// Represents one card.
    /// </summary>
    public class Card
    {
        public Suits Suit { get; }
        public Ranks Rank { get; }

        [DebuggerStepThrough]
        public Card(Ranks rank, Suits suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Card;

            return other != null
                   && other.Suit == Suit
                   && other.Rank == Rank;
        }

        public override int GetHashCode()
        {
            return ((int)Rank << 2) + (int)Suit;
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}
