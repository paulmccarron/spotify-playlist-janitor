using Microsoft.AspNetCore.Mvc;
using SpotifyPlaylistJanitorAPI.Models;
using SpotifyPlaylistJanitorAPI.Models.Database;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.SwaggerExamples.Database
{
    /// <summary>
    /// Swagger example.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DatabasePlaylistAlreadyExistsExample : IExamplesProvider<ErrorResponseModel>
    {
        /// <summary>
        /// Implementation of Swashbuckle GetExamples() IExamplesProvider method.
        /// </summary>
        /// <returns>Example <see cref="ErrorResponseModel"/>ErrorResponseModel.</returns>
        public ErrorResponseModel GetExamples()
        {
            return new ErrorResponseModel
            {
                Message = $"Playlist with id: ID already exists",
            };
        }
    }
}
