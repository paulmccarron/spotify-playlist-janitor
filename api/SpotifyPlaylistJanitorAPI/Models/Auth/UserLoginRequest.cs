namespace SpotifyPlaylistJanitorAPI.Models.Auth
{
    /// <summary>
    /// Model to hold user login details
    /// </summary>
    public class UserLoginRequest
    {
        /// <summary>
        /// User email.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        public required string Password { get; set; }
    }
}
