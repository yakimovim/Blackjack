namespace EdlinSoftware.BlackJack
{
    /// <summary>
    /// Represents result of one round of game.
    /// </summary>
    public enum RoundResults
    {
        RoundIsInProgress,
        PlayerHasWon,
        PlayerHasBusted,
        DealerHasWon,
        DealerHasBusted,
        Push,
        BlackJack,
        RoundNotStarted
    }
}