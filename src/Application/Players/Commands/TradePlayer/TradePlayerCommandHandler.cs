using PoolTools.Player.Application.Common.Interfaces;
using PoolTools.Player.Domain.Exceptions;

namespace PoolTools.Player.Application.Players.Commands.TradePlayer;

public class TradePlayerCommandHandler : IRequestHandler<TradePlayerCommand, int>
{
    private readonly IApplicationDbContext _context;

    public TradePlayerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(TradePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _context.Players.FindAsync(new object?[] { request.PlayerId }, cancellationToken);

        Guard.Against.NotFound(request.PlayerId,player);

        var team = await _context.Teams.AsNoTracking().SingleOrDefaultAsync(t => t.Code == request.NewTeamCode,cancellationToken);

        Guard.Against.NotFound(request.NewTeamCode , team);

        player.Trade(team);

        return await _context.SaveChangesAsync(cancellationToken);
       
    }
}
