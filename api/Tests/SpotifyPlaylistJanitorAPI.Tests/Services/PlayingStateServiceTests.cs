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
            _currentState = GetCurrentPlayingState();
            _playingStateService.PlayingState = _currentState;

            var newPlayingState = new SpotifyPlayingState
            {
                IsPlaying = true,
                Track = new SpotifyTrackModel
                {
                    Id = "new_track_id",
                    Name = "new_track_name",
                    PlaylistId = "new_playlist_id",
                },
            };

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState);

            //Assert
            skip.Should().BeFalse();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_False_When_Playback_Continuing_And_Song_Changes_After_Progress_Limit()
        {
            //Arrange
            _currentState = GetCurrentPlayingState();
            _playingStateService.PlayingState = _currentState;

            var newPlayingState = new SpotifyPlayingState
            {
                IsPlaying = true,
                Track = new SpotifyTrackModel
                {
                    Id = "new_track_id",
                    Name = "new_track_name",
                    PlaylistId = _currentState?.Track?.PlaylistId,
                },
            };

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState);

            //Assert
            skip.Should().BeFalse();
        }

        [Test]
        public void PlayingStateService_CheckSkipHasHappened_Returns_True_When_Playback_Continuing_And_Song_Changes_Before_Progress_Limit()
        {
            //Arrange
            _currentState = GetCurrentPlayingState(5000);
            _playingStateService.PlayingState = _currentState;

            var newPlayingState = new SpotifyPlayingState
            {
                IsPlaying = true,
                Track = new SpotifyTrackModel
                {
                    Id = "new_track_id",
                    Name = "new_track_name",
                    PlaylistId = _currentState?.Track?.PlaylistId,
                },
            };

            //Act
            var skip = _playingStateService.CheckSkipHasHappened(newPlayingState);

            //Assert
            skip.Should().BeTrue();
        }

        private static SpotifyPlayingState GetCurrentPlayingState(int progress = 20000)
        {
            return new SpotifyPlayingState
            {
                IsPlaying = true,
                Track = new SpotifyTrackModel
                {
                    Id = "currently_playing_id",
                    Name = "current_track_name",
                    PlaylistId = "current-playlist_id",
                    Progress = progress,
                }
            };
        }
    }
}
