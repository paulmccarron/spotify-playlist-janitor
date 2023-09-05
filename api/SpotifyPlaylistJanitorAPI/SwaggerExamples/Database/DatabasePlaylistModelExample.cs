using SpotifyPlaylistJanitorAPI.Models.Database;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.SwaggerExamples.Database
{
    /// <summary>
    /// Swagger example.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DatabasePlaylistModelExample : IExamplesProvider<DatabasePlaylistModel>
    {
        /// <summary>
        /// Implementation of Swashbuckle GetExamples() IExamplesProvider method.
        /// </summary>
        /// <returns>Example <see cref="DatabasePlaylistModel"/>.</returns>
        public DatabasePlaylistModel GetExamples()
        {
            return new DatabasePlaylistModel
            {
                Id = "playlist_id",
                Name = "Playlist Name",
                Href = "https://open.spotify.com/playlist/playlist_id",
            };
        }
    }
}
