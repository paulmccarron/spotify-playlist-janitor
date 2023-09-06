using Microsoft.AspNetCore.Mvc;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using SpotifyPlaylistJanitorAPI.Models;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.SwaggerExamples.Database;
using Swashbuckle.AspNetCore.Filters;

namespace SpotifyPlaylistJanitorAPI.Controllers
{
    /// <summary>
    /// Controller for requests to the database.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class DataController : Controller
    {
        private readonly IDatabaseService _databaseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataController"/> class.
        /// </summary>
        /// <param name="databaseService">The Database Service.</param>
        public DataController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Returns current user tracked playlists.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Current tracked playlists.</response>
        [HttpGet("playlists")]
        [ProducesResponseType(typeof(IEnumerable<DatabasePlaylistModel>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DatabasePlaylistsModelExample))]
        public async Task<ActionResult<IEnumerable<DatabasePlaylistModel>>> GetTrackedPlaylists()
        {
            var playlists = await _databaseService.GetPlaylists();

            return Ok(playlists);
        }

        /// <summary>
        /// Returns current user tracked playlist by id.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Current tracked playlist.</response>
        /// <response code="404">No playlist found for given Id.</response>
        [HttpGet("playlists/{id}")]
        [ProducesResponseType(typeof(DatabasePlaylistModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DatabasePlaylistModelExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(DatabasePlaylistNotFoundExample))]
        public async Task<ActionResult<DatabasePlaylistModel>> GetTrackedPlaylist(string id)
        {
            var playlist = await _databaseService.GetPlaylist(id);

            if(playlist is null)
            {
                return NotFoundResponse($"Could not find playlist with id: {id}");
            }

            return Ok(playlist);
        }

        /// <summary>
        /// Add playlist to database for the current user.
        /// </summary>
        /// <returns></returns>
        /// <response code="201">Playlist successfully added.</response>
        /// <response code="400">Playlist already exists.</response>
        [HttpPost("playlists")]
        [ProducesResponseType(typeof(DatabasePlaylistModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(DatabasePlaylistModelExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DatabasePlaylistAlreadyExistsExample))]
        public async Task<ActionResult<DatabasePlaylistModel>> CreateTrackedPlaylist([FromBody] DatabasePlaylistRequest playlistRequest)
        {
            var existingPlaylist = await _databaseService.GetPlaylist(playlistRequest.Id);

            if (existingPlaylist is not null)
            {
                return BadRequestResponse($"Playlist with id: {playlistRequest.Id} already exists");
            }

            var playlist = await _databaseService.AddPlaylist(playlistRequest.Id, playlistRequest.Name, playlistRequest.Href);

            return new ObjectResult(playlist)
            {
                StatusCode = 201,
            };
        }

        private NotFoundObjectResult NotFoundResponse(string message)
        {
            return NotFound(new { Message = message });
        }

        private BadRequestObjectResult BadRequestResponse(string message)
        {
            return BadRequest(new { Message = message });
        }
    }
}
