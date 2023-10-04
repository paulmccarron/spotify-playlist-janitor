using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Exceptions;

namespace SpotifyPlaylistJanitorAPI.Services.Interfaces
{
    /// <summary>
    /// Service to create ISpotifyClient.
    /// Interface introduced to simplify unit testing as well as to provide flexibility for future.
    /// </summary>
    public interface ISpotifyClientService
    {
        /// <summary>
        /// Create an instance a new instance of the <see cref="SpotifyClient"/> class.
        /// using provided code and callbackUrl.
        /// </summary>
        /// <param name="code">Callback code provide by first part of the Authorization flow.</param>
        /// <param name="callbackUrl">Callback URL provide by first part of the Authorization flow.</param>
        /// <returns><see cref = "SpotifyClient" /> that is authenticated for users Spotify account.</returns>
        Task<ISpotifyClient?> CreateClient(string code, string callbackUrl);

        /// <summary>
        /// Create an instance a new instance of the <see cref="SpotifyClient"/> class.
        /// using provided code and callbackUrl.
        /// </summary>
        /// <param name="tokenResponse">Token response from valid authorization request.</param>
        /// <param name="username">Username for issued token.</param>
        /// <returns><see cref = "SpotifyClient" /> that is authenticated for users Spotify account.</returns>
        Task<ISpotifyClient?> CreateClient(AuthorizationCodeTokenResponse tokenResponse, string? username);

        /// <summary>
        /// Throws exception if there are any missing Spotify credentials from environment config.
        /// </summary>
        /// <exception cref="SpotifyArgumentException"></exception>
        void CheckSpotifyCredentials();
    }
}
