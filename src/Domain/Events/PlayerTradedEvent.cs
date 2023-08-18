namespace PoolTools.Player.Domain.Events;
public class PlayerTradedEvent : BaseEvent
{
    public required int PlayerId { get; init; }
    public required string TeamCode { get; init; }

}
