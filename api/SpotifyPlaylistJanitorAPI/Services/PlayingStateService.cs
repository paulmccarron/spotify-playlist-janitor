using SpotifyPlaylistJanitorAPI.Models.Spotify;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Services
{
    /// <summary>
    /// Service to keep current playback state and compare with new playback state to evaluate possible track skips.
    /// </summary>
    public class PlayingStateService : IPlayingStateService
    {
        private readonly ILogger<PlayingStateService> _logger;

        /// <summary>
        /// Users current playback state.
        /// </summary>
        public SpotifyPlayingState PlayingState { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayingStateService"/> class.
        /// </summary>
        /// <param name="logger">The Application Logger.</param>
        public PlayingStateService(ILogger<PlayingStateService> logger)
        {
            _logger = logger;
            PlayingState = new SpotifyPlayingState
            {
                IsPlaying = false,
            };
        }

        /// <summary>
        /// Compares current playing state with new playing state to evaluate if a skip has occured.
        /// </summary>
        /// <param name="newPlayingState"></param>
        /// <returns>Returns <see cref="bool"/> based on a track changing to a new track in the same playlist before the configured 10 second cut-off.</returns>
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
                if (PlayingState.Track?.PlaylistId == newPlayingState.Track?.PlaylistId
                    && PlayingState.Track?.Id != newPlayingState.Track?.Id)
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
