using Microsoft.AspNetCore.Mvc;
using SpotifyPlaylistJanitorAPI.Models;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using SpotifyPlaylistJanitorAPI.SwaggerExamples.Spotify;
using Swashbuckle.AspNetCore.Filters;

namespace SpotifyPlaylistJanitorAPI.Controllers
{
    /// <summary>
    /// Controller for requests to Spotify API.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class SpotifyController : Controller
    {
        private readonly ISpotifyService _spotifyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyController"/> class.
        /// </summary>
        /// <param name="spotifyService">The Spotify Service.</param>
        public SpotifyController(ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        /// <summary>
        /// Returns the current user details.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Current user details.</response>
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

            var user = await _spotifyService.GetUserDetails();

            return user;
        }

        /// <summary>
        /// Returns the current user's playlists.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Current user's playlists.</response>
        /// <response code="500">Application has not been logged into users Spotify account.</response>
        [HttpGet("playlists")]
        [ProducesResponseType(typeof(IEnumerable<SpotifyPlaylistModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SpotifyPlaylistModelExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationNotLoggedInExample))]
        public async Task<ActionResult<IEnumerable<SpotifyPlaylistModel>>> GetPlaylists()
        {
            if (!_spotifyService.IsLoggedIn)
            {
                return GetApplicationNotLoggedResponse();
            }

            var playlists = await _spotifyService.GetUserPlaylists();

            return Ok(playlists);
        }

        private ActionResult GetApplicationNotLoggedResponse()
        {
            return StatusCode(500, new { Message = "Application has not been logged into your Spotify account." });
        }
    }
}
