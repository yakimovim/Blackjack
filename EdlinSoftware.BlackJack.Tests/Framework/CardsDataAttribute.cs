using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EdlinSoftware.Cards;
using Ploeh.AutoFixture;
using Xunit.Sdk;

namespace EdlinSoftware.BlackJack.Tests.Framework
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class CardsDataForGameAttribute : DataAttribute
    {
        public const int PlayerMoney = 100;
        public const int DealerMoney = 100;

        private readonly Ranks[] _ranks;

        public CardsDataForGameAttribute(params Ranks[] ranks)
        {
            _ranks = ranks;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var fixture = new Fixture();

            var cards = _ranks.Select(r => r.Of(fixture.Create<Suits>())).ToArray();

            yield return new object[] { new Game(new FakeCardsProvider(cards), new LeveledDealerStrategy(), PlayerMoney, DealerMoney) };
        }
    }
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class CardsDataForRoundAttribute : DataAttribute
    {
        private readonly Ranks[] _ranks;

        public CardsDataForRoundAttribute(params Ranks[] ranks)
        {
            _ranks = ranks;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var fixture = new Fixture();

            var cards = _ranks.Select(r => r.Of(fixture.Create<Suits>())).ToArray();

            yield return new object[] { new Round(new FakeCardsProvider(cards), new LeveledDealerStrategy())  };
        }
    }
}