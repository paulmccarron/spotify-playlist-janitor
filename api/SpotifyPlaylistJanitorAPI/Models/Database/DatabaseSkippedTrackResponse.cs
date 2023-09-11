namespace SpotifyPlaylistJanitorAPI.Models.Database
{
    /// <summary>
    /// Model for Database skipped track information.
    /// </summary>
    public class DatabaseSkippedTrackResponse
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
        /// Spotify track name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// DateTime if when skip occurred.
        /// </summary>
        public required DateTime? SkippedDate { get; set; }

        /// <summary>
        /// Spotify artists.
        /// </summary>
        public required IEnumerable<DatabaseArtistModel> Artists { get; set; }

        /// <summary>
        /// Spotify album.
        /// </summary>
        public required DatabaseAlbumResponse Album { get; set; }


    }
}
