using PoolTools.Player.Application.Common.Security;

namespace PoolTools.Player.Application.Players.Commands.ReleasePlayer;

public record ReleasePlayerCommand : IRequest<int>
{
    public int PlayerId { get; set; }
}
