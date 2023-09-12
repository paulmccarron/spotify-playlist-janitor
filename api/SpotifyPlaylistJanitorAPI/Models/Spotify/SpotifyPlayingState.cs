namespace SpotifyPlaylistJanitorAPI.Models.Spotify
{
    /// <summary>
    /// Model for Spotify playback state.
    /// </summary>
    public class SpotifyPlayingState
    {
        /// <summary>
        /// Is playback currently playing.
        /// </summary>
        public bool IsPlaying { get; set; }

        /// <summary>
        /// Currently playing track.
        /// </summary>
        public SpotifyCurrentlyPlayingTrackModel? Track { get; set; }
    }
}
