using SpotifyAPI.Web;

namespace SpotifyPlaylistJanitorAPI.Models.Spotify
{
    /// <summary>
    /// Model for Spotify playlist information.
    /// </summary>
    public class SpotifyPlaylistModel
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

        /// <summary>
        /// Spotify playlist images.
        /// </summary>
        public IEnumerable<SpotifyImageModel>? Images { get; set; }
    }
}
