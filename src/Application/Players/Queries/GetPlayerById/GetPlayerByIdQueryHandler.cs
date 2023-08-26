using PoolTools.Player.Application.Common.Interfaces;
using PoolTools.Player.Application.Players.Queries.GetPlayers;

namespace PoolTools.Player.Application.Players.Queries.GetPlayerById;

public class GetPlayerByIdQueryHandler : IRequestHandler<GetPlayerByIdQuery, PlayerDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public GetPlayerByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PlayerDto> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
    {
        var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == request.PlayerId, cancellationToken: cancellationToken);

        Guard.Against.NotFound(request.PlayerId, player);

        var playerDto = _mapper.Map<PlayerDto>(player);
        playerDto.Age = CalculateAge(player.DateOfBirth);
        playerDto.YearRemaining = CalculateContractYearsRemaining(player.Contract?.ExpirationYear);

        return playerDto;

    }

    private int CalculateAge(DateTime dateOfBirth)
    {
        var today = _dateTime.Today;

        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    private int CalculateContractYearsRemaining(int? expirationYear)
    {
        if (!expirationYear.HasValue)
        {
            return 0;
        }

        var today = _dateTime.Today;

        return today.Month >= 7 ? expirationYear.Value - today.Year : expirationYear.Value - today.Year + 1;
    }
}
