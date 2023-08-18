using Microsoft.Extensions.Logging;
using Moq;

namespace PoolTools.Player.Application.UnitTests;
public static class TestingExtensions
{
    public static Mock<ILogger<T>> VerifyLoggerWasCalled<T>(this Mock<ILogger<T>> logger, Times timeCalled, LogLevel? logLevel = null, string? expectedMessage = null)
    {
        Func<object, Type, bool> state = (v, t) => v.ToString().CompareTo(expectedMessage) == 0;

        logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => logLevel == null || l == logLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => expectedMessage == null || state(v, t)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),timeCalled);

        return logger;
    }
}
