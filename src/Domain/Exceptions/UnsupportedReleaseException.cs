namespace PoolTools.Player.Domain.Exceptions;
public class UnsupportedReleaseException : Exception
{
    public UnsupportedReleaseException(int playerId) : base($"Unsupported release for player {playerId}. Player is already unsigned.")
    {
    }
}
