namespace SpotifyPlaylistJanitorAPI.Models.Auth
{
    /// <summary>
    /// Model to hold user Spotify token details
    /// </summary>
    public class UserSpotifyTokenModel
    {
        /// <summary>
        /// User name.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// User Spotify token.
        /// </summary>
        public required string SpotifyToken { get; set; }
    }
}
