using System.Collections.Generic;
using EdlinSoftware.Cards;

namespace EdlinSoftware.BlackJack
{
    public class BlackJack
    {
        public static readonly int TwentyOne = 21;

        public static readonly int AceIncrement = 10;

        public static readonly IReadOnlyDictionary<Ranks, int> CardValues = new Dictionary<Ranks, int>
        {
            { Ranks.Ace, 1 },
            { Ranks.Two, 2 },
            { Ranks.Three, 3 },
            { Ranks.Four, 4 },
            { Ranks.Five, 5 },
            { Ranks.Six, 6 },
            { Ranks.Seven, 7 },
            { Ranks.Eight, 8 },
            { Ranks.Nine, 9 },
            { Ranks.Ten, 10 },
            { Ranks.Jack, 10 },
            { Ranks.Queen, 10 },
            { Ranks.King, 10 }
        };
    }
}