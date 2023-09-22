namespace SpotifyPlaylistJanitorAPI.Models.Auth
{
    /// <summary>
    /// Model for Database user information.
    /// </summary>
    public class UserDataModel
    {
        /// <summary>
        /// User id.
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// Hash of user password.
        /// </summary>
        public required string PasswordHash { get; set; }

        /// <summary>
        /// Is user an admin.
        /// </summary>
        public required bool IsAdmin { get; set; }

        /// <summary>
        /// Current refresh token for user.
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Current refresh token expiry date for user.
        /// </summary>
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
