using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Models.Spotify;

namespace SpotifyPlaylistJanitorAPI.Services.Interfaces
{
    /// <summary>
    /// Service to keep current playback state and compare with new playback state to evaluate possible track skips.
    /// Interface introduced to simplify unit testing as well as to provide flexibility for future.
    /// </summary>
    public interface IPlayingStateService
    {
        /// <summary>
        /// Users current playback state.
        /// </summary>
        SpotifyPlayingState PlayingState { get; set; }

        /// <summary>
        /// Update current playback state.
        /// </summary>
        /// <param name="newPlayingState"></param>
        void UpdatePlayingState(SpotifyPlayingState newPlayingState);

        /// <summary>
        /// Compares current playing state with new playing state to evaluate if a skip has occured.
        /// </summary>
        /// <param name="newPlayingState"></param>
        /// <param name="playlist"></param>
        /// <returns>Returns <see cref="bool"/> based on a track changing to a new track in the same playlist before the configured 10 second cut-off.</returns>
        bool CheckSkipHasHappened(SpotifyPlayingState newPlayingState, DatabasePlaylistModel playlist);
    }
}
