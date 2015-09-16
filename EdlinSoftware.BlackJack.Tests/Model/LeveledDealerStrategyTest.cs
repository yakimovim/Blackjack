using EdlinSoftware.Cards;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using Xunit.Abstractions;

namespace EdlinSoftware.BlackJack.Tests.Model
{
    public class LeveledDealerStrategyTest
    {
        private readonly ITestOutputHelper _outputHelper;

        public LeveledDealerStrategyTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Theory]
        [AutoData]
        public void Play_ShouldAddCardsUntil17(LeveledDealerStrategy dealerStrategy, 
            Hand dealerHand, 
            [Frozen(As = typeof(IDeckCreator))] FullDeckCreator deckCreator,
            EndlessCardsProvider cardsProvider)
        {
            dealerStrategy.Play(dealerHand, cardsProvider);

            _outputHelper.WriteLine(string.Join(", ", dealerHand.Cards));

            Assert.True(dealerHand.GetValue() >= 17);
        }
    }
}