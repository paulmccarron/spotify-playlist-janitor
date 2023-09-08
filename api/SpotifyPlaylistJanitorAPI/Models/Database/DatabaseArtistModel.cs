namespace SpotifyPlaylistJanitorAPI.Models.Database
{
    /// <summary>
    /// Model for Database artist information.
    /// </summary>
    public class DatabaseArtistModel
    {
        /// <summary>
        /// Spotify artist id.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Spotify artist name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Spotify artist href.
        /// </summary>
        public required string Href { get; set; }
    }
}
