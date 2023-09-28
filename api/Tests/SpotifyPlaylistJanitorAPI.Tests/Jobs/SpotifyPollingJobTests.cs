using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Quartz;
using SpotifyPlaylistJanitorAPI.Jobs;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Models.Spotify;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class SpotifyPollingJobTests : TestBase
    {
        private SpotifyPollingJob _spotifyPollingJob;
        private Mock<IJobExecutionContext> _mockJobContext;

        private Mock<ILogger<SpotifyPollingJob>> _loggerMock;

        private SpotifyTrackModel _playingStateTrack;
        private SpotifyPlayingState _playingState;
        private const string TRACK_ID = "TRACK_ID";
        private const string ARTIST_ID = "ARTIST_ID";
        private const string ALBUM_ID = "ALBUM_ID";

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<SpotifyPollingJob>>();

            MockSpotifyService
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

            MockSpotifyService
                .Setup(mock => mock.GetCurrentPlayback())
                .ReturnsAsync(_playingState);

            MockPlayingStateService
                .Setup(mock => mock.CheckSkipHasHappened(It.IsAny<SpotifyPlayingState>(), It.IsAny<DatabasePlaylistModel>()))
                .Returns(true);

            _spotifyPollingJob = new SpotifyPollingJob(
                MockSpotifyService.Object,
                MockPlayingStateService.Object,
                MockDatabaseService.Object,
                _loggerMock.Object
                );

            _mockJobContext = new Mock<IJobExecutionContext>();

            _loggerMock.Invocations.Clear();
        }

        [Test]
        public async Task SpotifyPollingJob_Execute_Logs_Info_Not_Logged_In()
        {
            //Arrange
            MockSpotifyService
                .Setup(mock => mock.IsLoggedIn)
                .Returns(false);

            //Act
            await _spotifyPollingJob.Execute(_mockJobContext.Object);

            //Assert
            VerifyLog(_loggerMock, LogLevel.Information, "Not currently logged into Spotify");
            MockSpotifyService.Verify(mock => mock.GetCurrentPlayback(), Times.Never);
        }

        [Test]
        public async Task SpotifyPollingJob_Execute_Logs_Info_Not_Currently_Listening()
        {
            //Arrange
            var currentPlayback = new SpotifyPlayingState
            {
                IsPlaying = false,
            };

            MockSpotifyService
                .Setup(mock => mock.GetCurrentPlayback())
                .ReturnsAsync(currentPlayback);

            //Act
            await _spotifyPollingJob.Execute(_mockJobContext.Object);

            //Assert
            MockSpotifyService.Verify(mock => mock.GetCurrentPlayback(), Times.Once);
            MockDatabaseService.Verify(mock => mock.GetPlaylist(It.IsAny<string>()), Times.Never);
            VerifyLog(_loggerMock, LogLevel.Information, "Not currently listening to a monitored playlist");
        }

        [Test]
        public async Task SpotifyPollingJob_Execute_Logs_Debug_Currently_Listening()
        {
            //Arrange
            var playlist = Fixture.Build<DatabasePlaylistModel>()
                .With(x => x.Id, _playingState.Track?.PlaylistId)
                .Create();

            MockDatabaseService
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(playlist);

            MockPlayingStateService
                .Setup(mock => mock.CheckSkipHasHappened(It.IsAny<SpotifyPlayingState>(), It.IsAny<DatabasePlaylistModel>()))
                .Returns(false);

            //Act
            await _spotifyPollingJob.Execute(_mockJobContext.Object);

            //Assert
            MockSpotifyService.Verify(mock => mock.GetCurrentPlayback(), Times.Once);
            MockDatabaseService.Verify(mock => mock.GetPlaylist(It.IsAny<string>()), Times.Once);
            MockPlayingStateService.Verify(mock => mock.CheckSkipHasHappened(It.IsAny<SpotifyPlayingState>(), It.IsAny<DatabasePlaylistModel>()), Times.Once);
            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently listening to playlist: {_playingState.Track?.PlaylistId}");
            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently playing: {_playingState.Track?.Name}");
        }

        [Test]
        public async Task SpotifyPollingJob_Execute_Logs_Info_Song_Was_Skipped()
        {
            //Arrange
            var playlist = Fixture.Build<DatabasePlaylistModel>()
                .With(x => x.Id, _playingState.Track?.PlaylistId)
                .Create();

            MockDatabaseService
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(playlist);

            var image = Fixture.Build<DatabaseImageModel>().Create();

            MockDatabaseService
                .Setup(mock => mock.AddImage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(image);

            //Act
            await _spotifyPollingJob.Execute(_mockJobContext.Object);

            //Assert
            MockSpotifyService.Verify(mock => mock.GetCurrentPlayback(), Times.Once);
            MockDatabaseService.Verify(mock => mock.GetPlaylist(It.IsAny<string>()), Times.Once);
            MockPlayingStateService.Verify(mock => mock.CheckSkipHasHappened(It.IsAny<SpotifyPlayingState>(), It.IsAny<DatabasePlaylistModel>()), Times.Once);

            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently listening to playlist: {_playingState.Track?.PlaylistId}");
            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently playing: {_playingState.Track?.Name}");

            MockDatabaseService.Verify(mock => mock.AddArtist(It.IsAny<DatabaseArtistModel>()), Times.Exactly(_playingStateTrack.Artists.Count()));
            MockDatabaseService.Verify(mock => mock.AddAlbum(It.IsAny<DatabaseAlbumRequest>()), Times.Once);
            MockDatabaseService.Verify(mock => mock.AddTrack(It.IsAny<DatabaseTrackModel>()), Times.Once);
            MockDatabaseService.Verify(mock => mock.AddArtistToTrack(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(_playingStateTrack.Artists.Count()));
            MockDatabaseService.Verify(mock => mock.AddArtistToAlbum(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(_playingStateTrack.Artists.Count()));
            MockDatabaseService.Verify(mock => mock.AddImage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(_playingStateTrack.Album.Images.Count()));
            MockDatabaseService.Verify(mock => mock.AddImageToAlbum(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(_playingStateTrack.Album.Images.Count()));
            MockDatabaseService.Verify(mock => mock.AddSkippedTrack(It.IsAny<DatabaseSkippedTrackRequest>()), Times.Once);

            VerifyLog(_loggerMock, LogLevel.Information, $"Skipped track: {_playingState.Track?.Name} was playing from monitored playlist: {_playingState.Track?.PlaylistId}");
        }

        [Test]
        public async Task SpotifyPollingJob_Execute_Logs_Debug_Local_Song_Was_Skipped()
        {
            //Arrange
            _playingState.Track.IsLocal = true;

            MockSpotifyService
                .Setup(mock => mock.GetCurrentPlayback())
                .ReturnsAsync(_playingState);
            
            var playlist = Fixture.Build<DatabasePlaylistModel>()
                .With(x => x.Id, _playingState.Track?.PlaylistId)
                .Create();

            MockDatabaseService
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(playlist);

            //Act
            await _spotifyPollingJob.Execute(_mockJobContext.Object);

            //Assert
            MockSpotifyService.Verify(mock => mock.GetCurrentPlayback(), Times.Once);
            MockDatabaseService.Verify(mock => mock.GetPlaylist(It.IsAny<string>()), Times.Once);
            MockPlayingStateService.Verify(mock => mock.CheckSkipHasHappened(It.IsAny<SpotifyPlayingState>(), It.IsAny<DatabasePlaylistModel>()), Times.Once);

            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently listening to playlist: {_playingState.Track?.PlaylistId}");
            VerifyLog(_loggerMock, LogLevel.Debug, $"Currently playing: {_playingState.Track?.Name}");

            MockDatabaseService.Verify(mock => mock.AddArtist(It.IsAny<DatabaseArtistModel>()), Times.Never);
            MockDatabaseService.Verify(mock => mock.AddAlbum(It.IsAny<DatabaseAlbumRequest>()), Times.Never);
            MockDatabaseService.Verify(mock => mock.AddTrack(It.IsAny<DatabaseTrackModel>()), Times.Never);
            MockDatabaseService.Verify(mock => mock.AddArtistToTrack(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            MockDatabaseService.Verify(mock => mock.AddArtistToAlbum(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            MockDatabaseService.Verify(mock => mock.AddImage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            MockDatabaseService.Verify(mock => mock.AddImageToAlbum(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            MockDatabaseService.Verify(mock => mock.AddSkippedTrack(It.IsAny<DatabaseSkippedTrackRequest>()), Times.Never);

            VerifyLog(_loggerMock, LogLevel.Debug, $"Local track: ${_playingState.Track?.Name} was skipped while playing from monitored playlist: {_playingState.Track?.PlaylistId}");
        }
    }
}
