namespace PoolTools.Player.Domain.Events;
public sealed class PlayerDeletedEvent : BaseEvent
{
    public required int PlayerId { get; init; }
}
