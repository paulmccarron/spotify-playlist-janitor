using SpotifyPlaylistJanitorAPI.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.SwaggerExamples.Auth
{
    /// <summary>
    /// Swagger example.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserAlreadyExistsExample : IExamplesProvider<ErrorResponseModel>
    {
        /// <summary>
        /// Implementation of Swashbuckle GetExamples() IExamplesProvider method.
        /// </summary>
        /// <returns>Example <see cref="ErrorResponseModel"/>ErrorResponseModel.</returns>
        public ErrorResponseModel GetExamples()
        {
            return new ErrorResponseModel
            {
                Message = "User already exists.",
            };
        }
    }
}
