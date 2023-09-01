using SpotifyPlaylistJanitorAPI.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.SwaggerExamples.Spotify
{
    /// <summary>
    /// Swagger example.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SpotifyUserModelExample : IExamplesProvider<SpotifyUserModel>
    {
        /// <summary>
        /// Implementation of Swashbuckle GetExamples() IExamplesProvider method.
        /// </summary>
        /// <returns>Example <see cref="SpotifyUserModel"/>.</returns>
        public SpotifyUserModel GetExamples()
        {
            return new SpotifyUserModel
            {
                Id = "spotifyUserId", 
                DisplayName = "Spotify User Display",
                Email = "spotifyUserEmailsAddress@email.com",
                Href = "https://api.spotify.com/v1/users/spotifyUserId",
            };
        }
    }
}
