using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.DataAccess.Models;
using SpotifyPlaylistJanitorAPI.Exceptions;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models.Spotify;
using SpotifyPlaylistJanitorAPI.Services;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using System;

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

        private Mock<SpotifyPlaylistJanitorDatabaseContext> _dbContextMock;
        private SpotifyPlayingState _playingState;

        [SetUp]
        public void Setup()
        {
            _spotifyServiceMock = new Mock<ISpotifyService>();
            _playingStateServiceMock = new Mock<IPlayingStateService>();
            _scopeFactoryMock = new Mock<IServiceScopeFactory>();
            _loggerMock = new Mock<ILogger<SpotifyPollingService>>();

            _spotifyServiceMock
                .Setup(mock => mock.IsLoggedIn)
                .Returns(true);

            _playingState = new SpotifyPlayingState
            {
                IsPlaying = true,
                Track = new SpotifyTrackModel
                {
                    Name = "Test_Name",
                    PlaylistId = "Test_PlaylistId"
                }
            };

            _spotifyServiceMock
                .Setup(mock => mock.GetCurrentPlayback())
                .ReturnsAsync(_playingState);

            _dbContextMock = new Mock<SpotifyPlaylistJanitorDatabaseContext>();
            Mock<IServiceProvider> mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(x => x.GetService(typeof(SpotifyPlaylistJanitorDatabaseContext)))
                .Returns(_dbContextMock.Object);

            Mock<IServiceScope> mockServiceScope = new Mock<IServiceScope>();
            mockServiceScope
                .Setup(mock => mock.ServiceProvider)
                .Returns(mockServiceProvider.Object);

            _scopeFactoryMock
                .Setup(mock => mock.CreateScope())
                .Returns(mockServiceScope.Object);

            _playingStateServiceMock
                .Setup(mock => mock.CheckSkipHasHappened(It.IsAny<SpotifyPlayingState>()))
                .Returns(true);

            _spotifyPollingService = new SpotifyPollingService(
                _spotifyServiceMock.Object,
                _playingStateServiceMock.Object,
                _scopeFactoryMock.Object,
                _loggerMock.Object
                );

            _loggerMock.Invocations.Clear();
        }

        [Test]
        public void SpotifyPollingService_PollSpotifyPlayback_Logs_Info_Not_Logged_In()
        {
            //Arrange
            _spotifyServiceMock
                .Setup(mock => mock.IsLoggedIn)
                .Returns(false);

            //Act
            _spotifyPollingService.PollSpotifyPlayback(null);

            //Assert
            VerifyLog(_loggerMock, LogLevel.Information, "Not currently logged into Spotify");
        }

        [Test]
        public void SpotifyPollingService_PollSpotifyPlayback_Logs_Info_Not_Currently_Listening()
        {
            //Arrange
            var currentPlayback = new SpotifyPlayingState
            {
                IsPlaying = false,
            };

            _spotifyServiceMock
                .Setup(mock => mock.GetCurrentPlayback())
                .ReturnsAsync(currentPlayback);

            //Act
            _spotifyPollingService.PollSpotifyPlayback(null);

            //Assert
            VerifyLog(_loggerMock, LogLevel.Information, "Not currently listening to a monitored playlist");
        }

        [Test]
        public void SpotifyPollingService_PollSpotifyPlayback_Logs_Debug_Currently_Listening()
        {
            //Arrange
            _playingStateServiceMock
                .Setup(mock => mock.CheckSkipHasHappened(It.IsAny<SpotifyPlayingState>()))
                .Returns(false);

            //Act
            _spotifyPollingService.PollSpotifyPlayback(null);

            //Assert
            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently listening to playlist: {_playingState.Track?.PlaylistId}");
            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently playing: {_playingState.Track?.Name}");
        }

        [Test]
        public void SpotifyPollingService_PollSpotifyPlayback_Logs_Info_Song_Was_Skipped()
        {
            //Arrange
            var dbPlaylists = Fixture.Build<SpotifyPlaylist>()
                .CreateMany()
                .ToList();
            dbPlaylists[0].Id = _playingState.Track?.PlaylistId ?? "";
            var queryAblePlaylists = dbPlaylists.AsQueryable();

            Mock<DbSet<SpotifyPlaylist>> dbSetPlaylistsMock = new Mock<DbSet<SpotifyPlaylist>>();

            dbSetPlaylistsMock.AddIQueryables(queryAblePlaylists);

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(dbSetPlaylistsMock.Object);

            //Act
            _spotifyPollingService.PollSpotifyPlayback(null);

            //Assert
            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently listening to playlist: {_playingState.Track?.PlaylistId}");
            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently playing: {_playingState.Track?.Name}");
            VerifyLog(_loggerMock, LogLevel.Information, $"Skipped track: {_playingState.Track?.Name} was playing from monitored playlist: {_playingState.Track?.PlaylistId}");
        }

        [Test]
        public void SpotifyPollingService_PollSpotifyPlayback_Logs_Info_Song_Was_Skipped_Unknown()
        {
            //Arrange
            _playingState = new SpotifyPlayingState
            {
                IsPlaying = true,
                Track = new SpotifyTrackModel
                {
                    Name = null,
                    PlaylistId = null
                }
            };

            _spotifyServiceMock
                .Setup(mock => mock.GetCurrentPlayback())
                .ReturnsAsync(_playingState);

            var dbPlaylists = Fixture.Build<SpotifyPlaylist>()
                .CreateMany()
                .ToList();
            dbPlaylists[0].Id = "";
            var queryAblePlaylists = dbPlaylists.AsQueryable();

            Mock<DbSet<SpotifyPlaylist>> dbSetPlaylistsMock = new Mock<DbSet<SpotifyPlaylist>>();

            dbSetPlaylistsMock.AddIQueryables(queryAblePlaylists);

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(dbSetPlaylistsMock.Object);

            //Act
            _spotifyPollingService.PollSpotifyPlayback(null);

            //Assert
            VerifyLog(_loggerMock, LogLevel.Debug, "Currently listening to playlist: unknown");
            VerifyLog(_loggerMock, LogLevel.Debug, "Currently playing: unknown");
            VerifyLog(_loggerMock, LogLevel.Information, "Skipped track: unknown was playing from monitored playlist: unknown");
        }
    }
}
