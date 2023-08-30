using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Http;
using SpotifyPlaylistJanitorAPI.Infrastructure;

namespace SpotifyPlaylistJanitorAPI.Services
{
    public class SpotifyService
    {
        private readonly SpotifyOption _spotifyOptions;
        private readonly ILogger<SpotifyService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private ISpotifyClient? _spotifyClient { get; set; }
        private IToken? _token { get; set; }

        public bool IsLoggedIn
        {
            get
            {
                return _spotifyClient != null;
            }
        }

        public SpotifyService(IOptions<SpotifyOption> spotifyOptions, ILogger<SpotifyService> logger, IHttpClientFactory httpClientFactory)
        {
            _spotifyOptions = spotifyOptions.Value;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task CreateClient(string code, string callbackUrl)
        {
            var connector = GetAPIConnector();
            var response = await new OAuthClient(connector).RequestToken(
              new AuthorizationCodeTokenRequest(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret, code, new Uri(callbackUrl))
            );

            var config = SpotifyClientConfig
              .CreateDefault()
              .WithHTTPClient(GetNetHttpClient())
              .WithAuthenticator(new AuthorizationCodeAuthenticator(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret, response));

            _spotifyClient = new SpotifyClient(config);
        }

        public async Task<PrivateUser> GetCurrentUser()
        {
            var thing = await _spotifyClient.UserProfile.Current();
            return await _spotifyClient.UserProfile.Current();
        }

        private IHTTPClient GetNetHttpClient()
        {
            var client = _httpClientFactory.CreateClient("Spotify");
            return new NetHttpClient(client);
        }

        private IAPIConnector GetAPIConnector()
        {
            return new APIConnector(SpotifyUrls.APIV1, null, new NewtonsoftJSONSerializer(), GetNetHttpClient(), null, null);
        }
    }
}
