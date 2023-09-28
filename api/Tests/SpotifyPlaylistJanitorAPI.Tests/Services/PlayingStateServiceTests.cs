using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Models.Spotify;
using SpotifyPlaylistJanitorAPI.Services;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class PlayingStateServiceTests : TestBase
    {
        private IPlayingStateService _playingStateService;
        private Mock<ILogger<PlayingStateService>> _loggerMock;
        private SpotifyCurrentlyPlayingTrackModel _currentTrack;
        private SpotifyPlayingState _currentState;
        private DatabasePlaylistModel _playlistModel;

        private const string _playlistId = "playlist_id";

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<PlayingStateService>>();
            _playingStateService = new PlayingStateService(_loggerMock.Object);

            _currentTrack = Fixture.Build<SpotifyCurrentlyPlayingTrackModel>()
                .With(x => x.Id, "currently_playing_id")
                .With(x => x.Name, "current_track_name")
                .With(x => x.PlaylistId, _playlistId)
                .With(x => x.Progress, 20000)
                .Create();
            _currentState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, false)
                .With(x => x.Track, _currentTrack)
                .Create();

            _playlistModel = Fixture.Build<DatabasePlaylistModel>()
                .With(x => x.Id, _playlistId)
                .Without(x => x.SkipThreshold)
                .With(x => x.IgnoreInitialSkips, false)
                .Without(x => x.AutoCleanupLimit)
                .Create();

            _playingStateService.PlayingState = _currentState;
        }

        [Test]
        public void PlayingStateService_UpdatePlayingState_Logs_When_Playback_Paused()
        {
            //Arrange
            _currentState.IsPlaying = true;
            _playingStateService.PlayingState = _currentState;

            var newPlayingState = new SpotifyPlayingState
            {
                IsPlaying = false,
            };

            //Act
            _playingStateService.UpdatePlayingState(newPlayingState);

            //Assert
            VerifyLog(_loggerMock, LogLevel.Debug, "Playback has been paused");
        }

        [Test]
        public void PlayingStateService_UpdatePlayingState_Logs_When_Playback_Starts()
        {
            //Arrange
            _playingStateService.PlayingState = _currentState;

            var newPlayingState = new SpotifyPlayingState
            {
                IsPlaying = true,
                Track = _currentTrack
            };

            //Act
            _playingStateService.UpdatePlayingState(newPlayingState);

            //Assert
            VerifyLog(_loggerMock, LogLevel.Debug, $"Playback has started, song: {_currentTrack?.Name}");
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_False_When_Playback_Continuing_And_Playlist_Changes()
        {
            //Arrange
            var newPlaylistId = "new_playlist_id";
            _currentState.IsPlaying = true;
            _playingStateService.PlayingState = _currentState;

            var newTrack = Fixture.Build<SpotifyCurrentlyPlayingTrackModel>()
                .With(x => x.Id, "new_track_id")
                .With(x => x.Name, "new_track_name")
                .With(x => x.PlaylistId, newPlaylistId)
                .Create();
            var newPlayingState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, true)
                .With(x => x.Track, newTrack)
                .Create();

            _playlistModel.Id = newPlaylistId;

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState, _playlistModel);

            //Assert
            skip.Should().BeFalse();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_False_When_Playback_Continuing_And_Song_Changes_After_Default_Progress_Limit()
        {
            //Arrange
            _currentState.IsPlaying = true;
            _playingStateService.PlayingState = _currentState;

            var newTrack = Fixture.Build<SpotifyCurrentlyPlayingTrackModel>()
                .With(x => x.Id, "new_track_id")
                .With(x => x.Name, "new_track_name")
                .With(x => x.PlaylistId, _currentState?.Track?.PlaylistId)
                .Create();
            var newPlayingState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, true)
                .With(x => x.Track, newTrack)
                .Create();

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState, _playlistModel);

            //Assert
            skip.Should().BeFalse();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_False_When_Playback_Continuing_And_Song_Changes_After_Custom_Progress_Limit()
        {
            //Arrange
            _currentState.IsPlaying = true;
            _playingStateService.PlayingState = _currentState;

            var newTrack = Fixture.Build<SpotifyCurrentlyPlayingTrackModel>()
                .With(x => x.Id, "new_track_id")
                .With(x => x.Name, "new_track_name")
                .With(x => x.PlaylistId, _currentState?.Track?.PlaylistId)
                .Create();
            var newPlayingState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, true)
                .With(x => x.Track, newTrack)
                .Create();

            _playlistModel.SkipThreshold = 5;

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState, _playlistModel);

            //Assert
            skip.Should().BeFalse();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_True_When_Playback_Continuing_And_Song_Changes_Before_Default_Progress_Limit()
        {
            //Arrange
            _currentState.IsPlaying = true;
            _currentState.Track.Progress = 5000;
            _playingStateService.PlayingState = _currentState;

            var newTrack = Fixture.Build<SpotifyCurrentlyPlayingTrackModel>()
                .With(x => x.Id, "new_track_id")
                .With(x => x.Name, "new_track_name")
                .With(x => x.PlaylistId, _currentState?.Track?.PlaylistId)
                .Create();
            var newPlayingState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, true)
                .With(x => x.Track, newTrack)
                .Create();

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState, _playlistModel);

            //Assert
            skip.Should().BeTrue();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_True_When_Playback_Continuing_And_Song_Changes_Custom_Default_Progress_Limit()
        {
            //Arrange
            _currentState.IsPlaying = true;
            _currentState.Track.Progress = 5000;
            _playingStateService.PlayingState = _currentState;

            var newTrack = Fixture.Build<SpotifyCurrentlyPlayingTrackModel>()
                .With(x => x.Id, "new_track_id")
                .With(x => x.Name, "new_track_name")
                .With(x => x.PlaylistId, _currentState?.Track?.PlaylistId)
                .Create();
            var newPlayingState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, true)
                .With(x => x.Track, newTrack)
                .Create();

            _playlistModel.SkipThreshold = 6;

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState, _playlistModel);

            //Assert
            skip.Should().BeTrue();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_False_When_Playback_Continuing_And_Song_Changes_Before_Default_Progress_Limit_And_Ignoring_Initial_Skips()
        {
            //Arrange
            _currentState.IsPlaying = true;
            _currentState.Track.Progress = 5000;
            _playingStateService.PlayingState = _currentState;

            var newTrack = Fixture.Build<SpotifyCurrentlyPlayingTrackModel>()
                .With(x => x.Id, "new_track_id")
                .With(x => x.Name, "new_track_name")
                .With(x => x.PlaylistId, _currentState?.Track?.PlaylistId)
                .Create();
            var newPlayingState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, true)
                .With(x => x.Track, newTrack)
                .Create();

            _playlistModel.IgnoreInitialSkips = true;

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState, _playlistModel);

            //Assert
            skip.Should().BeFalse();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_False_When_Playback_Continuing_And_Song_Changes_Before_Custom_Progress_Limit_And_Ignoring_Initial_Skips()
        {
            //Arrange
            _currentState.IsPlaying = true;
            _currentState.Track.Progress = 5000;
            _playingStateService.PlayingState = _currentState;

            var newTrack = Fixture.Build<SpotifyCurrentlyPlayingTrackModel>()
                .With(x => x.Id, "new_track_id")
                .With(x => x.Name, "new_track_name")
                .With(x => x.PlaylistId, _currentState?.Track?.PlaylistId)
                .Create();
            var newPlayingState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, true)
                .With(x => x.Track, newTrack)
                .Create();

            _playlistModel.IgnoreInitialSkips = true;
            _playlistModel.SkipThreshold = 6;

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState, _playlistModel);

            //Assert
            skip.Should().BeFalse();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_True_When_Playback_Continuing_And_Song_Changes_Before_Default_Progress_Limit_And_Ignoring_Initial_Skips_After_Completed_Play()
        {
            //Arrange
            _currentState.IsPlaying = true;
            _currentState.Track.Progress = 500000;
            _playingStateService.PlayingState = _currentState;

            var newTrack = Fixture.Build<SpotifyCurrentlyPlayingTrackModel>()
                .With(x => x.Id, "new_track_id")
                .With(x => x.Name, "new_track_name")
                .With(x => x.PlaylistId, _currentState?.Track?.PlaylistId)
                .Create();
            var newPlayingState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, true)
                .With(x => x.Track, newTrack)
                .Create();

            _playlistModel.IgnoreInitialSkips = true;

            _playingStateService.CheckSkipHasHappened(newPlayingState, _playlistModel);


            _currentState.Track.Progress = 500;
            _playingStateService.PlayingState = _currentState;

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState, _playlistModel);

            //Assert
            skip.Should().BeTrue();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_True_When_Playback_Continuing_And_Song_Changes_Before_Custom_Progress_Limit_And_Ignoring_Initial_Skips_After_Completed_Play()
        {
            //Arrange
            _currentState.IsPlaying = true;
            _currentState.Track.Progress = 500000;
            _playingStateService.PlayingState = _currentState;

            var newTrack = Fixture.Build<SpotifyCurrentlyPlayingTrackModel>()
                .With(x => x.Id, "new_track_id")
                .With(x => x.Name, "new_track_name")
                .With(x => x.PlaylistId, _currentState?.Track?.PlaylistId)
                .Create();
            var newPlayingState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, true)
                .With(x => x.Track, newTrack)
                .Create();

            _playlistModel.IgnoreInitialSkips = true;
            _playlistModel.SkipThreshold = 6;

            _playingStateService.CheckSkipHasHappened(newPlayingState, _playlistModel);


            _currentState.Track.Progress = 500;
            _playingStateService.PlayingState = _currentState;

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState, _playlistModel);

            //Assert
            skip.Should().BeTrue();
        }
    }
}
