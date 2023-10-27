using PoolTools.Player.Application.Common.Security;

namespace PoolTools.Player.Application.Players.Commands.DeletePlayer;

public record DeletePlayerCommand : IRequest<int>
{
    public int PlayerId { get; set; }
}
