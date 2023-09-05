using PoolTools.Player.Application.Common.Interfaces;
using PoolTools.Player.Application.Players.Queries.GetPlayers;

namespace PoolTools.Player.Application.Players.Queries.GetPlayerById;

public record PlayerDetailsDto : PlayerDto
{
    public int YearRemaining { get; set; }
    public int Age { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Player, PlayerDetailsDto>()
                .ForMember(p => p.Team, m => m.MapFrom(d => d.Team == null ? null : d.Team.Code))
                .ForMember(p => p.AAV, m => m.MapFrom(d => d.Contract == null ? (decimal?)null : d.Contract.AnnualAverage))
                .ForMember(p => p.Salary, m => m.MapFrom(d => d.Contract == null ? (decimal?)null : d.Contract.Salary))
                .ForMember(p => p.CapHit, m => m.MapFrom(d => d.Contract == null ? (decimal?)null : d.Contract.CapHit))
                .ForMember(p => p.Age, m => m.Ignore())
                .ForMember(p => p.YearRemaining, m => m.Ignore());
        }
    }
}
