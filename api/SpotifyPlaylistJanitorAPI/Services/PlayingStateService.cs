using SpotifyPlaylistJanitorAPI.Models.Spotify;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Services
{
    public class PlayingStateService : IPlayingStateService
    {
        private readonly ILogger<PlayingStateService> _logger;

        public SpotifyPlayingState PlayingState { get; set; }

        public PlayingStateService(ILogger<PlayingStateService> logger)
        {
            _logger = logger;
            PlayingState = new SpotifyPlayingState
            {
                IsPlaying = false,
            };
        }

        public bool CheckSkipHasHappened(SpotifyPlayingState newPlayingState)
        {
            if (PlayingState.IsPlaying && !newPlayingState.IsPlaying)
            {
                _logger.LogDebug($"Playback has been paused");
                return false;
            }
            else if (!PlayingState.IsPlaying && newPlayingState.IsPlaying)
            {
                _logger.LogDebug($"Playback has started, song: {newPlayingState?.Track?.Name}");
                return false;
            }
            else if (PlayingState.IsPlaying && newPlayingState.IsPlaying)
            {
                if ((PlayingState.Track?.PlaylistId ?? "").Equals(newPlayingState.Track?.PlaylistId)
                && !(PlayingState.Track?.Id + PlayingState.Track?.Name ?? "")
                    .Equals(newPlayingState.Track?.Id + PlayingState.Track?.Name))
                {
                    _logger.LogDebug($"Song has changed from {PlayingState?.Track?.Name} to {newPlayingState?.Track?.Name}");
                    var previousProgress = PlayingState?.Track?.Progress;
                    if (previousProgress < 10000)
                    {
                        _logger.LogInformation($"Song: {PlayingState?.Track?.Name} was skipped.");
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
