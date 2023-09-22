using System.Text.Json.Serialization;

namespace SpotifyPlaylistJanitorAPI.Models.Auth
{
    /// <summary>
    /// Model to hold JWT details
    /// </summary>
    public class JWTModel
    {
        /// <summary>
        /// Access Token.
        /// </summary>
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }

        /// <summary>
        /// Token type.
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Miliseconds to token expiry.
        /// </summary>
        [JsonPropertyName("expires_in")]
        public required int ExpiresIn { get; set; }

        /// <summary>
        /// Refresh Token.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public required string RefreshToken { get; set; }
    }
}
