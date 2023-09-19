using System.Text.Json.Serialization;

namespace PoolTools.Player.Domain.Entities;
public class Contract : BaseAuditableEntity
{
    [JsonIgnore]
    public Player Player { get; set; } = default!;

    public required int ExpirationYear { get; set; }
    public required decimal Salary { get; set; }
    public required decimal CapHit { get; set; }
    public required decimal AnnualAverage { get; set; }
}
