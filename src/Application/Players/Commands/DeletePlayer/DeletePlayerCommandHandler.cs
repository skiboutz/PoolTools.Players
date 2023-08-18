using PoolTools.Player.Application.Common.Interfaces;
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

        player.AddDomainEvent(new PlayerDeletedEvent { PlayerId = request.PlayerId });

        //TODO: Deactivate player instead of deleting it to keep contract history?
        _context.Players.Remove(player);
        return await _context.SaveChangesAsync(cancellationToken);
                
    }
}
