using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoolTools.Player.Domain.Entities;

namespace PoolTools.Player.Infrastructure.Data.Configurations;
public sealed class PlayerConfiguration : IEntityTypeConfiguration<Domain.Entities.Player>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Player> builder)
    {
        builder.Property(p => p.FirstName)
            .HasMaxLength(50);

        builder.Property(p => p.LastName)
            .HasMaxLength(100);

        builder.Property(p => p.Position)
            .HasMaxLength(2);

        builder.Property(p => p.DateOfBirth)
            .HasDefaultValueSql("DATEADD(year, -18, CAST(GETDATE() as DATE))");

        builder.HasOne(p => p.Team).WithMany(t => t.Roster).HasForeignKey(p => p.TeamId);

        builder.Navigation(p => p.Team).AutoInclude();

        builder.HasOne(p => p.Contract).WithOne(c => c.Player).HasForeignKey<Domain.Entities.Player>(p => p.ContractId);

        builder.Navigation(p => p.Contract).AutoInclude();
    }
}
