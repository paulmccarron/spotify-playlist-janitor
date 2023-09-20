using System.Text.Json.Serialization;

namespace SpotifyPlaylistJanitorAPI.Models.Auth
{
    /// <summary>
    /// Model to hold token refresh request details
    /// </summary>
    public class TokenRefreshModel
    {
        /// <summary>
        /// Refresh token.
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}
