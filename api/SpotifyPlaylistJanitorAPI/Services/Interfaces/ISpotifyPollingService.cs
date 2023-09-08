namespace SpotifyPlaylistJanitorAPI.Services.Interfaces
{
    /// <summary>
    /// Hosted Service that polls users Spotify playback activity to monitor when skips occur.
    /// Interface introduced to simplify unit testing as well as to provide flexibility for future.
    /// </summary>
    public interface ISpotifyPollingService
    {
        /// <summary>
        /// Poll users current Spotify playback activity.
        /// </summary>
        /// <param name="state">Optional state paramter</param>
        void PollSpotifyPlayback(object? state);
    }
}
