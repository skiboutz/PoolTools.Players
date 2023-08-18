using PoolTools.Player.Application.Common.Exceptions;
using PoolTools.Player.Application.Players.Commands.AddPlayer;
using PoolTools.Player.Application.Players.Commands.ReleasePlayer;
using PoolTools.Player.Application.Players.Commands.TradePlayer;
using PoolTools.Player.Domain.Entities;
using PoolTools.Player.Domain.Exceptions;

namespace PoolTools.Player.Application.IntegrationTests.Players.Commands;

using static Testing;

public class TradePlayerTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new TradePlayerCommand { PlayerId = 1, NewTeamCode = "TST" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldRequireNewTeam()
    {
        await RunAsDefaultUserAsync();
        
        await AddAsync(new Team { Code = "TST", City = "Testville", Name = "Testers" });
        var playerId = await SendAsync(new AddPlayerCommand { NewPlayer = new AddPlayerDto { FirstName = "Test", LastName = "Player", Position = "C", Team = "TST", DateOfBirth = DateTime.Now.AddYears(-20) } });

        var command = new TradePlayerCommand { PlayerId = playerId, NewTeamCode = "TST" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireExistingTeam()
    {
        await RunAsDefaultUserAsync();
        await AddAsync(new Team { Code = "TST", City = "Testville", Name = "Testers" });
        var playerId = await SendAsync(new AddPlayerCommand { NewPlayer = new AddPlayerDto { FirstName = "Test", LastName = "Player", Position = "C", Team = "TST", DateOfBirth = DateTime.Now.AddYears(-20) } });

        var command = new TradePlayerCommand { PlayerId = playerId, NewTeamCode = "XXX" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireExistingPlayer()
    {
        await RunAsDefaultUserAsync();
        var command = new TradePlayerCommand { PlayerId = 1, NewTeamCode = "NEW" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldTradePlayer()
    {
        var userId = await RunAsDefaultUserAsync();

        await AddAsync(new Team { Code = "TST", City = "Testville", Name = "Testers" });
        await AddAsync(new Team { Code = "NEW", City = "Newville", Name = "Testers" });

        var playerId = await SendAsync(new AddPlayerCommand { NewPlayer = new AddPlayerDto { FirstName = "Test", LastName = "Player", Position = "C", Team = "TST", DateOfBirth = DateTime.Now.AddYears(-20) } });

        var command = new TradePlayerCommand
        {
            PlayerId = playerId,
            NewTeamCode = "NEW"
        };

        await SendAsync(command);

        var player = await FindAsync<Domain.Entities.Player>(playerId);

        player.Should().NotBeNull();
        player!.Team.Code.Should().Be("NEW");
        player.LastModifiedBy.Should().NotBeNull();
        player.LastModifiedBy.Should().Be(userId);
        player.LastModified.Should().NotBeNull();
        player.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
