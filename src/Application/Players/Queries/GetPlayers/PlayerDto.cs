using PoolTools.Player.Application.Common.Interfaces;

namespace PoolTools.Player.Application.Players.Queries.GetPlayers;

public record PlayerDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Team { get; set; }
    public required  string Position { get; set; }
    public decimal? Salary { get; set; }
    public decimal? CapHit { get; set; }
    public decimal? AAV { get; set; }

    public int YearRemaining { get; set; } = 0;

    public required int Age { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Player, PlayerDto>()
                .ForMember(p => p.Team, m => m.MapFrom(d => d.Team == null ? null : d.Team.Code))
                .ForMember(p => p.AAV, m => m.MapFrom(d => d.Contract == null ? (decimal?)null : d.Contract.AnnualAverage))
                .ForMember(p => p.Salary, m => m.MapFrom(d => d.Contract == null ? (decimal?)null : d.Contract.Salary))
                .ForMember(p => p.CapHit, m => m.MapFrom(d => d.Contract == null ? (decimal?)null : d.Contract.CapHit))
                .ForMember(p => p.Age, m => m.Ignore())
                .ForMember(p => p.YearRemaining, m => m.Ignore());
        }
    }
}
