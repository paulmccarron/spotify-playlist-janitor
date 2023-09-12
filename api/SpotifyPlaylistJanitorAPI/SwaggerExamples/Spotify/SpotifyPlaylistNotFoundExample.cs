using SpotifyPlaylistJanitorAPI.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.SwaggerExamples.Spotify
{
    /// <summary>
    /// Swagger example.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SpotifyPlaylistNotFoundExample : IExamplesProvider<ErrorResponseModel>
    {
        /// <summary>
        /// Implementation of Swashbuckle GetExamples() IExamplesProvider method.
        /// </summary>
        /// <returns>Example <see cref="ErrorResponseModel"/>ErrorResponseModel.</returns>
        public ErrorResponseModel GetExamples()
        {
            return new ErrorResponseModel
            {
                Message = "Could not find Spotify playlist with id: ID",
            };
        }
    }
}
