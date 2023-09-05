using Microsoft.AspNetCore.Mvc;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
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
        /// Returns the current user tracked playlists.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Current tracked playlists.</response>
        /// <response code="500">Application has not been logged into users Spotify account.</response>
        [HttpGet("playlists")]
        [ProducesResponseType(typeof(IEnumerable<DatabasePlaylistModel>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DatabasePlaylistModelExample))]
        public async Task<ActionResult<IEnumerable<DatabasePlaylistModel>>> GetTrackedPlaylists()
        {
            var playlists = await _databaseService.GetPlaylists();

            return Ok(playlists);
        }
    }
}
