using PoolTools.Player.Domain.Entities;

namespace PoolTools.Player.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Player> Players { get; }
    DbSet<Team> Teams { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
