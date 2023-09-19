using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;
using PoolTools.Player.Domain.Events;

namespace PoolTools.Player.Application.Players.EventHandlers;
public sealed class PlayerCreatedEventHandler : INotificationHandler<PlayerCreatedEvent>
{
    private readonly IAzureClientFactory<ServiceBusClient> _serviceBusClientFactory;

    public PlayerCreatedEventHandler(IAzureClientFactory<ServiceBusClient> serviceBusClientFactory)
    {
        _serviceBusClientFactory = serviceBusClientFactory;
    }

    public async Task Handle(PlayerCreatedEvent notification, CancellationToken cancellationToken)
    {
        var client = _serviceBusClientFactory.CreateClient("PlayerClient");

        ServiceBusSender sender = client.CreateSender("players");

        var message = new ServiceBusMessage(JsonSerializer.Serialize(notification.Player))
        {
            Subject = "PlayerCreated"
        };

        await sender.SendMessageAsync(message, cancellationToken);
    }
}
