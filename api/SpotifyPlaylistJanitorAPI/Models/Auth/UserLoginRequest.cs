namespace SpotifyPlaylistJanitorAPI.Models.Auth
{
    /// <summary>
    /// Model to hold user login details
    /// </summary>
    public class UserLoginRequest
    {
        private string _email = "";

        /// <summary>
        /// User email.
        /// </summary>
        public required string Email
        {
            get
            {
                return _email.ToLower();
            }
            set
            {
                _email = value;
            }
        }

        /// <summary>
        /// User password.
        /// </summary>
        public required string Password { get; set; }
    }
}
