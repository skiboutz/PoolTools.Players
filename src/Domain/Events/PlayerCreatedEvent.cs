namespace PoolTools.Player.Domain.Events;
public class PlayerCreatedEvent : BaseEvent
{
    public required int PlayerId { get; init; }
}
