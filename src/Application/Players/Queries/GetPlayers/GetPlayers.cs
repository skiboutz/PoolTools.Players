using PoolTools.Player.Application.Common.Security;

namespace PoolTools.Player.Application.Players.Queries.GetPlayers;

public record GetPlayersQuery : IRequest<List<PlayerDto>>
{
}
