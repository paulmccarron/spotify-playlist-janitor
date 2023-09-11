using SpotifyPlaylistJanitorAPI.Models.Database;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.SwaggerExamples.Database
{
    /// <summary>
    /// Swagger example.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DatabasePlaylistsModelExample : IExamplesProvider<IEnumerable<DatabasePlaylistModel>>
    {
        /// <summary>
        /// Implementation of Swashbuckle GetExamples() IExamplesProvider method.
        /// </summary>
        /// <returns>Example <see cref="IEnumerable{T}"/> of type <see cref="DatabasePlaylistModel"/>.</returns>
        public IEnumerable<DatabasePlaylistModel> GetExamples()
        {
            return new[]
            {
                new DatabasePlaylistModel
                {
                    Id = "playlist_id_1",
                },
                new DatabasePlaylistModel
                {
                    Id = "playlist_id_2",
                },
                new DatabasePlaylistModel
                {
                    Id = "playlist_id_3",
                },
                new DatabasePlaylistModel
                {
                    Id = "playlist_id_4",
                },
            };
        }
    }
}
