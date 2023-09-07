using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Exceptions;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models.Spotify;
using SpotifyPlaylistJanitorAPI.Services;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class SpotifyPollingServiceTests : TestBase
    {
        private ISpotifyPollingService _spotifyPollingService;
        private Mock<ISpotifyService> _spotifyServiceMock;
        private Mock<IPlayingStateService> _playingStateServiceMock;
        private Mock<IServiceScopeFactory> _scopeFactoryMock;
        private Mock<ILogger<SpotifyPollingService>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _spotifyServiceMock = new Mock<ISpotifyService>();
            _playingStateServiceMock = new Mock<IPlayingStateService>();
            _scopeFactoryMock = new Mock<IServiceScopeFactory>();
            _loggerMock = new Mock<ILogger<SpotifyPollingService>>();

            _spotifyPollingService = new SpotifyPollingService(
                _spotifyServiceMock.Object,
                _playingStateServiceMock.Object,
                _scopeFactoryMock.Object,
                _loggerMock.Object
                );

            _spotifyServiceMock
                .Setup(mock => mock.IsLoggedIn)
                .Returns(true);

            _loggerMock.Invocations.Clear();
        }

        [Test]
        public void SpotifyPollingService_PollSpotifyPlayback_Logs_Not_logged_In()
        {
            //Arrange
            _spotifyServiceMock
                .Setup(mock => mock.IsLoggedIn)
                .Returns(false);

            //Act
            _spotifyPollingService.PollSpotifyPlayback(null);

            //Assert
            //_loggerMock.Verify(logger => logger.LogInformation(
            //    It.Is<string>(s => s.Equals("Not currently logged into Spotify")),
            //    It.IsAny<object>()
            //));

            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Not currently logged into Spotify" && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }


    }
}
