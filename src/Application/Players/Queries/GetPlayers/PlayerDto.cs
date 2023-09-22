using PoolTools.Player.Application.Common.Interfaces;
using PoolTools.Player.Domain.Enums;

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
    public string Status { get; set; } = PlayerStatus.Active.ToString();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Player, PlayerDto>()
                .ForMember(p => p.Team, m => m.MapFrom(d => d.Team == null ? null : d.Team.Code))
                .ForMember(p => p.AAV, m => m.MapFrom(d => d.Contract == null ? (decimal?)null : d.Contract.AnnualAverage))
                .ForMember(p => p.Salary, m => m.MapFrom(d => d.Contract == null ? (decimal?)null : d.Contract.Salary))
                .ForMember(p => p.CapHit, m => m.MapFrom(d => d.Contract == null ? (decimal?)null : d.Contract.CapHit))
                .ForMember(p => p.Status, m => m.MapFrom(d => d.Status.ToString()));
        }
    }
}
