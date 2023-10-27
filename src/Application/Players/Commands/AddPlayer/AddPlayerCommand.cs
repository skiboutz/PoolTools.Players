using PoolTools.Player.Application.Common.Security;

namespace PoolTools.Player.Application.Players.Commands.AddPlayer;

public record AddPlayerCommand : IRequest<int>
{
    public required AddPlayerDto NewPlayer { get; init; }
}
