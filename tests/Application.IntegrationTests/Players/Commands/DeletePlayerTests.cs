﻿using PoolTools.Player.Application.Players.Commands.AddPlayer;
using PoolTools.Player.Application.Players.Commands.DeletePlayer;
using PoolTools.Player.Domain.Entities;
using PoolTools.Player.Domain.Enums;

namespace PoolTools.Player.Application.IntegrationTests.Players.Commands;

using static Testing;

public class DeletePlayerTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new DeletePlayerCommand { PlayerId = 1 };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldRequireValidPlayerId()
    {
        var command = new DeletePlayerCommand { PlayerId = 1};

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDesactivatePlayer()
    {
        var existingTeam = new Team { Code = "TST", City = "Testville", Name = "Testers" };
        await AddAsync(existingTeam);

        var playerId = await SendAsync(new AddPlayerCommand { NewPlayer = new AddPlayerDto { FirstName = "Test", LastName = "Player", Position = "C", Team = existingTeam.Code, DateOfBirth = DateTime.Now.AddYears(-20) } });

        await SendAsync(new DeletePlayerCommand { PlayerId = playerId });

        var player = await FindAsync<Domain.Entities.Player>(playerId);

        player.Should().NotBeNull();
        player!.Status.Should().Be(PlayerStatus.InActive);
    }
}
