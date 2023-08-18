using PoolTools.Player.Application.Common.Interfaces;
using PoolTools.Player.Application.Players.Queries.GetPlayers;

namespace PoolTools.Player.Application.Players.Queries.GetPlayerById;

public class GetPlayerByIdQueryHandler : IRequestHandler<GetPlayerByIdQuery, PlayerDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPlayerByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PlayerDto> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
    {
        var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == request.PlayerId, cancellationToken: cancellationToken);

        return _mapper.Map<PlayerDto>(player);

    }
}
