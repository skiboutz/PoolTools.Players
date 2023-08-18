using PoolTools.Player.Application.Players.Queries.GetPlayers;

namespace PoolTools.Player.Application.Players.Queries.GetPlayerById;

public record GetPlayerByIdQuery : IRequest<PlayerDto>
{
    public int PlayerId { get; set; }
}
