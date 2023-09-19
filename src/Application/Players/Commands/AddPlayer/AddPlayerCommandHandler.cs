using PoolTools.Player.Application.Common.Interfaces;
using PoolTools.Player.Domain.Entities;
using PoolTools.Player.Domain.Events;

namespace PoolTools.Player.Application.Players.Commands.AddPlayer;

public class AddPlayerCommandHandler : IRequestHandler<AddPlayerCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddPlayerCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(AddPlayerCommand request, CancellationToken cancellationToken)
    {
        var player = _mapper.Map<Domain.Entities.Player>(request.NewPlayer);
        player.Team = await GetTeam(request.NewPlayer.Team);
        player.AddDomainEvent(new PlayerCreatedEvent { Player = player });

        _context.Players.Add(player);

        await _context.SaveChangesAsync(cancellationToken);

        return player.Id;
    }

    private async Task<Team> GetTeam(string teamCode)
    {
        var team = await _context.Teams.FirstOrDefaultAsync(t => t.Code == teamCode);

        Guard.Against.NotFound(teamCode, team);
        return team;
    }
}
