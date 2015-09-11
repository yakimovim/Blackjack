using EdlinSoftware.Cards;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace EdlinSoftware.BlackJack.Tests.Model
{
    public class CardsProviderTest
    {
        [Theory]
        [AutoData]
        public void CardsProvider_ShouldReturnEndlessStreamOfCards([Frozen(As = typeof(IDeckCreator))] FullDeckCreator deckCreator, CardsProvider cardsProvider)
        {
            for (int i = 0; i < 1000; i++)
            {
                Assert.NotNull(cardsProvider.Deal());
            }
        }
    }
}