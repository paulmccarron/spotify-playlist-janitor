using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyPlaylistJanitorAPI.Models;
using SpotifyPlaylistJanitorAPI.Services;
using SpotifyPlaylistJanitorAPI.SwaggerExamples.Spotify;
using Swashbuckle.AspNetCore.Filters;

namespace SpotifyPlaylistJanitorAPI.Controllers
{
    /// <summary>
    /// Controller for requests to Spotify API
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class SpotifyController : Controller
    {
        private readonly SpotifyService _spotifyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyController"/> class.
        /// </summary>
        /// <param name="spotifyService">The Spotify Service</param>
        public SpotifyController(SpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        /// <summary>
        /// Returns the currently logged in user.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Currently logged in user.</response>
        /// <response code="500">Application has not been logged into users Spotify account.</response>
        [HttpGet("user")]
        [ProducesResponseType(typeof(SpotifyUserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SpotifyUserModelExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationNotLoggedInExample))]
        public async Task<ActionResult<SpotifyUserModel>> GetUser()
        {
            if (!_spotifyService.IsLoggedIn)
            {
                return GetApplicationNotLoggedResponse();
            }

            var user = await _spotifyService.GetCurrentUser();

            return new OkObjectResult(user);
        }

        private ObjectResult GetApplicationNotLoggedResponse()
        {
            return StatusCode(500, new { Message = "Application has not been logged into your Spotify account." });
        }
    }
}
