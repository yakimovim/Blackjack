namespace EdlinSoftware.Cards
{
    /// <summary>
    /// Represents ranks of cards.
    /// </summary>
    public enum Ranks
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

    public static class RanksExtensions
    {
        /// <summary>
        /// Create one card.
        /// </summary>
        /// <param name="rank">Rank of card.</param>
        /// <param name="suit">Suit of card.</param>
        /// <returns>One card.</returns>
        public static Card Of(this Ranks rank, Suits suit)
        {
            return new Card(rank, suit);
        }
    }
}