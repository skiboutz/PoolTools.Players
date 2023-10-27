using PoolTools.Player.Application.Common.Security;

namespace PoolTools.Player.Application.Players.Commands.TradePlayer;

public record TradePlayerCommand : IRequest<int>
{
    public int PlayerId { get; set; }
    public required string NewTeamCode { get; set; }
}
