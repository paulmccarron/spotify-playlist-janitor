using SpotifyAPI.Web;

namespace SpotifyPlaylistJanitorAPI.Models.Spotify
{
    /// <summary>
    /// Model for Spotify image information.
    /// </summary>
    public class SpotifyImageModel
    {
        /// <summary>
        /// Spotify image height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Spotify image width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Spotify image url.
        /// </summary>
        public string? Url { get; set; }
    }
}
