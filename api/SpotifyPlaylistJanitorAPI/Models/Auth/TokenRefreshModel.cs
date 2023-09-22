namespace SpotifyPlaylistJanitorAPI.Models.Auth
{
    /// <summary>
    /// Model for token refresh request.
    /// </summary>
    public class TokenRefreshModel
    {
        /// <summary>
        /// Refresh token.
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}
