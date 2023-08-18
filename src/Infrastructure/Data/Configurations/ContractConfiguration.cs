using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoolTools.Player.Domain.Entities;

namespace PoolTools.Player.Infrastructure.Data.Configurations;
public sealed class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.Property(c => c.Salary)
            .HasPrecision(10, 2);

        builder.Property(c => c.CapHit)
            .HasPrecision(10, 2);

        builder.Property(c => c.AnnualAverage)
            .HasColumnName("AAV")
            .HasPrecision(10, 2);
    }
}
