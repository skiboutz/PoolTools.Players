namespace PoolTools.Player.Domain.Entities;
public class Contract : BaseAuditableEntity
{
    public required Player Player { get; set; }
    public int ExpirationYear { get; set; }
    public decimal Salary { get; set; }
    public decimal CapHit { get; set; }
    public decimal AnnualAverage { get; set; }
}
