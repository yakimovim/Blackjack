using System;
using EdlinSoftware.Cards;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using static EdlinSoftware.Cards.Ranks;

namespace EdlinSoftware.BlackJack.Tests.Model
{
    public class HandTest
    {
        [Theory]
        [AutoData]
        public void AddCard_ShouldThrowException_IfCardIsNull(Hand hand)
        {
            Assert.Throws<ArgumentNullException>(() => hand.AddCard(null));
        }

        [Theory]
        [AutoData]
        public void GetValue_ShouldReturnCorrectResult_ForOneCard(Hand hand, Card card)
        {
            var expectedValue = BlackJack.CardValues[card.Rank];
            if (card.Rank == Ace)
            {
                expectedValue += BlackJack.AceIncrement;
            }

            hand.AddCard(card);

            Assert.Equal(expectedValue, hand.GetValue());
        }

        [Theory]
        [AutoData]
        public void Cards_ShouldReturnCorrectResult(Hand hand, Card[] cards)
        {
            foreach (var card in cards)
            {
                hand.AddCard(card);
            }

            Assert.Equal(cards, hand.Cards);
        }

        [Theory]
        [InlineAutoData(Two, Two, 4)]
        [InlineAutoData(Eight, Seven, 15)]
        [InlineAutoData(Ten, King, 20)]
        [InlineAutoData(Queen, Ace, 21)]
        [InlineAutoData(Ace, Ace, 12)]
        public void GetValue_ShouldReturnCorrectResult_ForTwoCards(Ranks rank1, Ranks rank2, int expectedValue, Hand hand, Suits suit)
        {
            hand.AddCard(rank1.Of(suit));
            hand.AddCard(rank2.Of(suit));

            Assert.Equal(expectedValue, hand.GetValue());
        }

        [Theory]
        [InlineAutoData(Two, Two, Ten, 14)]
        [InlineAutoData(Seven, Five, Nine, 21)]
        [InlineAutoData(King, Five, Nine, 24)]
        [InlineAutoData(King, Ace, Nine, 20)]
        [InlineAutoData(King, Ace, Ace, 12)]
        [InlineAutoData(Two, Ace, Seven, 20)]
        public void GetValue_ShouldReturnCorrectResult_ForThreeCards(Ranks rank1, Ranks rank2, Ranks rank3, int expectedValue, Hand hand, Suits suit)
        {
            hand.AddCard(rank1.Of(suit));
            hand.AddCard(rank2.Of(suit));
            hand.AddCard(rank3.Of(suit));

            Assert.Equal(expectedValue, hand.GetValue());
        }
    }
}