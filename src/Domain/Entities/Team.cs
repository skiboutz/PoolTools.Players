﻿namespace PoolTools.Player.Domain.Entities;
public class Team : BaseAuditableEntity
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }

    public ICollection<Player> Roster { get; } = new List<Player>();
}
