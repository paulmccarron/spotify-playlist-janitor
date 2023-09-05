using SpotifyAPI.Web;

namespace SpotifyPlaylistJanitorAPI.Models.Spotify
{
    /// <summary>
    /// Model for Spotify user information
    /// </summary>
    public class SpotifyUserModel
    {
        /// <summary>
        /// Spotify user Id
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Spotify user display name
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Spotify user email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Href link to Spotify user page
        /// </summary>
        public string? Href { get; set; }
    }
}
