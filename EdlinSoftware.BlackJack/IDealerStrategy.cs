﻿using System;

namespace EdlinSoftware.BlackJack
{
    public interface IDealerStrategy
    {
        void Play(Hand dealersHand, ICardsProvider cardsProvider);
    }

    public class LeveledDealerStrategy : IDealerStrategy
    {
        private readonly int _level;

        public LeveledDealerStrategy() : this(17)
        { }

        internal LeveledDealerStrategy(int level)
        {
            _level = level;
        }

        public void Play(Hand dealersHand, ICardsProvider cardsProvider)
        {
            if (dealersHand == null) throw new ArgumentNullException(nameof(dealersHand));
            if (cardsProvider == null) throw new ArgumentNullException(nameof(cardsProvider));

            while (dealersHand.GetValue() < _level)
            {
                dealersHand.AddCard(cardsProvider.Deal());
            }
        }
    }
}