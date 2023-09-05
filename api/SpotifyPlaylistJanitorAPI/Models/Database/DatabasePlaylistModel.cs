namespace SpotifyPlaylistJanitorAPI.Models.Database
{
    /// <summary>
    /// Model for Spotify playlist information.
    /// </summary>
    public class DatabasePlaylistModel
    {
        /// <summary>
        /// Spotify playlist id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Spotify playlist name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Href link to Spotify playlist.
        /// </summary>
        public string? Href { get; set; }
    }
}
