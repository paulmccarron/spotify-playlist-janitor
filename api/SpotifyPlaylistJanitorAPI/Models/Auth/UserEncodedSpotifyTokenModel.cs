namespace SpotifyPlaylistJanitorAPI.Models.Auth
{
    /// <summary>
    /// Model to hold user Spotify token details
    /// </summary>
    public class UserEncodedSpotifyTokenModel
    {
        /// <summary>
        /// User name.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// User Spotify token.
        /// </summary>
        public required string EncodedSpotifyToken { get; set; }
    }
}
