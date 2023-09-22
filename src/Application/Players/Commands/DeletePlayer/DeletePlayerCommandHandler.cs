using PoolTools.Player.Application.Common.Interfaces;
using PoolTools.Player.Domain.Enums;
using PoolTools.Player.Domain.Events;

namespace PoolTools.Player.Application.Players.Commands.DeletePlayer;

public class DeletePlayerCommandHandler : IRequestHandler<DeletePlayerCommand, int>
{
    private readonly IApplicationDbContext _context;

    public DeletePlayerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _context.Players.FindAsync(new object?[] { request.PlayerId }, cancellationToken: cancellationToken);

        Guard.Against.NotFound(request.PlayerId, player);

        player.Status = PlayerStatus.InActive;

        player.AddDomainEvent(new PlayerDeletedEvent { PlayerId = request.PlayerId });

        return await _context.SaveChangesAsync(cancellationToken);
                
    }
}
