using PoolTools.Player.Application.Common.Interfaces;

namespace PoolTools.Player.Application.Players.Commands.ReleasePlayer;

public class ReleasePlayerCommandHandler : IRequestHandler<ReleasePlayerCommand, int>
{
    private readonly IApplicationDbContext _context;

    public ReleasePlayerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(ReleasePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _context.Players.FindAsync(new object?[] { request.PlayerId}, cancellationToken);

        Guard.Against.NotFound(request.PlayerId, player);

        player.Release();

        return await _context.SaveChangesAsync(cancellationToken);
    }
}
