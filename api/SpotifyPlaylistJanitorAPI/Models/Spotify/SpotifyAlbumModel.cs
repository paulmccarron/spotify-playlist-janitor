using SpotifyAPI.Web;

namespace SpotifyPlaylistJanitorAPI.Models.Spotify
{
    /// <summary>
    /// Model for Spotify album information.
    /// </summary>
    public class SpotifyAlbumModel
    {
        /// <summary>
        /// Spotify album id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Spotify album name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Href link to Spotify album.
        /// </summary>
        public string? Href { get; set; }

        /// <summary>
        /// Spotify album images.
        /// </summary>
        public IEnumerable<SpotifyImageModel>? Images { get; set; }
    }
}
