namespace PoolTools.Player.Domain.Entities;
public class Contract : BaseAuditableEntity
{
    public Player Player { get; set; } = default!;
    public required int ExpirationYear { get; set; }
    public required decimal Salary { get; set; }
    public required decimal CapHit { get; set; }
    public required decimal AnnualAverage { get; set; }
}
