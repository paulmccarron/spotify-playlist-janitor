using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.Exceptions;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Models.Spotify;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.Services
{
    /// <summary>
    /// Service to interact with Spotify API.
    /// </summary>
    public class DatabaseService : IDatabaseService
    {
        private readonly SpotifyPlaylistJanitorDatabaseContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseService"/> class.
        /// </summary>
        /// <param name="context">Database context.</param>
        public DatabaseService(SpotifyPlaylistJanitorDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns current users tracked playlists from the database.
        /// </summary>
        /// <returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "DatabasePlaylistModel" />.</returns>
        public async Task<IEnumerable<DatabasePlaylistModel>> GetPlaylists()
        {
            var playlists = await _context.SpotifyPlaylists
                .Select(x => new DatabasePlaylistModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Href = x.Href,
                })
                .OrderBy(playlist => playlist.Name)
                .ToAsyncEnumerable()
                .ToListAsync();

            return playlists;
        }
    }
}
