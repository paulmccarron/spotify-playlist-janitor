using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Infrastructure;

namespace SpotifyPlaylistJanitorAPI.Services
{
    public class SpotifyService
    {
        private readonly SpotifyOption _spotifyOptions;
        private readonly ILogger<SpotifyService> _logger;
        private ISpotifyClient? _spotifyClient { get; set; }
        private IToken? _token { get; set; }

        public bool IsLoggedIn
        {
            get
            {
                return _spotifyClient != null;
            }
        }

        public SpotifyService(IOptions<SpotifyOption> spotifyOptions, ILogger<SpotifyService> logger)
        {
            _spotifyOptions = spotifyOptions.Value;
            _logger = logger;
        }

        public async Task CreateClient(string code, string callbackUrl)
        {
            _logger.LogInformation("1");
            var response = await new OAuthClient().RequestToken(
              new AuthorizationCodeTokenRequest(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret, code, new Uri(callbackUrl))
            );

            _logger.LogInformation("2");
            var config = SpotifyClientConfig
              .CreateDefault()
              .WithAuthenticator(new AuthorizationCodeAuthenticator(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret, response));

            _logger.LogInformation("3");
            _spotifyClient = new SpotifyClient(config);
        }

        public async Task<PrivateUser> GetCurrentUser()
        {
            var thing = await _spotifyClient.UserProfile.Current();
            return await _spotifyClient.UserProfile.Current();
        }
    }
}
