namespace SpotifyPlaylistJanitorAPI.Models.Auth
{
    /// <summary>
    /// Model to hold user login details
    /// </summary>
    public class UserRegisterRequest
    {
        private string _email = "";
        private string _spotifyEmail = "";

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
        /// Users Spotify email.
        /// </summary>
        public required string SpotifyEmail
        {
            get
            {
                return _spotifyEmail.ToLower();
            }
            set
            {
                _spotifyEmail = value;
            }
        }

        /// <summary>
        /// User password.
        /// </summary>
        public required string Password { get; set; }
    }
}
