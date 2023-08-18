namespace PoolTools.Player.Domain.Exceptions;
public class UnsupportedTradeException : Exception
{
    public UnsupportedTradeException(int playerId, string teamCode) : base($"Unsupported Trade for player {playerId}. Player already on team {teamCode}.")
    {
    }
}
