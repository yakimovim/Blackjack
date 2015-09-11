using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace EdlinSoftware.Cards.Tests
{
    public class DeckCreatorTests
    {
        [Theory]
        [AutoData]
        public void FullDeckCreator_ShouldCreateDeckWith52Cards(FullDeckCreator creator)
        {
            var allCardsFromDeck = GetAllCardsFromDeck(creator);

            Assert.Equal(52, allCardsFromDeck.Length);
        }

        [Theory]
        [AutoData]
        public void SmallDeckCreator_ShouldCreateDeckWith36Cards(SmallDeckCreator creator)
        {
            var allCardsFromDeck = GetAllCardsFromDeck(creator);

            Assert.Equal(36, allCardsFromDeck.Length);
        }

        [Theory]
        [AutoData]
        public void FullDeckCreator_ShouldCreateDeckOfUniqueCards(FullDeckCreator creator)
        {
            var allCardsFromDeck = GetAllCardsFromDeck(creator);

            Assert.True(UniqueCards(allCardsFromDeck));
        }

        [Theory]
        [AutoData]
        public void SmallDeckCreator_ShouldCreateDeckOfUniqueCards(SmallDeckCreator creator)
        {
            var allCardsFromDeck = GetAllCardsFromDeck(creator);

            Assert.True(UniqueCards(allCardsFromDeck));
        }

        private Card[] GetAllCardsFromDeck(IDeckCreator deckCreator)
        {
            var cards = new LinkedList<Card>();

            var deck = deckCreator.CreateDeck();

            while (!deck.IsEmpty)
            {
                cards.AddLast(deck.Deal());
            }

            return cards.ToArray();
        }

        private bool UniqueCards(Card[] allCardsFromDeck)
        {
            return allCardsFromDeck.All(c => allCardsFromDeck.Count(c.Equals) == 1);
        }
    }
}