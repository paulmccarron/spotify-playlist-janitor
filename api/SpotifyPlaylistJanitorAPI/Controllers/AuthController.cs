using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models;
using SpotifyPlaylistJanitorAPI.Services;
using System.Diagnostics;

namespace SpotifyPlaylistJanitorAPI.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly SpotifyService _spotifyService;
        private readonly SpotifyOption _spotifyOptions;
        private readonly ILogger<AuthController> _logger;
        private static string STATE = $"{Guid.NewGuid()}{Guid.NewGuid()}".Replace("-", "");

        public AuthController(SpotifyService spotifyService, IOptions<SpotifyOption> spotifyOptions, ILogger<AuthController> logger)
        {
            _spotifyService = spotifyService;
            _spotifyOptions = spotifyOptions.Value;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet()]
        public IActionResult Auth()
        {
            var baseUrl = Request.GetTypedHeaders().Referer.ToString();
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

        [HttpGet()]
        public async Task<IActionResult> Callback(string code, string state)
        {
            if (!state.Equals(STATE))
            {
                return Error();
            }

            var baseUrl = Request.GetTypedHeaders().Referer.ToString();

            await _spotifyService.CreateClient(code, $"{baseUrl.TrimEnd('/')}/auth/callback");

            var profile = await _spotifyService.GetCurrentUser();
            var userName = profile.DisplayName;
            ViewBag.UserName = userName;

            return View("~/Views/Auth/Callback.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}