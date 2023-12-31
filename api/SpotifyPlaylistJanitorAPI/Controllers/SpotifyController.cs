﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotifyPlaylistJanitorAPI.Models;
using SpotifyPlaylistJanitorAPI.Models.Spotify;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using SpotifyPlaylistJanitorAPI.SwaggerExamples.Spotify;
using Swashbuckle.AspNetCore.Filters;

namespace SpotifyPlaylistJanitorAPI.Controllers
{
    /// <summary>
    /// Controller for requests to Spotify API.
    /// </summary>
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class SpotifyController : Controller
    {
        private readonly ISpotifyService _spotifyService;
        private readonly IDatabaseService _databaseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyController"/> class.
        /// </summary>
        /// <param name="spotifyService">The Spotify Service.</param>
        /// <param name="databaseService">The Database Service.</param>
        public SpotifyController(ISpotifyService spotifyService, IDatabaseService databaseService)
        {
            _spotifyService = spotifyService;
            _databaseService = databaseService;
        }

        /// <summary>
        /// Returns the current user details.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Current user details.</response>
        /// <response code="401">No valid Bearer Token in request header.</response>
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
        /// <response code="401">No valid Bearer Token in request header.</response>
        /// <response code="500">Application has not been logged into users Spotify account.</response>
        [HttpGet("playlists")]
        [ProducesResponseType(typeof(IEnumerable<SpotifyPlaylistModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SpotifyPlaylistsModelExample))]
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

        /// <summary>
        /// Returns current user's playlist by id.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Current user's playlist.</response>
        /// <response code="401">No valid Bearer Token in request header.</response>
        /// <response code="404">No playlist found for that id.</response>
        /// <response code="500">Application has not been logged into users Spotify account.</response>
        [HttpGet("playlists/{id}")]
        [ProducesResponseType(typeof(SpotifyPlaylistModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SpotifyPlaylistModelExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(SpotifyPlaylistNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationNotLoggedInExample))]
        public async Task<ActionResult<SpotifyPlaylistModel>> GetPlaylist(string id)
        {
            if (!_spotifyService.IsLoggedIn)
            {
                return GetApplicationNotLoggedResponse();
            }

            var playlist = await _spotifyService.GetUserPlaylist(id);

            if(playlist is null)
            {
                return PlaylistNotFoundResponse(id);
            }

            return Ok(playlist);
        }

        /// <summary>
        /// Returns tracks from current user's playlist by id.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Tracks from current user's playlist.</response>
        /// <response code="401">No valid Bearer Token in request header.</response>
        /// <response code="404">No playlist found for that id.</response>
        /// <response code="500">Application has not been logged into users Spotify account.</response>
        [HttpGet("playlists/{id}/tracks")]
        [ProducesResponseType(typeof(IEnumerable<SpotifyTrackModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SpotifyTracksModelExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(SpotifyPlaylistNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationNotLoggedInExample))]
        public async Task<ActionResult<IEnumerable<SpotifyTrackModel>>> GetPlaylistTracks(string id)
        {
            if (!_spotifyService.IsLoggedIn)
            {
                return GetApplicationNotLoggedResponse();
            }

            var playlist = await _spotifyService.GetUserPlaylist(id);

            if (playlist is null)
            {
                return PlaylistNotFoundResponse(id);
            }

            var tracks = await _spotifyService.GetUserPlaylistTracks(id);

            return Ok(tracks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trackIds">Max 100 track Ids supported.</param>
        /// <returns></returns>
        /// <response code="204">Tracks successfully removed from current user's playlist.</response>
        /// <response code="401">No valid Bearer Token in request header.</response>
        /// <response code="404">No playlist found for that id.</response>
        /// <response code="500">Application has not been logged into users Spotify account.</response>
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(SpotifyPlaylistNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationNotLoggedInExample))]
        [HttpDelete("playlists/{id}/tracks")]
        public async Task<ActionResult<object>> DeletePlaylistTracks(string id, [FromQuery(Name = "trackIds[]")] IEnumerable<string> trackIds)
        {
            if (!_spotifyService.IsLoggedIn)
            {
                return GetApplicationNotLoggedResponse();
            }

            var playlist = await _spotifyService.GetUserPlaylist(id);

            if (playlist is null)
            {
                return PlaylistNotFoundResponse(id);
            }

            await _spotifyService.DeletePlaylistTracks(id, trackIds);
            await _databaseService.DeleteSkippedTracks(id, trackIds);

            return NoContent();
        }

        private ActionResult GetApplicationNotLoggedResponse()
        {
            return StatusCode(500, new { Message = "Application has not been logged into your Spotify account." });
        }

        private NotFoundObjectResult PlaylistNotFoundResponse(string id)
        {
            return NotFound(new { Message = $"Could not find Spotify playlist with id: {id}" });
        }
    }
}
