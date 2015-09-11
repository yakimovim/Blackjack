using System.Linq;
using EdlinSoftware.BlackJack.Tests.Framework;
using EdlinSoftware.Cards;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using static EdlinSoftware.Cards.Ranks;

namespace EdlinSoftware.BlackJack.Tests.Model
{
    public class GameTest
    {
        [Theory]
        [AutoData]
        public void NewGameIsInCorrectState(
            [Frozen(As = typeof(IDeckCreator))] FullDeckCreator deckCreator,
            [Frozen(As = typeof(ICardsProvider))] CardsProvider cardsProvider,
            [Frozen(As = typeof(IDealerStrategy))] LeveledDealerStrategy dealerStrategy,
            Game game)
        {
            Assert.Equal(RoundResults.RoundNotStarted, game.RoundResult);
            Assert.Equal(RoundStates.RoundIsOver, game.RoundState);
            Assert.Equal(GameStates.GameIsInProgress, game.GameState);
        }

        [Theory]
        [AutoData]
        public void NewGameHasInitialPositiveMoney(
            [Frozen(As = typeof(IDeckCreator))] FullDeckCreator deckCreator,
            [Frozen(As = typeof(ICardsProvider))] CardsProvider cardsProvider,
            [Frozen(As = typeof(IDealerStrategy))] LeveledDealerStrategy dealerStrategy,
            Game game)
        {
            Assert.True(game.PlayerMoney > 0);
            Assert.True(game.DealerMoney > 0);
        }

        [Theory]
        [AutoData]
        public void NewGameHasNoCards(
            [Frozen(As = typeof(IDeckCreator))] FullDeckCreator deckCreator,
            [Frozen(As = typeof(ICardsProvider))] CardsProvider cardsProvider,
            [Frozen(As = typeof(IDealerStrategy))] LeveledDealerStrategy dealerStrategy,
            Game game)
        {
            Assert.Null(game.PlayersCards);
            Assert.Null(game.DealersCards);
        }

        [Theory]
        [AutoData]
        public void StartRound_DealsTwoCardsToEachPlayer(
            [Frozen(As = typeof(IDeckCreator))] FullDeckCreator deckCreator,
            [Frozen(As = typeof(ICardsProvider))] CardsProvider cardsProvider,
            [Frozen(As = typeof(IDealerStrategy))] LeveledDealerStrategy dealerStrategy,
            Game game)
        {
            game.StartRound(2);

            Assert.Equal(2, game.PlayersCards.Count());
            Assert.Equal(2, game.DealersCards.Count());
        }

        [Theory]
        [AutoData]
        public void StartRound_SetsCorrectBet(
            [Frozen(As = typeof(IDeckCreator))] FullDeckCreator deckCreator,
            [Frozen(As = typeof(ICardsProvider))] CardsProvider cardsProvider,
            [Frozen(As = typeof(IDealerStrategy))] LeveledDealerStrategy dealerStrategy,
            Game game)
        {
            game.StartRound(2);

            Assert.Equal(2, game.CurrentBet);
        }

        [Theory]
        [CardsDataForGame(Two, Two, Two, Two, Ten)]
        public void Hit_DealsOneCardToPlayer(Game game)
        {
            game.StartRound(2);

            game.Hit();

            Assert.Equal(3, game.PlayersCards.Count());
        }

        [Theory]
        [CardsDataForGame(Two, Two, Two, Two, Ten, Ten)]
        public void SeveralHits_UserHasBusted(Game game)
        {
            game.StartRound(2);

            while (game.RoundResult != RoundResults.PlayerHasBusted)
            { game.Hit(); }
        }

        [Theory]
        [CardsDataForGame(Two, Two, Two, Two, Ten, Ten)]
        public void RoundResultPropertyChangedEventWorks(Game game)
        {
            game.StartRound(2);

            Assert.PropertyChanged(game, nameof(game.RoundResult), () =>
            {
                while (game.RoundResult != RoundResults.PlayerHasBusted)
                { game.Hit(); }
            });
        }

        [Theory]
        [CardsDataForGame(Two, Two, Two, Two, Ten, Ten)]
        public void RoundStatePropertyChangedEventWorks(Game game)
        {
            game.StartRound(2);

            Assert.PropertyChanged(game, nameof(game.RoundState), () =>
            {
                while (game.RoundResult != RoundResults.PlayerHasBusted)
                { game.Hit(); }
            });
        }

        [Theory]
        [CardsDataForGame(Two, Two, Two, Two, Ten, Ten)]
        public void GameStatePropertyChangedEventWorks(Game game)
        {
            game.StartRound(CardsDataForGameAttribute.PlayerMoney);

            Assert.PropertyChanged(game, nameof(game.GameState), () =>
            {
                while (game.RoundResult != RoundResults.PlayerHasBusted)
                { game.Hit(); }
            });
        }

        [Theory]
        [CardsDataForGame(Ten, Two, Ten, Two, Ten)]
        public void TestPlayerHasBusted(Game game)
        {
            game.StartRound(2);

            game.Hit();

            Assert.Equal(RoundResults.PlayerHasBusted, game.RoundResult);
            Assert.Equal(4, game.DealerMoney - game.PlayerMoney);
        }

        [Theory]
        [CardsDataForGame(Two, Ten, Two, Five, Ten)]
        public void TestDealerHasBusted(Game game)
        {
            game.StartRound(2);

            game.Stand();

            Assert.Equal(RoundResults.DealerHasBusted, game.RoundResult);
            Assert.Equal(-4, game.DealerMoney - game.PlayerMoney);
        }

        [Theory]
        [CardsDataForGame(Ten, Two, Ten, Seven, Ten)]
        public void TestPlayerHasWon(Game game)
        {
            game.StartRound(2);

            game.Stand();

            Assert.Equal(RoundResults.PlayerHasWon, game.RoundResult);
            Assert.Equal(-4, game.DealerMoney - game.PlayerMoney);
        }

        [Theory]
        [CardsDataForGame(Two, Ten, Two, Eight)]
        public void TestDealerHasWon(Game game)
        {
            game.StartRound(2);

            game.Stand();

            Assert.Equal(RoundResults.DealerHasWon, game.RoundResult);
            Assert.Equal(4, game.DealerMoney - game.PlayerMoney);
        }

        [Theory]
        [CardsDataForGame(Eight, Eight, Ten, Ten)]
        public void TestPush(Game game)
        {
            game.StartRound(2);

            game.Stand();

            Assert.Equal(RoundResults.Push, game.RoundResult);
            Assert.Equal(0, game.DealerMoney - game.PlayerMoney);
        }

        [Theory]
        [CardsDataForGame(Ten, Eight, Ace, Ten)]
        public void TestBlackJack(Game game)
        {
            game.StartRound(2);

            game.Stand();

            Assert.Equal(RoundResults.BlackJack, game.RoundResult);
            Assert.Equal(-4, game.DealerMoney - game.PlayerMoney);
        }

        [Theory]
        [CardsDataForGame(Ten, Ten, Ace, Ace)]
        public void TestPushOnBlackJacks(Game game)
        {
            game.StartRound(2);

            game.Stand();

            Assert.Equal(RoundResults.Push, game.RoundResult);
            Assert.Equal(0, game.DealerMoney - game.PlayerMoney);
        }

        [Theory]
        [CardsDataForGame(Ten, Ten, Ace, Two)]
        public void Hit_ShouldNotAddCards_WhenRoundIsFinished(Game game)
        {
            game.StartRound(2);

            game.Hit();

            Assert.Equal(2, game.PlayersCards.Count());
        }
    }
}