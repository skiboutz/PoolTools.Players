using PoolTools.Player.Application.Common.Interfaces;

namespace PoolTools.Player.Application.Players.Commands.TradePlayer;

public class TradePlayerCommandValidator : AbstractValidator<TradePlayerCommand>
{
    private readonly IApplicationDbContext _context;

    public TradePlayerCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.PlayerId)
            .MustAsync(PlayerExists)
            .WithMessage("Cannot trade innexistant player");

        RuleFor(v => v.NewTeamCode)
            .Length(3)
            .MustAsync(TeamExists)
            .WithMessage("Cannot trade to innexistant team.");

        RuleFor(v => v)
            .MustAsync(TradeToNewTeam)
            .WithMessage("Cannot trade to current team.");
    }

    private async Task<bool> TeamExists(string teamCode, CancellationToken cancellationToken)
    {
        var team = await _context.Teams.AsNoTracking().SingleOrDefaultAsync(t => t.Code == teamCode,cancellationToken);

        return team is not null;

    }

    private async Task<bool> PlayerExists(int playerId, CancellationToken cancellationToken)
    {
        var player = await _context.Players.AsNoTracking().SingleOrDefaultAsync(p => p.Id == playerId, cancellationToken);

        return player is not null;
    }

    private async Task<bool> TradeToNewTeam(TradePlayerCommand command, CancellationToken cancellationToken)
    {
        var player = await _context.Players.AsNoTracking().Include(p => p.Team).SingleOrDefaultAsync(p => p.Id == command.PlayerId, cancellationToken);

        return player?.Team?.Code != command.NewTeamCode;
    }

}
