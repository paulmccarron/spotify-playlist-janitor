namespace SpotifyPlaylistJanitorAPI.Models.Auth
{
    /// <summary>
    /// Model to hold user login details
    /// </summary>
    public class UserLoginRequest
    {
        /// <summary>
        /// User name.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        public required string Password { get; set; }
    }
}
