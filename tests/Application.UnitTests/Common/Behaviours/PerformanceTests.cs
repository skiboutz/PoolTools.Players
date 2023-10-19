

using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PoolTools.Player.Application.Common.Behaviours;
using PoolTools.Player.Application.Common.Interfaces;
using PoolTools.Player.Application.Players.Commands.AddPlayer;

namespace PoolTools.Player.Application.UnitTests.Common.Behaviours;

[Category("UT")]
public class PerformanceTests
{
    private Mock<ILogger<AddPlayerCommand>> _logger;
    private Mock<IUser> _user;
    private Mock<IIdentityService> _identityService;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<AddPlayerCommand>>();
        _user = new Mock<IUser>();
        _identityService = new Mock<IIdentityService>();
    }

    [Test]
    public async Task ShouldNotLogIfShortRunningRequest()
    {
        var requestPerformance = new PerformanceBehaviour<AddPlayerCommand, int>(_logger.Object, _user.Object,_identityService.Object);
        var command = new AddPlayerCommand { NewPlayer = new AddPlayerDto { FirstName = "Test", LastName = "Player", Position = "C", Team = "TST", DateOfBirth = DateTime.Now.AddYears(-20) } };

        await requestPerformance.Handle(command, async () => await RequestHandler(100), CancellationToken.None);

        _logger.VerifyLoggerWasCalled(Times.Never());
    }

    [Test]
    public async Task ShouldLogIfLongRunningRequest()
    {
        var requestPerformance = new PerformanceBehaviour<AddPlayerCommand, int>(_logger.Object, _user.Object, _identityService.Object);
        var command = new AddPlayerCommand { NewPlayer = new AddPlayerDto { FirstName = "Test", LastName = "Player", Position = "C", Team = "TST" , DateOfBirth = DateTime.Now.AddYears(-20) } };

        await requestPerformance.Handle(command, async () => await RequestHandler(600), CancellationToken.None);

        _logger.VerifyLoggerWasCalled(Times.Once(), LogLevel.Warning);
    }


    private async Task<int> RequestHandler(int handlerExecutionTime)
    {
        await Task.Delay(handlerExecutionTime);
        return 0;
    }
}
