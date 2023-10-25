using System.Reflection;
using PoolTools.Player.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using PoolTools.Player.Domain.Entities;

namespace PoolTools.Player.Infrastructure.Data;

public class ApplicationDbContext : DbContext,IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Domain.Entities.Player> Players => Set<Domain.Entities.Player>();
    public DbSet<Team> Teams => Set<Team>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
