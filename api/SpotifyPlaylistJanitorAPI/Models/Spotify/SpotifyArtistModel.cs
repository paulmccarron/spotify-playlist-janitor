using SpotifyAPI.Web;

namespace SpotifyPlaylistJanitorAPI.Models.Spotify
{
    /// <summary>
    /// Model for Spotify artist information.
    /// </summary>
    public class SpotifyArtistModel
    {
        /// <summary>
        /// Spotify artist id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Spotify artist name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Href link to Spotify artist.
        /// </summary>
        public string? Href { get; set; }
    }
}
