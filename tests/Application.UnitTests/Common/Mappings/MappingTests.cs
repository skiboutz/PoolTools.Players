using System.Reflection;
using System.Runtime.Serialization;
using AutoMapper;
using PoolTools.Player.Application.Common.Interfaces;
using NUnit.Framework;
using PoolTools.Player.Domain.Entities;
using PoolTools.Player.Application.Players.Queries.GetPlayers;
using PoolTools.Player.Application.Players.Commands.AddPlayer;
using PoolTools.Player.Application.Players.Queries.GetPlayerById;

namespace PoolTools.Player.Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config => 
            config.AddMaps(Assembly.GetAssembly(typeof(IApplicationDbContext))));

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(Domain.Entities.Player), typeof(PlayerDto))]
    [TestCase(typeof(Domain.Entities.Player), typeof(PlayerDetailsDto))]
    [TestCase(typeof(AddPlayerDto), typeof(Domain.Entities.Player))]
    [TestCase(typeof(ContractDto), typeof(Contract))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private static object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        // TODO: Figure out an alternative approach to the now obsolete `FormatterServices.GetUninitializedObject` method.
#pragma warning disable SYSLIB0050 // Type or member is obsolete
        return FormatterServices.GetUninitializedObject(type);
#pragma warning restore SYSLIB0050 // Type or member is obsolete
    }
}
