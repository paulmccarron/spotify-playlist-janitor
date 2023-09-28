using SpotifyPlaylistJanitorAPI.Models.Auth;
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
        /// Update playlist in the database.
        /// </summary>
        ///<returns>Returns a <see cref = "DatabasePlaylistModel" />.</returns>
        Task<DatabasePlaylistModel?> UpdatePlaylist(string id, DatabasePlaylistUpdateRequest playlistUpdateRequest);

        /// <summary>
        /// Deletes playlist from database.
        /// </summary>
        Task DeletePlaylist(string id);

        /// <summary>
        /// Add artist to database.
        /// </summary>
        Task AddArtist(DatabaseArtistModel artistRequest);

        /// <summary>
        /// Add album to database.
        /// </summary>
        Task AddAlbum(DatabaseAlbumRequest albumRequest);

        /// <summary>
        /// Add image to database.
        /// </summary>
        Task<DatabaseImageModel> AddImage(int height, int width, string url);

        /// <summary>
        /// Add track to database.
        /// </summary>
        ///
        Task AddTrack(DatabaseTrackModel trackRequest);

        /// <summary>
        /// Add artist to track relationship to database.
        /// </summary>
        Task AddArtistToTrack(string artistId, string trackId);

        /// <summary>
        /// Add artist to album relationship to database.
        /// </summary>
        Task AddArtistToAlbum(string artistId, string albumId);

        /// <summary>
        /// Add image to album relationship to database.
        /// </summary>
        Task AddImageToAlbum(int imageId, string albumId);

        /// <summary>
        /// Add skipped track to database.
        /// </summary>
        Task AddSkippedTrack(DatabaseSkippedTrackRequest skippedTrackRequest);

        /// <summary>
        /// Remove skipped tracks from database.
        /// </summary>
        Task DeleteSkippedTracks(string playlistId, IEnumerable<string> trackIds);

        /// <summary>
        /// Get skipped tracks for monitored plauylist from database.
        /// </summary>
        ///<returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "DatabaseSkippedTrackResponse" />.</returns>
        Task<IEnumerable<DatabaseSkippedTrackResponse>> GetPlaylistSkippedTracks(string playlistId);

        /// <summary>
        /// Returns user from database.
        /// </summary>
        ///<returns>Returns a <see cref = "UserDataModel" />.</returns>
        Task<UserDataModel?> GetUser(string username);

        /// <summary>
        /// Adds user to database.
        /// </summary>
        Task AddUser(string username, string passwordHash);

        /// <summary>
        /// Updates user refresh token in database.
        /// </summary>
        Task UpdateUserRefreshToken(string username, string? refreshToken, DateTime? refreshTokenExpiry);
    }
}
