namespace SpotifyPlaylistJanitorAPI.Models.Auth
{
    /// <summary>
    /// Model to hold JWT details
    /// </summary>
    public class JWTModel
    {
        /// <summary>
        /// User Token.
        /// </summary>
        public required string Token { get; set; }
    }
}
