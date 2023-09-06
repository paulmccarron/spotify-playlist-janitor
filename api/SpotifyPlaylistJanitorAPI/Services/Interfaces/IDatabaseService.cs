using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Models.Database;
using System;

namespace SpotifyPlaylistJanitorAPI.Services.Interfaces
{
    /// <summary>
    /// Introduced to simplify unit testing as well as to provide flexibility for future.
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        /// Returns playlists from database.
        /// </summary>
        ///<returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "DatabasePlaylistModel" />.</returns>
        Task<IEnumerable<DatabasePlaylistModel>> GetPlaylists();

        /// <summary>
        /// Returns playlist from database by id.
        /// </summary>
        ///<returns>Returns a <see cref = "DatabasePlaylistModel" />.</returns>
        Task<DatabasePlaylistModel?> GetPlaylist(string id);

        /// <summary>
        /// Add playlist to database.
        /// </summary>
        ///<returns>Returns a <see cref = "DatabasePlaylistModel" />.</returns>
        Task<DatabasePlaylistModel> AddPlaylist(DatabasePlaylistRequest playlistRequest);
    }
}
