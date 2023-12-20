using PoolTools.Player.Application.Players.Commands.AddPlayer;
using PoolTools.Player.Application.Players.Commands.ReleasePlayer;
using PoolTools.Player.Domain.Entities;
using PoolTools.Player.Domain.Exceptions;

namespace PoolTools.Player.Application.IntegrationTests.Players.Commands;

using static Testing;

public class ReleasePlayerTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new ReleasePlayerCommand { PlayerId = 1 };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldRequireExistingPlayer()
    {
        var command = new ReleasePlayerCommand { PlayerId = 1 };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireUnreleasedPlayer()
    {
        var playerId = await AddAsync(new Domain.Entities.Player { FirstName = "Test", LastName = "Player", Position = "C" ,  DateOfBirth = DateTime.Now.AddYears(-20) });

        var command = new ReleasePlayerCommand { PlayerId = playerId };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<UnsupportedReleaseException>().WithMessage($"Unsupported release for player {playerId}. Player is already unsigned.");
    }

    [Test]
    public async Task ShouldReleasePlayer()
    {
        await AddAsync(new Team { Code = "TST", City = "Testville", Name = "Testers" });

        var playerId = await SendAsync(new AddPlayerCommand { NewPlayer = new AddPlayerDto { FirstName = "Test", LastName = "Player", Position = "C", Team = "TST" , DateOfBirth = DateTime.Now.AddYears(-20) } });

        var command = new ReleasePlayerCommand
        {
            PlayerId = playerId
        };

        await SendAsync(command);

        var player = await FindAsync<Domain.Entities.Player>(playerId);

        player.Should().NotBeNull();
        player!.Team.Should().BeNull();
        player.LastModifiedBy.Should().NotBeNull();
        player.LastModified.Should().NotBeNull();
        player.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
