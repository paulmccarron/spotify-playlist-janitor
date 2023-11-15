namespace SpotifyPlaylistJanitorAPI.Models.Auth
{
    /// <summary>
    /// Model to hold user details
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// User name.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// User's Spotify name.
        /// </summary>
        public required string SpotifyUsername { get; set; }

        /// <summary>
        /// User role.
        /// </summary>
        public required string Role { get; set; }
    }
}
