using SpotifyPlaylistJanitorAPI.Models.Database;

namespace SpotifyPlaylistJanitorAPI.Services.Interfaces
{
    /// <summary>
    /// Service to interact with application database.
    /// Interface introduced to simplify unit testing as well as to provide flexibility for future.
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

        /// <summary>
        /// Deletes playlist from database.
        /// </summary>
        Task DeletePlaylist(string id);

        /// <summary>
        /// Add artist to database.
        /// </summary>
        ///<returns>Returns a <see cref = "DatabaseArtistModel" />.</returns>
        ///
        Task<DatabaseArtistModel> AddArtist(DatabaseArtistModel artistRequest);

        /// <summary>
        /// Add album to database.
        /// </summary>
        ///<returns>Returns a <see cref = "DatabaseAlbumModel" />.</returns>
        ///
        Task<DatabaseAlbumModel> AddAlbum(DatabaseAlbumModel albumRequest);

        /// <summary>
        /// Add track to database.
        /// </summary>
        ///<returns>Returns a <see cref = "DatabaseTrackModel" />.</returns>
        ///
        Task<DatabaseTrackModel> AddTrack(DatabaseTrackModel trackRequest);

        /// <summary>
        /// Add skipped track to database.
        /// </summary>
        ///<returns>Returns a <see cref = "DatabaseSkippedTrackModel" />.</returns>
        Task<DatabaseSkippedTrackModel> AddSkippedTrack(DatabaseSkippedTrackModel skippedTrackRequest);
    }
}
