using SpotifyPlaylistJanitorAPI.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.SwaggerExamples.Database
{
    /// <summary>
    /// Swagger example.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DatabasePlaylistNotFoundExample : IExamplesProvider<ErrorResponseModel>
    {
        /// <summary>
        /// Implementation of Swashbuckle GetExamples() IExamplesProvider method.
        /// </summary>
        /// <returns>Example <see cref="ErrorResponseModel"/>ErrorResponseModel.</returns>
        public ErrorResponseModel GetExamples()
        {
            return new ErrorResponseModel
            {
                Message = "Could not find playlist with id: ID",
            };
        }
    }
}
