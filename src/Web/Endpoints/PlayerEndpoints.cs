using Microsoft.AspNetCore.Http.HttpResults;
using PoolTools.Player.Application.Players.Queries.GetPlayers;
using PoolTools.Player.Application.Players.Queries.GetPlayerById;
using PoolTools.Player.Application.Players.Commands.AddPlayer;
using PoolTools.Player.Application.Players.Commands.TradePlayer;
using PoolTools.Player.Domain.Exceptions;
using PoolTools.Player.Application.Players.Commands.ReleasePlayer;
using PoolTools.Player.Application.Players.Commands.DeletePlayer;

namespace PoolTools.Player.Web.Endpoints;

public static class PlayerEndpoints
{
    public static void MapPlayerEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Player").WithTags(nameof(Player));

        group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken) => await mediator.Send(new GetPlayersQuery(), cancellationToken))
        .CacheOutput(c => c.Expire(TimeSpan.FromDays(1)))
        .WithName("GetAllPlayers")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<PlayerDto>, NotFound>> (int id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var request = new GetPlayerByIdQuery { PlayerId = id };

            var foundPlayer = await mediator.Send(request, cancellationToken);

            return foundPlayer is null ? TypedResults.NotFound() : TypedResults.Ok(foundPlayer);
        })
        .CacheOutput(c => c.Expire(TimeSpan.FromDays(1)))
        .WithName("GetPlayerById")
        .WithOpenApi();

        group.MapPut("/{id}/trade/{newTeamCode}", async Task<Results<Ok, NotFound, BadRequest<string>>> (int id, string newTeamCode, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new TradePlayerCommand { PlayerId = id, NewTeamCode = newTeamCode };

            try
            {
                var affected = await mediator.Send(command, cancellationToken);

                return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
            }
            catch (UnsupportedTradeException ex)
            {

                return TypedResults.BadRequest(ex.Message);
            }

        })
        .WithName("TradePlayer")
        .WithOpenApi();

        group.MapPut("/{id}/release", async Task<Results<Ok, NotFound>> (int id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new ReleasePlayerCommand { PlayerId = id };

            var affected = await mediator.Send(command, cancellationToken);

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("ReleasePlayer")
        .WithOpenApi();

        group.MapPost("/", async (AddPlayerDto newPlayer, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new AddPlayerCommand { NewPlayer = newPlayer };

            int playerId = await mediator.Send(command, cancellationToken);

            return TypedResults.Created($"/api/Player/{playerId}");
        })
        .WithName("CreatePlayer")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new DeletePlayerCommand { PlayerId = id };

            var affected = await mediator.Send(command, cancellationToken);

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePlayer")
        .WithOpenApi();
    }
}
