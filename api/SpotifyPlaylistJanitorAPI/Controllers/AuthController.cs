using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.Controllers
{
    /// <summary>
    /// Controller for authorising application with Spotify API to make calls on behalf of the user.
    /// </summary>
    public class AuthController : BaseController
    {
        private readonly ISpotifyService _spotifyService;
        private readonly IAuthService _authService;
        private readonly SpotifyOption _spotifyOptions;
        private static string STATE = $"{Guid.NewGuid()}{Guid.NewGuid()}".Replace("-", "");

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="spotifyService">The Spotify Service.</param>
        /// <param name="authService">The Authentication Service.</param>
        /// <param name="spotifyOptions">The Spotify access credentials read from environment vars.</param>
        public AuthController(ISpotifyService spotifyService, IAuthService authService, IOptions<SpotifyOption> spotifyOptions)
        {
            _spotifyService = spotifyService;
            _authService = authService;
            _spotifyOptions = spotifyOptions.Value;
        }

        /// <summary>
        /// Default view for AuthController.
        /// </summary>
        /// <returns>Default view.</returns>
        [ExcludeFromCodeCoverage]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Auth view for AuthController.
        /// </summary>
        /// <returns>Auth view.</returns>
        [ExcludeFromCodeCoverage]
        [HttpGet()]
        public IActionResult Auth()
        {
            _spotifyService.CheckSpotifyCredentials();
            var baseUrl = Request?.GetTypedHeaders()?.Referer?.ToString() ?? "";
            var loginRequest = new LoginRequest(new Uri($"{baseUrl.TrimEnd('/')}/auth/callback"), _spotifyOptions.ClientId, LoginRequest.ResponseType.Code)
            {
                Scope = new[] {
                    Scopes.UserReadEmail,
                    Scopes.PlaylistReadPrivate,
                    Scopes.PlaylistReadCollaborative,
                    Scopes.PlaylistModifyPublic,
                    Scopes.PlaylistModifyPrivate,
                    Scopes.UserReadPlaybackState,
                    Scopes.UserReadCurrentlyPlaying,
                },
                State = STATE,
            };
            var uri = loginRequest.ToUri();
            return Redirect(uri.ToString());
        }

        /// <summary>
        /// Callback view for AuthController.
        /// </summary>
        /// <param name="code">Code returned from first part of Authorization flow, used to request Oauth token.</param>
        /// <param name="state">Additional state parameter to vaidate callback information from first part of Authorization flow.</param>
        /// <returns>Callback view.</returns>
        [ExcludeFromCodeCoverage]
        [HttpGet()]
        public async Task<IActionResult> Callback(string code, string state)
        {
            if (!state.Equals(STATE))
            {
                return Error();
            }

            var baseUrl = Request?.GetTypedHeaders()?.Referer?.ToString() ?? "";

            var spotifyClient = await _spotifyService.CreateClient(code, $"{baseUrl.TrimEnd('/')}/auth/callback");
            
            ////check attempted login against database users
            //var userProfile = await spotifyClient.UserProfile.Current();
            //var userEmail = userProfile.Email;

            _spotifyService.SetClient(spotifyClient);

            return Redirect("~/");
        }

        /// <summary>
        /// Register a new user with the application.
        /// </summary>
        /// <param name="newUser">New User request.</param>
        /// <returns></returns>
        /// <response code="204">User successfully registered with the application.</response>
        /// <response code="400">User already registered with the application.</response>
        [HttpPost("/register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserLoginRequest newUser)
        {
            var registered = await _authService.RegisterUser(newUser);

            if (!registered)
            {
                return GetBadRequestResponse("User already exists.");
            }

            return NoContent();
        }

        /// <summary>
        /// Log a user into the application.
        /// </summary>
        /// <param name="login">User login request.</param>
        /// <returns></returns>
        /// <response code="200">Successful login request, returns Bearer Token.</response>
        /// <response code="401">Unsuccessful login request, returns 401 unauthorized.</response>
        [HttpPost("/login")]
        public async Task<ActionResult<JWTModel>> Login([FromBody] UserLoginRequest login)
        {
            var jwt = await _authService.AuthenticateUser(login);

            if(jwt is null)
            {
                return Unauthorized();
            }

            return Ok(jwt);
        }

        /// <summary>
        /// Error view for AuthController.
        /// </summary>
        /// <returns>Error view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}