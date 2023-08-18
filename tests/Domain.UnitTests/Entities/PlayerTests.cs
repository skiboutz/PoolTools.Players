using FluentAssertions;
using NUnit.Framework;
using PoolTools.Player.Domain.Entities;
using PoolTools.Player.Domain.Events;
using PoolTools.Player.Domain.Exceptions;

namespace PoolTools.Player.Domain.UnitTests.Entities;

public class PlayerTests
{
    [Test]
    public void ShouldTradePlayer()
    {
        var player = new Domain.Entities.Player
        {
            FirstName = "Test",
            LastName = "Player",
            Position = "C",
            Team = new Team { Code = "TST", City = "Testville", Name = "Testers"},
            DateOfBirth = DateTime.Now.AddYears(-20)
        };

        player.Trade(new Team { Code = "NEW", City = "Newville", Name = "Newers" });

        player.Team.Code.Should().Be("NEW");
        player.DomainEvents.Should().HaveCount(1);
        player.DomainEvents.First().Should().BeOfType<PlayerTradedEvent>();
    }

    [Test]
    public void ShouldThrowWhenTradedToSameTeam()
    {
        Team team = new Team { Code = "TST", City = "Testville", Name = "Testers" };

        var player = new Domain.Entities.Player
        {
            Id = 1,
            FirstName = "Test",
            LastName = "Player",
            Position = "C",
            Team = team,
            DateOfBirth = DateTime.Now.AddYears(-20)
        };

        FluentActions.Invoking(() => player.Trade(team)).Should().Throw<UnsupportedTradeException>().WithMessage($"Unsupported Trade for player {player.Id}. Player already on team { player.Team.Code }.");

        
    }

    [Test]
    public void ShouldReleasePlayer()
    {
        var player = new Domain.Entities.Player
        {
            FirstName = "Test",
            LastName = "Player",
            Position = "C",
            Team = new Team { Code = "TST", City = "Testville", Name = "Testers" },
            DateOfBirth = DateTime.Now.AddYears(-20)
        };

        player.Release();

        player.Team.Should().BeNull();
        player.DomainEvents.Should().HaveCount(1);
        player.DomainEvents.First().Should().BeOfType<PlayerReleasedEvent>();
    }

    [Test]
    public void ShouldThrowWhenReleasingReleasedPlayer()
    {
        var player = new Domain.Entities.Player
        {
            Id = 1,
            FirstName = "Test",
            LastName = "Player",
            Position = "C",
            DateOfBirth = DateTime.Now.AddYears(-20)
        };

        FluentActions.Invoking(() => player.Release()).Should().Throw<UnsupportedReleaseException>().WithMessage($"Unsupported release for player {player.Id}. Player is already unsigned.");
    }
}
