
using Bogus;
using PoolTools.Player.Application.Players.Queries.GetPlayerById;
using PoolTools.Player.Domain.Entities;

namespace PoolTools.Player.Application.IntegrationTests.Players.Queries;

using static Testing;
public class GetPlayerByIdTests : BaseTestFixture
{
    [Test]
    public async Task ShouldAllowAnonymousUser()
    {
        var query = new GetPlayerByIdQuery { PlayerId = 1};

        var action = () => SendAsync(query);

        await action.Should().NotThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldNotFoundInexistantPlayer()
    {
        await RunAsDefaultUserAsync();

        var playerGenerator = new Faker<Domain.Entities.Player>()
            .RuleFor(p => p.FirstName, f => f.Person.FirstName)
            .RuleFor(p => p.LastName, f => f.Person.LastName)
            .RuleFor(p => p.Position, f => f.PickRandom(new[] { "C", "LW", "RW", "G", "D" }))
            .RuleFor(p => p.Team, new Team { Code = "TST", City = "Testville", Name = "Testers" });

        await AddAsync(playerGenerator.Generate());
        
        var query = new GetPlayerByIdQuery { PlayerId = 99 };

        var playerFound = await SendAsync(query);

        playerFound.Should().BeNull();
    }

    [Test]
    public async Task ShouldReturnExistingPlayer()
    {
        await RunAsDefaultUserAsync();

        var team = new Team { Code = "TST", City = "Testville", Name = "Testers" };
        var teamId = await AddAsync(team);

        var player = new Domain.Entities.Player{ FirstName = "Test", LastName = "Player", Position = "RW", TeamId = teamId, DateOfBirth = DateTime.Now.AddYears(-20) };

        var insertedPlayerId = await AddAsync(player);

        var query = new GetPlayerByIdQuery { PlayerId = insertedPlayerId };

        var playerFound = await SendAsync(query);

        playerFound.Should().NotBeNull();
        playerFound.FirstName.Should().Be(player.FirstName);
        playerFound.LastName.Should().Be(player.LastName);
        playerFound.Position.Should().Be(player.Position);
        playerFound.Team.Should().Be(team?.Code);
        playerFound.Age.Should().Be(20);
    }
}
