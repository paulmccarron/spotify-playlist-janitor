using Microsoft.AspNetCore.Mvc;
using SpotifyPlaylistJanitorAPI.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.SwaggerExamples.Spotify
{
    /// <summary>
    /// Swagger example.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApplicationNotLoggedInExample : IExamplesProvider<ErrorResponseModel>
    {
        /// <summary>
        /// Implementation of Swashbuckle GetExamples() IExamplesProvider method.
        /// </summary>
        /// <returns>Example <see cref="ErrorResponseModel"/>ErrorResponseModel.</returns>
        public ErrorResponseModel GetExamples()
        {
            return new ErrorResponseModel
            {
                Message = "Application has not been logged into your Spotify account."
            };
        }
    }
}
