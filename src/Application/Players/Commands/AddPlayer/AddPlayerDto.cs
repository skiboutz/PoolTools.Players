using PoolTools.Player.Domain.Entities;

namespace PoolTools.Player.Application.Players.Commands.AddPlayer;

public class AddPlayerDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Team { get; set; }
    public required string Position { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public ContractDto Contract { get; set; } = default!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<AddPlayerDto, Domain.Entities.Player>()
                .ForMember(p => p.Id, m => m.Ignore())
                .ForMember(p => p.Team, m => m.Ignore())
                .ForMember(p => p.TeamId, m => m.Ignore())
                .ForMember(p => p.ContractId, m => m.Ignore())
                .ForMember(p => p.DomainEvents,m => m.Ignore())
                .ForMember(p => p.Created, m => m.Ignore())
                .ForMember(p => p.CreatedBy, m => m.Ignore())
                .ForMember(p => p.LastModified, m => m.Ignore())
                .ForMember(p => p.LastModifiedBy, m => m.Ignore());

            CreateMap<ContractDto, Contract>()
                .ForMember(p => p.Id, m => m.Ignore())
                .ForMember( c => c.Player, m => m.Ignore())
                .ForMember(p => p.DomainEvents, m => m.Ignore())
                .ForMember(p => p.Created, m => m.Ignore())
                .ForMember(p => p.CreatedBy, m => m.Ignore())
                .ForMember(p => p.LastModified, m => m.Ignore())
                .ForMember(p => p.LastModifiedBy, m => m.Ignore()); ;
        }
    }
}