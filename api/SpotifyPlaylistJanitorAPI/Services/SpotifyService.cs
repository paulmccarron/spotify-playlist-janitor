using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Http;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.Services
{
    public class SpotifyService
    {
        private readonly SpotifyOption _spotifyOptions;
        private ISpotifyClient? _spotifyClient { get; set; }

        public bool IsLoggedIn
        {
            get
            {
                return _spotifyClient != null;
            }
        }

        public SpotifyService(IOptions<SpotifyOption> spotifyOptions)
        {
            _spotifyOptions = spotifyOptions.Value;
        }

        [ExcludeFromCodeCoverage]
        public async Task<ISpotifyClient> CreateClient(string code, string callbackUrl)
        {
            var response = await new OAuthClient().RequestToken(
              new AuthorizationCodeTokenRequest(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret, code, new Uri(callbackUrl))
            );

            var config = SpotifyClientConfig
              .CreateDefault()
              .WithAuthenticator(new AuthorizationCodeAuthenticator(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret, response));

            return new SpotifyClient(config);
        }

        public void SetClient(ISpotifyClient? spotifyClient)
        {
            _spotifyClient = spotifyClient;
        }

        public async Task<PrivateUser> GetCurrentUser()
        {
            return await _spotifyClient.UserProfile.Current();
        }
    }
}
