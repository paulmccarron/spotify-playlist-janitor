using Microsoft.EntityFrameworkCore;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.DataAccess.Models;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Services
{
    /// <summary>
    /// Service to interact with application database.
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
        /// Returns playlists from database.
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

        /// <summary>
        /// Returns playlist from database by id.
        /// </summary>
        /// <returns>Returns a <see cref = "DatabasePlaylistModel" />.</returns>
        public async Task<DatabasePlaylistModel?> GetPlaylist(string id)
        {
            var playlistDto = await _context.SpotifyPlaylists
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(x => x.Id == id);

            var playlistModel = playlistDto is null ? null : new DatabasePlaylistModel
            {
                Id = playlistDto.Id,
                Name = playlistDto.Name,
                Href = playlistDto.Href,
            };

            return playlistModel;
        }

        /// <summary>
        /// Add playlist to database.
        /// </summary>
        /// <returns>Returns a <see cref = "DatabasePlaylistModel" />.</returns>
        public async Task<DatabasePlaylistModel> AddPlaylist(DatabasePlaylistRequest playlistRequest)
        {
            var playlistDto = new SpotifyPlaylist
            {
                Id = playlistRequest.Id,
                Name = playlistRequest.Name,
                Href = playlistRequest.Href,
            };

            await _context.AddAsync(playlistDto);
            await _context.SaveChangesAsync();

            var playlistModel = new DatabasePlaylistModel
            {
                Id = playlistDto.Id,
                Name = playlistDto.Name,
                Href = playlistDto.Href,
            };

            return playlistModel;
        }

        /// <summary>
        /// Deletes playlist from database.
        /// </summary>
        public async Task DeletePlaylist(string id)
        {
            _context.SkippedTracks.RemoveRange(_context.SkippedTracks.Where(track => track.SpotifyPlaylistId == id));
            _context.SpotifyPlaylists.RemoveRange(_context.SpotifyPlaylists.Where(playlist => playlist.Id == id));
            await _context.SaveChangesAsync();
        }
    }
}
