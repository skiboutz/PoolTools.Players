using PoolTools.Player.Application.Common.Exceptions;
using PoolTools.Player.Application.Players.Commands.AddPlayer;
using PoolTools.Player.Domain.Entities;

namespace PoolTools.Player.Application.IntegrationTests.Players.Commands;

using static Testing;

public class CreatePlayerTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new AddPlayerCommand { NewPlayer = new AddPlayerDto { FirstName = "Test", LastName = "Player", Position = "C", Team = "TST", DateOfBirth = DateTime.Now.AddYears(-20) } };

        var action = () => SendAsync(command);

        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldCreatePlayer()
    {
        var userId = await RunAsDefaultUserAsync();

        var existingTeam = new Team { Code = "TST", City = "Testville", Name = "Testers" };
        await AddAsync(existingTeam);

        var command = new AddPlayerCommand
        {
            NewPlayer = new AddPlayerDto
            { 
                FirstName = "Test",
                LastName = "Player",
                Position = "C",
                Team = existingTeam.Code,
                DateOfBirth = DateTime.Now.AddYears(-20)
            }
        };

        var playerId = await SendAsync(command);

        var player = await FindAsync<Domain.Entities.Player>(playerId);

        player.Should().NotBeNull();
        player!.Id.Should().BeGreaterThan(0);
        player.FirstName.Should().Be(player.FirstName);
        player.LastName.Should().Be(player.LastName);
        player.Position.Should().Be(player.Position);
        player.Team?.Code.Should().Be(existingTeam.Code);
        player.CreatedBy.Should().Be(userId);
        player.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        player.LastModifiedBy.Should().Be(userId);
        player.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
