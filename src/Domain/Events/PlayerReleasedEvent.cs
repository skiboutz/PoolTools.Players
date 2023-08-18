namespace PoolTools.Player.Domain.Events;
public class PlayerReleasedEvent : BaseEvent
{
    public required int PlayerId { get; set; }
}
