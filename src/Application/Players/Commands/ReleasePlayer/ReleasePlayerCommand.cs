using PoolTools.Player.Application.Common.Security;

namespace PoolTools.Player.Application.Players.Commands.ReleasePlayer;

[Authorize]
public record ReleasePlayerCommand : IRequest<int>
{
    public int PlayerId { get; set; }
}
