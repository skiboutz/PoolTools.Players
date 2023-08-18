using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoolTools.Player.Domain.Entities;

namespace PoolTools.Player.Infrastructure.Data.Configurations;
public sealed class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.Property(t => t.City).HasMaxLength(100);
        builder.Property(t => t.Code).HasMaxLength(3);
        builder.Property(t => t.Name).HasMaxLength(100);
    }
}
