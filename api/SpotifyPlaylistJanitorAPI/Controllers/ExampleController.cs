using Microsoft.AspNetCore.Mvc;

namespace SpotifyPlaylistJanitorAPI.Controllers
{
    [Route("[controller]")]
    //[Authorize]
    public class ExampleController : Controller
    {

        public ExampleController() { }

        [HttpGet()]
        public IActionResult GetExample()
        {
            var obj = new { data = new { prop = "value" } };

            return new OkObjectResult(obj);
        }

        [HttpPost()]
        public IActionResult PostExample(object postBody)
        {
            return new OkObjectResult(postBody);
        }

        [HttpPut("{id}")]
        public IActionResult PutExample(object putBody)
        {
            return new OkObjectResult(putBody);
        }

        [HttpDelete("{id}")]
        public IActionResult DelteExample(string id)
        {
            return StatusCode(204, new { Message = $"Deleted entity with id: {id}" });;
        }
    }
}