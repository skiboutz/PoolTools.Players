using PoolTools.Player.Application.Common.Interfaces;

namespace PoolTools.Player.Application.Players.Queries.GetPlayers;

public class GetPlayersQueryHandler : IRequestHandler<GetPlayersQuery, List<PlayerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPlayersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PlayerDto>> Handle(GetPlayersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Players.AsNoTracking()
                .ProjectTo<PlayerDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.LastName)
                .ToListAsync(cancellationToken);
    }
}
