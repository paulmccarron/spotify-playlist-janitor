using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.DataAccess.Entities;
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

        private Mock<SpotifyPlaylistJanitorDatabaseContext> _dbContextMock;
        private SpotifyTrackModel _playingStateTrack;
        private SpotifyPlayingState _playingState;
        private const string TRACK_ID = "TRACK_ID";
        private const string ARTIST_ID = "ARTIST_ID";
        private const string ALBUM_ID = "ALBUM_ID";
        private const int IMAGE_ID = 123;

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

            var mockArtists = Fixture.Build<SpotifyArtistModel>()
                .With(x => x.Id, ARTIST_ID)
                .CreateMany();
            var mockAlbum = Fixture.Build<SpotifyAlbumModel>()
                .With(x => x.Id, ALBUM_ID)
                .Create();
            _playingStateTrack = Fixture.Build<SpotifyCurrentlyPlayingTrackModel>()
                .With(x => x.Id, TRACK_ID)
                .With(x => x.Name, "Test_Name")
                .With(x => x.PlaylistId, "Test_PlaylistId")
                .With(x => x.Artists, mockArtists)
                .With(x => x.Album, mockAlbum)
                .With(x => x.Progress, 20000)
                .With(x => x.IsLocal, false)
                .Create();
            _playingState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, true)
                .With(x => x.Track, _playingStateTrack)
                .Create();

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
            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .ToList();
            dbPlaylists[0].Id = _playingState.Track?.PlaylistId;
            var queryAblePlaylists = dbPlaylists.AsQueryable();

            Mock<DbSet<Playlist>> dbSetPlaylistsMock = new Mock<DbSet<Playlist>>();
            dbSetPlaylistsMock.AddIQueryables(queryAblePlaylists);
            _dbContextMock
                .Setup(mock => mock.Playlists)
                .Returns(dbSetPlaylistsMock.Object);

            Mock<DbSet<Artist>> dbSetArtistsMock = new Mock<DbSet<Artist>>();
            var dbArtists = Fixture.Build<Artist>()
                .With(x => x.Id, ARTIST_ID)
                .CreateMany()
                .AsQueryable();
            dbSetArtistsMock.AddIQueryables(dbArtists);
            _dbContextMock
                .Setup(mock => mock.Artists)
                .Returns(dbSetArtistsMock.Object);

            Mock<DbSet<Album>> dbSetAlbumsMock = new Mock<DbSet<Album>>();
            var dbAlbums = Fixture.Build<Album>()
                .With(x => x.Id, ALBUM_ID)
                .CreateMany()
                .AsQueryable();
            dbSetAlbumsMock.AddIQueryables(dbAlbums);
            _dbContextMock
                .Setup(mock => mock.Albums)
                .Returns(dbSetAlbumsMock.Object);

            Mock<DbSet<Track>> dbSetTracksMock = new Mock<DbSet<Track>>();
            var dbTracks = Fixture.Build<Track>()
                .With(x => x.Id, TRACK_ID)
                .With(x => x.Artists, new List<Artist>())
                .CreateMany()
                .AsQueryable();
            dbSetTracksMock.AddIQueryables(dbTracks);
            _dbContextMock
                .Setup(mock => mock.Tracks)
                .Returns(dbSetTracksMock.Object);

            Mock<DbSet<Image>> dbSetImagesMock = new Mock<DbSet<Image>>();
            var dbImages = Fixture.Build<Image>()
                .With(x => x.Id, 0)
                .CreateMany()
                .AsQueryable();
            dbSetImagesMock.AddIQueryables(dbImages);
            _dbContextMock
                .Setup(mock => mock.Images)
                .Returns(dbSetImagesMock.Object);

            Mock<DbSet<SkippedTrack>> dbSetSkippedTracksMock = new Mock<DbSet<SkippedTrack>>();
            dbSetSkippedTracksMock.AddIQueryables(new List<SkippedTrack>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.SkippedTracks)
                .Returns(dbSetSkippedTracksMock.Object);

            //Act
            _spotifyPollingService.PollSpotifyPlayback(null);

            //Assert
            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently listening to playlist: {_playingState.Track?.PlaylistId}");
            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently playing: {_playingState.Track?.Name}");
            VerifyLog(_loggerMock, LogLevel.Information, $"Skipped track: {_playingState.Track?.Name} was playing from monitored playlist: {_playingState.Track?.PlaylistId}");
        }

        [Test]
        public void SpotifyPollingService_PollSpotifyPlayback_Logs_Debug_Local_Song_Was_Skipped()
        {
            //Arrange
            _playingState.Track.IsLocal = true;

            _spotifyServiceMock
                .Setup(mock => mock.GetCurrentPlayback())
                .ReturnsAsync(_playingState);
            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .ToList();
            dbPlaylists[0].Id = _playingState.Track.PlaylistId;
            var queryAblePlaylists = dbPlaylists.AsQueryable();

            Mock<DbSet<Playlist>> dbSetPlaylistsMock = new Mock<DbSet<Playlist>>();
            dbSetPlaylistsMock.AddIQueryables(queryAblePlaylists);
            _dbContextMock
                .Setup(mock => mock.Playlists)
                .Returns(dbSetPlaylistsMock.Object);

            //Act
            _spotifyPollingService.PollSpotifyPlayback(null);

            //Assert
            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently listening to playlist: {_playingState.Track?.PlaylistId}");
            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently playing: {_playingState.Track?.Name}");
            VerifyLog(_loggerMock, LogLevel.Debug, $"Local track: ${_playingState.Track?.Name} was skipped while playing from monitored playlist: {_playingState.Track?.PlaylistId}");
        }
    }
}
