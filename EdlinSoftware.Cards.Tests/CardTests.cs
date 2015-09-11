using Ploeh.AutoFixture.Xunit2;
using Xunit;
using static EdlinSoftware.Cards.Ranks;
using static EdlinSoftware.Cards.Suits;

namespace EdlinSoftware.Cards.Tests
{
    public class CardTests
    {
        [Theory]
        [AutoData]
        public void ToString_ShouldReturnCorrectValue(Ranks rank, Suits suit)
        {
            Assert.Equal($"{rank} of {suit}", rank.Of(suit).ToString());
        }

        [Theory]
        [AutoData]
        public void Equals_ShouldReturnFalse_ForCardsWithDifferentRanks(Suits suit)
        {
            Assert.NotEqual(Ace.Of(suit), Eight.Of(suit));
        }

        [Theory]
        [AutoData]
        public void Equals_ShouldReturnFalse_ForCardsWithDifferentSuits(Ranks rank)
        {
            Assert.NotEqual(rank.Of(Clubs), rank.Of(Diamonds));
        }

        [Theory]
        [AutoData]
        public void Equals_ShouldReturnTrue_ForSameCards(Ranks rank, Suits suit)
        {
            Assert.Equal(rank.Of(suit), rank.Of(suit));
        }
    }
}