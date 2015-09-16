using System.Collections.Generic;
using EdlinSoftware.Cards;

namespace EdlinSoftware.BlackJack.Tests.Framework
{
    public class FakeCardsProvider : IEndlessCardsProvider
    {
        private readonly LinkedList<Card> _cards;

        public FakeCardsProvider(params Card[] cards)
        {
            _cards = new LinkedList<Card>(cards);
        }

        public Card Deal()
        {
            var card = _cards.First.Value;
            _cards.RemoveFirst();
            return card;
        }
    }
}