namespace SpotifyPlaylistJanitorAPI.Models.Spotify
{
    /// <summary>
    /// Model for Spotify Track.
    /// </summary>
    public class SpotifyCurrentlyPlayingTrackModel : SpotifyTrackModel
    {
        /// <summary>
        /// Spotify playlist id.
        /// </summary>
        public required string PlaylistId { get; set; }

        /// <summary>
        /// Length of the song in milliseconds.
        /// </summary>
        public required int Duration { get; set; }

        /// <summary>
        /// Playback progress of the song in milliseconds.
        /// </summary>
        public required int Progress { get; set; }

        /// <summary>
        /// Device track is being played on.
        /// </summary>
        public string? ListeningOn { get; set; }
    }
}
