namespace PoolTools.Player.Domain.Events;
public class PlayerCreatedEvent : BaseEvent
{
    public required Entities.Player Player { get; init; }
}
