using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Exceptions;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.Services
{
    /// <summary>
    /// Service to interact with Spotify API.
    /// </summary>
    public class SpotifyService : ISpotifyService
    {
        private readonly SpotifyOption _spotifyOptions;
        private ISpotifyClient? _spotifyClient { get; set; }

        /// <summary>
        /// Sets the internal Spotify Client fro the service.
        /// </summary>
        /// <param name="spotifyClient">The Spotify access credentials read from environment vars.</param>
        public void SetClient(ISpotifyClient? spotifyClient)
        {
            _spotifyClient = spotifyClient;
        }

        /// <summary>
        /// Returns true if service has a Spotify Client configured.
        /// </summary>
        public bool IsLoggedIn
        {
            get
            {
                return _spotifyClient != null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyOption"/> class.
        /// </summary>
        /// <param name="spotifyOptions"></param>
        public SpotifyService(IOptions<SpotifyOption> spotifyOptions)
        {
            _spotifyOptions = spotifyOptions.Value;
        }

        /// <summary>
        /// Create an instance a new instance of the <see cref="SpotifyClient"/> class.
        /// Makes an Oauth request to Spotify API using provided ClientId and ClientSecret
        /// </summary>
        /// <param name="code">Callback code provide by first part of the Authorization flow.</param>
        /// <param name="callbackUrl">Callback URL provide by first part of the Authorization flow.</param>
        /// <returns cref="SpotifyClient">Client that is authenticated for users Spotify account.</returns>
        [ExcludeFromCodeCoverage]
        public async Task<ISpotifyClient> CreateClient(string code, string callbackUrl)
        {
            CheckSpotifyCredentials();

            var response = await new OAuthClient().RequestToken(
              new AuthorizationCodeTokenRequest(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret, code, new Uri(callbackUrl))
            );

            var config = SpotifyClientConfig
              .CreateDefault()
              .WithAuthenticator(new AuthorizationCodeAuthenticator(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret, response));

            return new SpotifyClient(config);
        }

        /// <summary>
        /// Returns current users details.
        /// </summary>
        /// <returns cref="SpotifyUserModel">User details.</returns>
        /// <exception cref="SpotifyArgumentException">Thrown if service has no instance of <see cref="SpotifyClient"/> class.</exception>
        public async Task<SpotifyUserModel> GetUserDetails()
        {
            if (_spotifyClient is null)
            {
                throw new SpotifyArgumentException("No Spotify Client configured");
            }
            
            var currentUser =  await _spotifyClient.UserProfile.Current();

            return new SpotifyUserModel
            {
                Id = currentUser.Id,
                DisplayName = currentUser.DisplayName,
                Email = currentUser.Email,
                Href    = currentUser.Href,
            };
        }

        /// <summary>
        /// Returns current users playlists.
        /// </summary>
        /// <returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "SpotifyPlaylistModel" />.</returns>
        /// <exception cref="SpotifyArgumentException">Thrown if service has no instance of <see cref="SpotifyClient"/> class.</exception>
        public async Task<IEnumerable<SpotifyPlaylistModel>> GetUserPlaylists()
        {
            if (_spotifyClient is null)
            {
                throw new SpotifyArgumentException("No Spotify Client configured");
            }

            var page = await _spotifyClient.Playlists.CurrentUsers();
            var allPages = await _spotifyClient.PaginateAll(page);
            var playlists = allPages
                .Select(playlist => new SpotifyPlaylistModel
                {
                    Id = playlist.Id,
                    Name = playlist.Name,
                    Href = playlist.Href,
                    Images = playlist.Images.Select(image => new SpotifyImageModel {
                        Height = image.Height,
                        Width = image.Width,
                        Url = image.Url,
                    })
                })
                .OrderBy(playlist => playlist?.Name?.ToLower());

            return playlists;
        }

        /// <summary>
        /// Throws exception if there are any missing Spotify credentials from environment config.
        /// </summary>
        /// <exception cref="SpotifyArgumentException"></exception>
        public void CheckSpotifyCredentials()
        {
            var clientIdEmpty = string.IsNullOrWhiteSpace(_spotifyOptions.ClientId);
            var clientSecretEmpty = string.IsNullOrWhiteSpace(_spotifyOptions.ClientSecret);
            if (clientIdEmpty && clientSecretEmpty){
                throw new SpotifyArgumentException("No Spotify ClientId or ClientSecret configured");
            }
            if (clientIdEmpty)
            {
                throw new SpotifyArgumentException("No Spotify ClientId configured");
            }
            if (clientSecretEmpty)
            {
                throw new SpotifyArgumentException("No Spotify ClientSecret configured");
            }
        }
    }
}
