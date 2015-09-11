using System.Linq;
using EdlinSoftware.BlackJack.Tests.Framework;
using EdlinSoftware.Cards;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using static EdlinSoftware.Cards.Ranks;

namespace EdlinSoftware.BlackJack.Tests.Model
{
    public class RoundTest
    {
        [Theory]
        [AutoData]
        public void NewRoundIsInCorrectState(
            [Frozen(As = typeof(IDeckCreator))] FullDeckCreator deckCreator,
            [Frozen(As = typeof(ICardsProvider))] CardsProvider cardsProvider,
            [Frozen(As = typeof(IDealerStrategy))] LeveledDealerStrategy dealerStrategy,
            Round round)
        {
            Assert.Equal(RoundResults.RoundNotStarted, round.RoundResult);
            Assert.Equal(RoundStates.RoundIsOver, round.RoundState);
        }

        [Theory]
        [AutoData]
        public void NewRoundHasNoCards(
            [Frozen(As = typeof(IDeckCreator))] FullDeckCreator deckCreator,
            [Frozen(As = typeof(ICardsProvider))] CardsProvider cardsProvider,
            [Frozen(As = typeof(IDealerStrategy))] LeveledDealerStrategy dealerStrategy,
            Round round)
        {
            Assert.Null(round.PlayersCards);
            Assert.Null(round.DealersCards);
        }

        [Theory]
        [AutoData]
        public void StartRound_DealsTwoCardsToEachPlayer(
            [Frozen(As = typeof(IDeckCreator))] FullDeckCreator deckCreator,
            [Frozen(As = typeof(ICardsProvider))] CardsProvider cardsProvider,
            [Frozen(As = typeof(IDealerStrategy))] LeveledDealerStrategy dealerStrategy,
            Round round)
        {
            round.StartRound();

            Assert.Equal(2, round.PlayersCards.Count());
            Assert.Equal(2, round.DealersCards.Count());
        }

        [Theory]
        [CardsDataForRound(Two, Two, Two, Two, Ten)]
        public void Hit_DealsOneCardToPlayer(Round round)
        {
            round.StartRound();

            round.Hit();

            Assert.Equal(3, round.PlayersCards.Count());
        }

        [Theory]
        [CardsDataForRound(Two, Two, Two, Two, Ten, Ten)]
        public void SeveralHits_UserHasBusted(Round round)
        {
            round.StartRound();

            while (round.RoundResult != RoundResults.PlayerHasBusted)
            { round.Hit(); }
        }

        [Theory]
        [CardsDataForRound(Two, Two, Two, Two, Ten, Ten)]
        public void RoundResultPropertyChangedEventWorks(Round round)
        {
            round.StartRound();

            Assert.PropertyChanged(round, nameof(round.RoundResult), () =>
            {
                while (round.RoundResult != RoundResults.PlayerHasBusted)
                { round.Hit(); }
            });
        }

        [Theory]
        [CardsDataForRound(Two, Two, Two, Two, Ten, Ten)]
        public void RoundStatePropertyChangedEventWorks(Round round)
        {
            round.StartRound();

            Assert.PropertyChanged(round, nameof(round.RoundState), () =>
            {
                while (round.RoundResult != RoundResults.PlayerHasBusted)
                { round.Hit(); }
            });
        }

        [Theory]
        [CardsDataForRound(Ten, Two, Ten, Two, Ten)]
        public void TestPlayerHasBusted(Round round)
        {
            round.StartRound();

            round.Hit();

            Assert.Equal(RoundResults.PlayerHasBusted, round.RoundResult);
        }

        [Theory]
        [CardsDataForRound(Two, Ten, Two, Five, Ten)]
        public void TestDealerHasBusted(Round round)
        {
            round.StartRound();

            round.Stand();

            Assert.Equal(RoundResults.DealerHasBusted, round.RoundResult);
        }

        [Theory]
        [CardsDataForRound(Ten, Two, Ten, Seven, Ten)]
        public void TestPlayerHasWon(Round round)
        {
            round.StartRound();

            round.Stand();

            Assert.Equal(RoundResults.PlayerHasWon, round.RoundResult);
        }

        [Theory]
        [CardsDataForRound(Two, Ten, Two, Eight)]
        public void TestDealerHasWon(Round round)
        {
            round.StartRound();

            round.Stand();

            Assert.Equal(RoundResults.DealerHasWon, round.RoundResult);
        }

        [Theory]
        [CardsDataForRound(Eight, Eight, Ten, Ten)]
        public void TestPush(Round round)
        {
            round.StartRound();

            round.Stand();

            Assert.Equal(RoundResults.Push, round.RoundResult);
        }

        [Theory]
        [CardsDataForRound(Ten, Eight, Ace, Ten)]
        public void TestBlackJack(Round round)
        {
            round.StartRound();

            round.Stand();

            Assert.Equal(RoundResults.BlackJack, round.RoundResult);
        }

        [Theory]
        [CardsDataForRound(Ten, Ten, Ace, Ace)]
        public void TestPushOnBlackJacks(Round round)
        {
            round.StartRound();

            round.Stand();

            Assert.Equal(RoundResults.Push, round.RoundResult);
        }

        [Theory]
        [CardsDataForRound(Ten, Ten, Ace, Two)]
        public void Hit_ShouldNotAddCards_WhenRoundIsFinished(Round round)
        {
            round.StartRound();

            round.Hit();

            Assert.Equal(2, round.PlayersCards.Count());
        }
    }
}
