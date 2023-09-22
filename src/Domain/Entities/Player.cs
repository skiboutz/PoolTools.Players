
using PoolTools.Player.Domain.Enums;

namespace PoolTools.Player.Domain.Entities;


public class Player : BaseAuditableEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set;}
    public required DateTime DateOfBirth { get; set; }

    public int? TeamId { get; set; }
    public Team? Team { get; set; }
    public required  string Position { get; set; }

    public int? ContractId { get; set; }
    public Contract? Contract { get; set; }

    public PlayerStatus Status { get; set; }

    public void Release()
    {
        if (Team is null)
        {
            throw new UnsupportedReleaseException(Id);
        }

        Team = null;
        Contract = null;
        AddDomainEvent(new PlayerReleasedEvent { PlayerId = Id });
    }

    public void Trade(Team newTeam)
    {
        if (Team == newTeam)
        {
            throw new UnsupportedTradeException(Id, newTeam.Code);
        }

        Team = newTeam;

        AddDomainEvent(new PlayerTradedEvent { PlayerId = Id, TeamCode = newTeam.Code });
    }
}