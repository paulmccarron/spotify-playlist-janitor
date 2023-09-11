namespace SpotifyPlaylistJanitorAPI.Models.Database
{
    /// <summary>
    /// Model for Database skipped track information.
    /// </summary>
    public class DatabaseSkippedTrackModel
    {
        /// <summary>
        /// Spotify playlist id.
        /// </summary>
        public required string PlaylistId { get; set; }

        /// <summary>
        /// Spotify track id.
        /// </summary>
        public required string TrackId { get; set; }

        /// <summary>
        /// DateTime if when skip occurred.
        /// </summary>
        public required DateTime? SkippedDate { get; set; }
    }
}
