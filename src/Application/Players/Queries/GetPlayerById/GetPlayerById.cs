namespace PoolTools.Player.Application.Players.Queries.GetPlayerById;

public record GetPlayerByIdQuery : IRequest<PlayerDetailsDto>
{
    public int PlayerId { get; set; }
}
