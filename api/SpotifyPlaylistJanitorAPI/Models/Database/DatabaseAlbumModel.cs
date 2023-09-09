namespace SpotifyPlaylistJanitorAPI.Models.Database
{
    /// <summary>
    /// Model for Database album information.
    /// </summary>
    public class DatabaseAlbumModel
    {
        /// <summary>
        /// Spotify album id.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Spotify album name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Spotify album href.
        /// </summary>
        public required string Href { get; set; }
    }
}
