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
        /// Returns current user monitored playlists.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Current monitored playlists.</response>
        [HttpGet("playlists")]
        [ProducesResponseType(typeof(IEnumerable<DatabasePlaylistModel>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DatabasePlaylistsModelExample))]
        public async Task<ActionResult<IEnumerable<DatabasePlaylistModel>>> GetMonitoredPlaylists()
        {
            var playlists = await _databaseService.GetPlaylists();

            return Ok(playlists);
        }

        /// <summary>
        /// Returns current user monitored playlist by id.
        /// </summary>
        /// <param name="id">Playlist id.</param>
        /// <returns></returns>
        /// <response code="200">Current monitored playlist.</response>
        /// <response code="404">No playlist found for given Id.</response>
        [HttpGet("playlists/{id}")]
        [ProducesResponseType(typeof(DatabasePlaylistModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DatabasePlaylistModelExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(DatabasePlaylistNotFoundExample))]
        public async Task<ActionResult<DatabasePlaylistModel>> GetMonitoredPlaylist(string id)
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
        /// <param name="playlistRequest">Playlist request model.</param>
        /// <returns></returns>
        /// <response code="201">Playlist successfully added.</response>
        /// <response code="400">Playlist already exists.</response>
        [HttpPost("playlists")]
        [ProducesResponseType(typeof(DatabasePlaylistModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(DatabasePlaylistModelExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DatabasePlaylistAlreadyExistsExample))]
        public async Task<ActionResult<DatabasePlaylistModel>> CreateMonitoredPlaylist([FromBody] DatabasePlaylistRequest playlistRequest)
        {
            var existingPlaylist = await _databaseService.GetPlaylist(playlistRequest.Id);

            if (existingPlaylist is not null)
            {
                return BadRequestResponse($"Playlist with id: {playlistRequest.Id} already exists");
            }

            var playlist = await _databaseService.AddPlaylist(playlistRequest);

            return new ObjectResult(playlist)
            {
                StatusCode = 201,
            };
        }

        /// <summary>
        /// Deletes current user monitored playlist by id.
        /// </summary>
        /// <param name="id">Playlist id.</param>
        /// <returns></returns>
        /// <response code="204">Playlist successfully deleted.</response>
        /// <response code="404">No playlist found for given Id.</response>
        [HttpDelete("playlists/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(DatabasePlaylistNotFoundExample))]
        public async Task<ActionResult> DeleteMonitoredPlaylist(string id)
        {
            var playlist = await _databaseService.GetPlaylist(id);

            if(playlist is null)
            {
                return NotFoundResponse($"Could not find playlist with id: {id}");
            }

            await _databaseService.DeletePlaylist(id);

            return NoContent();
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
