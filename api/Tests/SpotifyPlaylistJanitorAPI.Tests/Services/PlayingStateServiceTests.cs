using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
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
        private SpotifyTrackModel _currentTrack;
        private SpotifyPlayingState _currentState;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<PlayingStateService>>();
            _playingStateService = new PlayingStateService(_loggerMock.Object);

            _currentState = new SpotifyPlayingState
            {
                IsPlaying = false,
            };

            _currentTrack = Fixture.Build<SpotifyTrackModel>()
                .With(x => x.Id, "currently_playing_id")
                .With(x => x.Name, "current_track_name")
                .With(x => x.PlaylistId, "playlist_id")
                .With(x => x.Progress, 20000)
                .Create();
            _currentState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, false)
                .With(x => x.Track, _currentTrack)
                .Create();

            _playingStateService.PlayingState = _currentState;
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_False_When_No_Playback()
        {
            //Arrange
            var newPlayingState = new SpotifyPlayingState
            {
                IsPlaying = false,
            };

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState);

            //Assert
            skip.Should().BeFalse();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_False_When_Playback_Begins()
        {
            //Arrange
            var newPlayingState = new SpotifyPlayingState
            {
                IsPlaying = true,
            };

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState);

            //Assert
            skip.Should().BeFalse();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_False_When_Playback_Stops()
        {
            //Arrange
            _currentState.IsPlaying = true;
            _playingStateService.PlayingState = _currentState;

            var newPlayingState = new SpotifyPlayingState
            {
                IsPlaying = false,
            };

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState);

            //Assert
            skip.Should().BeFalse();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_False_When_Playback_Continuing_And_Playlist_Changes()
        {
            //Arrange
            _playingStateService.PlayingState = _currentState;

            var newTrack = Fixture.Build<SpotifyTrackModel>()
                .With(x => x.Id, "new_track_id")
                .With(x => x.Name, "new_track_name")
                .With(x => x.PlaylistId, "new_playlist_id")
                .Create();
            var newPlayingState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, true)
                .With(x => x.Track, newTrack)
                .Create();

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState);

            //Assert
            skip.Should().BeFalse();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_False_When_Playback_Continuing_And_Song_Changes_After_Progress_Limit()
        {
            //Arrange
            _currentState.IsPlaying = true;
            _playingStateService.PlayingState = _currentState;

            var newTrack = Fixture.Build<SpotifyTrackModel>()
                .With(x => x.Id, "new_track_id")
                .With(x => x.Name, "new_track_name")
                .With(x => x.PlaylistId, _currentState?.Track?.PlaylistId)
                .Create();
            var newPlayingState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, true)
                .With(x => x.Track, newTrack)
                .Create();

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState);

            //Assert
            skip.Should().BeFalse();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_True_When_Playback_Continuing_And_Song_Changes_Before_Progress_Limit()
        {
            //Arrange
            _currentState.IsPlaying = true;
            _currentState.Track.Progress = 5000;
            _playingStateService.PlayingState = _currentState;

            var newTrack = Fixture.Build<SpotifyTrackModel>()
                .With(x => x.Id, "new_track_id")
                .With(x => x.Name, "new_track_name")
                .With(x => x.PlaylistId, _currentState?.Track?.PlaylistId)
                .Create();
            var newPlayingState = Fixture.Build<SpotifyPlayingState>()
                .With(x => x.IsPlaying, true)
                .With(x => x.Track, newTrack)
                .Create();

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState);

            //Assert
            skip.Should().BeTrue();
        }
    }
}
