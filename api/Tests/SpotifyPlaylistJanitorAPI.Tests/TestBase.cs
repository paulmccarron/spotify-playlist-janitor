using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;

namespace SpotifyPlaylistJanitorAPI.Tests
{
    public abstract class TestBase
    {
        protected IFixture Fixture { get; }

        protected TestBase()
        {
            Fixture = new Fixture();
            Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        protected void VerifyLog<T>(Mock<ILogger<T>> mockLogger, LogLevel logLevel, string message)
        {
            mockLogger.Verify(logger => logger.Log(
                It.Is<LogLevel>(level => level == logLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == message && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }
    }
}
