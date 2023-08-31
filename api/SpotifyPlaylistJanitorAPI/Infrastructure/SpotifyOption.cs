namespace SpotifyPlaylistJanitorAPI.Infrastructure
{
    /// <summary>
    /// POCO for Spotify credential values
    /// </summary>
    public class SpotifyOption
    {
        /// <summary>
        /// Spotify Client Id
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Spotify Client Secret
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
    }
}
