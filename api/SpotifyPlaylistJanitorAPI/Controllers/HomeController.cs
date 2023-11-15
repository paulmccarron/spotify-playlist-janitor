using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class HomeController : Controller
    {
        private readonly ISpotifyService _spotifyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="spotifyService">The Spotify Service.</param>
        public HomeController(ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        /// <summary>
        /// Default view for HomeController.
        /// </summary>
        /// <returns>Default view.</returns>
        public async Task<IActionResult> Index()
        {
            if (_spotifyService.SpotifyClients.Count > 0)
            {
                var firstUser = _spotifyService.SpotifyClients.Keys.First();
                var profile = await _spotifyService.GetUserDetails(firstUser);
                ViewBag.Message = $"Logged in as Spotify user: {profile.DisplayName}.";
                ViewBag.Button = "Re-Authorize App";
            }
            else
            {
                ViewBag.Message = "No Spotify user logged in.";
                ViewBag.Button = "Authorize App";
            }

            return View();
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