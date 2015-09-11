using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace EdlinSoftware.Cards.Tests
{
    public class DeckTests
    {
        private static readonly Deck EmptyDeck = new Deck(new Card[0]);

        [Theory]
        [AutoData]
        public void IsEmpty_ShouldReturnFalse_IfThereAreCardsInTheDeck(Deck deck)
        {
            Assert.False(deck.IsEmpty);
        }

        [Fact]
        public void IsEmpty_ShouldReturnTrue_IfThereAreNoCardsInTheDeck()
        {
            Assert.True(EmptyDeck.IsEmpty);
        }

        [Theory]
        [AutoData]
        public void Deal_ShouldProvideCards_WhileDeckIsNotEmpty(Deck deck)
        {
            while (!deck.IsEmpty)
            {
                Assert.IsType<Card>(deck.Deal());
            }
        }

        [Fact]
        public void Deal_ShouldThrowException_IfDeckIsEmpty()
        {
            var deck = EmptyDeck;

            Assert.Throws<InvalidOperationException>(() => deck.Deal());
        }

        [Fact]
        public void Shuffle_ShouldChangeOrderOfCards()
        {
            var fixture = new Fixture();

            Card[] cards = Enumerable.Range(1, 30).Select(i => fixture.Create<Card>()).ToArray();

            fixture.Inject((IEnumerable<Card>)cards);

            var deck1 = fixture.Create<Deck>();
            deck1.Shuffle();
            var deck2 = fixture.Create<Deck>();
            deck2.Shuffle();

            var cards1 = new List<Card>(cards.Length);
            var cards2 = new List<Card>(cards.Length);

            while (!deck1.IsEmpty)
            {
                cards1.Add(deck1.Deal());
                cards2.Add(deck2.Deal());
            }

            Assert.NotEqual(cards1, cards2);
        }
    }
}