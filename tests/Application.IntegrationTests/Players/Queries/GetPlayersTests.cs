using Bogus;
using PoolTools.Player.Application.Players.Queries.GetPlayers;
using PoolTools.Player.Domain.Entities;

namespace PoolTools.Player.Application.IntegrationTests.Players.Queries;

using static Testing;
public class GetPlayersTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnAllPlayers()
    {
        var team = new Team { Code = "TST", City = "Testville", Name = "Testers" };
        var teamId = await AddAsync(team);

        var contractGenerator = new Faker<Contract>()
            .RuleFor(c => c.ExpirationYear, f => DateTime.Now.Year + f.Random.Int(1, 8));

        var playerGenerator = new Faker<Domain.Entities.Player>()
            .RuleFor(p => p.FirstName, f => f.Person.FirstName)
            .RuleFor(p => p.LastName, f => f.Person.LastName)
            .RuleFor(p => p.Position, f => f.PickRandom(new[] { "C", "LW", "RW", "G", "D" }))
            .RuleFor(p => p.TeamId, teamId);

        var existingPlayers = playerGenerator.Generate(10);

        foreach (var player in existingPlayers)
        {
            await AddAsync(player);
        }

        var query = new GetPlayersQuery();

        var result = await SendAsync(query);

        result.Should().HaveCount(10);
        result.Any(p => p.Team == null).Should().BeFalse();
    }

    [Test]
    public async Task ShouldAllowAnonymousUser()
    {
        var query = new GetPlayersQuery();

        var action = () => SendAsync(query);

        await action.Should().NotThrowAsync<UnauthorizedAccessException>();
    }
}
