using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using SpotifyPlaylistJanitorAPI.SwaggerExamples.Auth;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace SpotifyPlaylistJanitorAPI.Controllers
{
    /// <summary>
    /// Controller for authorising application with Spotify API to make calls on behalf of the user.
    /// </summary>
    public class AuthController : BaseController
    {
        private readonly ISpotifyService _spotifyService;
        private readonly ISpotifyClientService _spotifyClientService;
        private readonly IAuthService _authService;
        private readonly SpotifyOption _spotifyOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static string STATE = $"{Guid.NewGuid()}{Guid.NewGuid()}".Replace("-", "");

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="spotifyService">The Spotify Service.</param>
        /// <param name="spotifyClientService">The Spotify Client Service.</param>
        /// <param name="authService">The Authentication Service.</param>
        /// <param name="spotifyOptions">The Spotify access credentials read from environment vars.</param>
        /// <param name="httpContextAccessor">Http Context Accessor.</param>
        public AuthController(ISpotifyService spotifyService, ISpotifyClientService spotifyClientService, IAuthService authService, IOptions<SpotifyOption> spotifyOptions, IHttpContextAccessor httpContextAccessor)
        {
            _spotifyService = spotifyService;
            _spotifyClientService = spotifyClientService;
            _authService = authService;
            _spotifyOptions = spotifyOptions.Value;
            _httpContextAccessor = httpContextAccessor;
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
            _spotifyClientService.CheckSpotifyCredentials();
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

            await _spotifyService.CreateClient(code, $"{baseUrl.TrimEnd('/')}/auth/callback");

            return Redirect("~/");
        }

        /// <summary>
        /// Error view for AuthController.
        /// </summary>
        /// <returns>Error view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [ExcludeFromCodeCoverage]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Register a new user with the application.
        /// </summary>
        /// <param name="newUser">New User request.</param>
        /// <returns></returns>
        /// <response code="204">User successfully registered with the application.</response>
        /// <response code="400">User already registered with the application.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(UserAlreadyExistsExample))]
        [HttpPost("/register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequest newUser)
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
        [ProducesResponseType(typeof(UserLoginExample), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status401Unauthorized)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserLoginExample))]
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
        /// Refresh user bearer token.
        /// </summary>
        /// <param name="refreshRequest">Token refresh request.</param>
        /// <returns></returns>
        /// <response code="200">Successful refresh request, returns new Bearer Token.</response>
        /// <response code="400">Unsuccessful refresh request, supplied refresh token is invalid.</response>
        [ProducesResponseType(typeof(UserLoginExample), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserLoginExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(InvalidRefreshResponseExample))]
        [Authorize]
        [HttpPost("/refresh")]
        public async Task<ActionResult<JWTModel>> Refresh([FromBody] TokenRefreshModel refreshRequest)
        {
            var accessToken = await _httpContextAccessor.HttpContext?.GetTokenAsync("access_token");
            var jwt = await _authService.RefreshUserToken(accessToken, refreshRequest.RefreshToken);

            if (jwt is null)
            {
                return GetBadRequestResponse("Invalid refresh request.");
            }

            return Ok(jwt);
        }

        /// <summary>
        /// Revoke current users refresh token.
        /// </summary>
        /// <returns></returns>
        /// <response code="204">Successful revoked users refresh token.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        [HttpPost("/revoke")]
        public async Task<ActionResult<JWTModel>> Revoke()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            await _authService.ExpireUserRefreshToken(username);

            return NoContent();
        }
    }
}