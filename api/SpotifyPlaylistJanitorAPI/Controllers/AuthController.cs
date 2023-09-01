using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.Controllers
{
    /// <summary>
    /// Controller for authorising application with Spotify API to make calls on behalf of the user.
    /// </summary>
    [AllowAnonymous]
    [ExcludeFromCodeCoverage]
    public class AuthController : Controller
    {
        private readonly ISpotifyService _spotifyService;
        private readonly SpotifyOption _spotifyOptions;
        private static string STATE = $"{Guid.NewGuid()}{Guid.NewGuid()}".Replace("-", "");

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="spotifyService">The Spotify Service.</param>
        /// <param name="spotifyOptions">The Spotify access credentials read from environment vars.</param>
        public AuthController(ISpotifyService spotifyService, IOptions<SpotifyOption> spotifyOptions)
        {
            _spotifyService = spotifyService;
            _spotifyOptions = spotifyOptions.Value;
        }

        /// <summary>
        /// Default view for AuthController.
        /// </summary>
        /// <returns>Default view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Auth view for AuthController.
        /// </summary>
        /// <returns>Auth view.</returns>
        [HttpGet()]
        public IActionResult Auth()
        {
            _spotifyService.CheckSpotifyCredentials();
            var baseUrl = Request?.GetTypedHeaders()?.Referer?.ToString() ?? "";
            var loginRequest = new LoginRequest(new Uri($"{baseUrl.TrimEnd('/')}/callback"), _spotifyOptions.ClientId, LoginRequest.ResponseType.Code)
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
        [HttpGet()]
        public async Task<IActionResult> Callback(string code, string state)
        {
            if (!state.Equals(STATE))
            {
                return Error();
            }

            var baseUrl = Request?.GetTypedHeaders()?.Referer?.ToString() ?? "";

            var spotifyClient = await _spotifyService.CreateClient(code, $"{baseUrl.TrimEnd('/')}/auth/callback");
            _spotifyService.SetClient(spotifyClient);

            var profile = await _spotifyService.GetUserDetails();
            var userName = profile.DisplayName;
            ViewBag.UserName = userName;

            return View("~/Views/Auth/Callback.cshtml");
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