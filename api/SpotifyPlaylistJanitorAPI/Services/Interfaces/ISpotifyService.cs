using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Models;
using System;

namespace SpotifyPlaylistJanitorAPI.Services.Interfaces
{
    /// <summary>
    /// Introduced to simplify unit testing as well as to provide flexibility for future.
    /// </summary>
    public interface ISpotifyService
    {
        /// <summary>
        /// Sets the internal Spotify Client fro the service.
        /// </summary>
        /// <param name="spotifyClient"></param>
        void SetClient(ISpotifyClient? spotifyClient);

        /// <summary>
        /// Returns true if service has a Spotify Client configured.
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        /// Create an instance a new instance of the <see cref="SpotifyClient"/> class
        /// using provided code and callbackUrl.
        /// </summary>
        /// <param name="code">Callback code provide by first part of the Authorization flow.</param>
        /// <param name="callbackUrl">Callback URL provide by first part of the Authorization flow.</param>
        /// <returns cref="SpotifyClient">Client that is authenticated for users Spotify account.</returns>
        Task<ISpotifyClient> CreateClient(string code, string callbackUrl);

        /// <summary>
        /// Returns current users details
        /// </summary>
        /// <returns cref="SpotifyUserModel">User details</returns>
        Task<SpotifyUserModel> GetUserDetails();

        /// <summary>
        /// Returns current users playlists
        /// </summary>
        ///<returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "SpotifyPlaylistModel" />.</returns>
        Task<IEnumerable<SpotifyPlaylistModel>> GetUserPlaylists();

        /// <summary>
        /// Check for Spotify credentials
        /// </summary>
        void CheckSpotifyCredentials();
    }
}
