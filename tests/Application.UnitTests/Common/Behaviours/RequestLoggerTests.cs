using PoolTools.Player.Application.Common.Behaviours;
using PoolTools.Player.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PoolTools.Player.Application.Players.Commands.AddPlayer;

namespace PoolTools.Player.Application.UnitTests.Common.Behaviours;

[Category("UT")]
public class RequestLoggerTests
{
    private Mock<ILogger<AddPlayerCommand>> _logger = null!;
    private Mock<IUser> _user = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<AddPlayerCommand>>();
        _user = new Mock<IUser>();
        _identityService = new Mock<IIdentityService>();
    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        _user.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());

        var requestLogger = new LoggingBehaviour<AddPlayerCommand>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(new AddPlayerCommand { NewPlayer = new AddPlayerDto { FirstName = "Test", LastName = "Player", Position = "C", Team = "TST", DateOfBirth = DateTime.Now.AddYears(-20) } }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        var requestLogger = new LoggingBehaviour<AddPlayerCommand>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(new AddPlayerCommand { NewPlayer = new AddPlayerDto { FirstName = "Test", LastName = "Player", Position = "C", Team = "TST", DateOfBirth = DateTime.Now.AddYears(-20) } }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
    }
}
