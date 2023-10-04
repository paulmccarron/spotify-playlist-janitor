using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Exceptions;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.Services
{
    /// <summary>
    /// Service to interact with Spotify API.
    /// </summary>
    public class SpotifyClientService : ISpotifyClientService
    {
        private readonly SpotifyOption _spotifyOptions;
        private readonly IUserService _userService;
        private readonly ILogger<SpotifyClientService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyClientService"/> class.
        /// </summary>
        /// <param name="spotifyOptions">The Spotify access credentials read from environment vars.</param>
        /// <param name="userService">Service that impliments the <see cref="IUserService"/> interface.</param>
        /// <param name="logger">The Application Logger.</param>
        public SpotifyClientService(IOptions<SpotifyOption> spotifyOptions, IUserService userService, ILogger<SpotifyClientService> logger)
        {
            _spotifyOptions = spotifyOptions.Value;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Create an instance a new instance of the <see cref="SpotifyClient"/> class.
        /// Makes an Oauth request to Spotify API using ClientId, ClientSecret,
        /// response code, and callbackUrl.
        /// </summary>
        /// <param name="code">Callback code provide by first part of the Authorization flow.</param>
        /// <param name="callbackUrl">Callback URL provide by first part of the Authorization flow.</param>
        /// <returns><see cref = "SpotifyClient" /> that is authenticated for users Spotify account.</returns>
        [ExcludeFromCodeCoverage]
        public async Task<ISpotifyClient?> CreateClient(string code, string callbackUrl)
        {
            CheckSpotifyCredentials();

            var tokenResponse = await new OAuthClient().RequestToken(
              new AuthorizationCodeTokenRequest(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret, code, new Uri(callbackUrl))
            );

            var client = await CreateSpotifyClient(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret, tokenResponse, null);

            return client;
        }

        /// <summary>
        /// Create an instance a new instance of the <see cref="SpotifyClient"/> class.
        /// Makes an Oauth request to Spotify API using ClientId, ClientSecret,
        /// response code, and callbackUrl.
        /// </summary>
        /// <param name="tokenResponse">Token response from valid authorization request.</param>
        /// <param name="username">Username for issued token.</param>
        /// <returns><see cref = "SpotifyClient" /> that is authenticated for users Spotify account.</returns>
        [ExcludeFromCodeCoverage]
        public async Task<ISpotifyClient?> CreateClient(AuthorizationCodeTokenResponse tokenResponse, string? username)
        {
            CheckSpotifyCredentials();

            var client = await CreateSpotifyClient(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret, tokenResponse, username);

            return client;
        }

        [ExcludeFromCodeCoverage]
        private async Task<ISpotifyClient?> CreateSpotifyClient(string clientId, string clientSecret, AuthorizationCodeTokenResponse tokenResponse, string? username)
        {
            try
            {
                var authenticator = new AuthorizationCodeAuthenticator(clientId, clientSecret, tokenResponse);

                authenticator.TokenRefreshed += Authenticator_TokenRefreshed;

                var config = SpotifyClientConfig
                  .CreateDefault()
                  .WithAuthenticator(authenticator);

                var client = new SpotifyClient(config);

                var currentUser = await client.UserProfile.Current();

                if (username is not null && username == currentUser.Email)
                {
                    return client;
                }
            }
            catch (Exception ex)
            {
                if(username is not null)
                {
                    SetTokenInStore(username, null).Wait();

                    _logger.LogError(
                        exception: ex,
                        message: "Failed to create Spotify Client on start-up for stored user token",
                    args: new[] {
                                new { prop = "Username", value = username }
                        });
                }
            }

            return null;
        }

        [ExcludeFromCodeCoverage]
        private async void Authenticator_TokenRefreshed(object? sender, AuthorizationCodeTokenResponse e)
        {
            _logger.LogDebug("Spotify Token Refreshed");

            var config = SpotifyClientConfig
              .CreateDefault()
              .WithToken(e.AccessToken, e.TokenType);

            var client = new SpotifyClient(config);

            var user = client.UserProfile.Current().Result;

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                await SetTokenInStore(user.Email, e);
                _logger.LogDebug("Spotify Token Stored");
            }
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

        [ExcludeFromCodeCoverage]
        private async Task SetTokenInStore(string user, AuthorizationCodeTokenResponse? tokenResponse)
        {
            var tokenString = tokenResponse is null ? null : JsonConvert.SerializeObject(tokenResponse);
            await _userService.AddUserSpotifyToken(user, tokenString);
        }
    }
}
