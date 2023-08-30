using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyPlaylistJanitorAPI.Services;

namespace SpotifyPlaylistJanitorAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SpotifyController : Controller
    {
        private readonly SpotifyService _spotifyService;

        public SpotifyController(SpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            if (!_spotifyService.IsLoggedIn)
            {
                return GetAppNotAuthorizedError();
            }

            var user = await _spotifyService.GetCurrentUser();

            return new OkObjectResult(user);
        }

        private IActionResult GetAppNotAuthorizedError()
        {
            return StatusCode(500, new { Message = "Application has to been granted SPotify authorization" });
        }
    }
}
