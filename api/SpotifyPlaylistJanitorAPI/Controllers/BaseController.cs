using Microsoft.AspNetCore.Mvc;

namespace SpotifyPlaylistJanitorAPI.Controllers
{
    /// <summary>
    /// Abstract class to provide Object response methods
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Return a NotFoundObjectResult with supplied message.
        /// </summary>
        /// <param name="message"></param>
        ///<returns>Returns a <see cref = "NotFoundObjectResult" />.</returns>
        protected NotFoundObjectResult GetNotFoundResponse(string message)
        {
            return NotFound(new { Message = message });
        }

        /// <summary>
        /// Return a BadRequestObjectResult with supplied message.
        /// </summary>
        /// <param name="message"></param>
        ///<returns>Returns a <see cref = "BadRequestObjectResult" />.</returns>
        protected BadRequestObjectResult GetBadRequestResponse(string message)
        {
            return BadRequest(new { Message = message });
        }
    }
}
