namespace SpotifyPlaylistJanitorAPI.Models.Database
{
    /// <summary>
    /// Model for Database track information.
    /// </summary>
    public class DatabaseTrackModel
    {
        /// <summary>
        /// Spotify track id.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Spotify track name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Spotify track length.
        /// </summary>
        public required int Length { get; set; }

        /// <summary>
        /// Spotify album id.
        /// </summary>
        public required string AlbumId { get; set; }
    }
}
