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
        /// Returns current users playlists
        /// </summary>
        ///<returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "DatabasePlaylistModel" />.</returns>
        Task<IEnumerable<DatabasePlaylistModel>> GetPlaylists();

        /// <summary>
        /// Returns current users playlist by id
        /// </summary>
        ///<returns>Returns a <see cref = "DatabasePlaylistModel" />.</returns>
        Task<DatabasePlaylistModel?> GetPlaylist(string id);
    }
}
