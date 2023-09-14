using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Models.Spotify;

namespace SpotifyPlaylistJanitorAPI.Services.Interfaces
{
    /// <summary>
    /// Service to interact with Spotify API.
    /// Interface introduced to simplify unit testing as well as to provide flexibility for future.
    /// </summary>
    public interface ISpotifyService
    {
        /// <summary>
        /// Sets the internal Spotify Client fro the service.
        /// </summary>
        /// <param name="spotifyClient"></param>
        void SetClient(ISpotifyClient? spotifyClient);

        /// <summary>
        /// Returns true if service has a Spotify Client configured.
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        /// Create an instance a new instance of the <see cref="SpotifyClient"/> class.
        /// using provided code and callbackUrl.
        /// </summary>
        /// <param name="code">Callback code provide by first part of the Authorization flow.</param>
        /// <param name="callbackUrl">Callback URL provide by first part of the Authorization flow.</param>
        /// <returns><see cref = "SpotifyClient" /> that is authenticated for users Spotify account.</returns>
        Task<ISpotifyClient> CreateClient(string code, string callbackUrl);

        /// <summary>
        /// Returns current users details.
        /// </summary>
        /// <returns><see cref = "SpotifyUserModel" /> with user details</returns>
        Task<SpotifyUserModel> GetUserDetails();

        /// <summary>
        /// Returns current users playlists.
        /// </summary>
        ///<returns>Returns an <see cref="IEnumerable{T}" /> of type <see cref = "SpotifyPlaylistModel" />.</returns>
        Task<IEnumerable<SpotifyPlaylistModel>> GetUserPlaylists();

        /// <summary>
        /// Returns current users playlist by id.
        /// </summary>
        /// <returns><see cref = "SpotifyPlaylistModel" /></returns>
        Task<SpotifyPlaylistModel?> GetUserPlaylist(string id);

        /// <summary>
        /// Returns tracks from current users playlist by id.
        /// </summary>
        ///<returns>Returns an <see cref="IEnumerable{T}" /> of type <see cref = "SpotifyTrackModel" />.</returns>
        Task<IEnumerable<SpotifyTrackModel>> GetUserPlaylistTracks(string id);

        /// <summary>
        /// Returns current playback state.
        /// </summary>
        /// <returns><see cref = "SpotifyPlayingState" /> Current playback state.</returns>
        Task<SpotifyPlayingState> GetCurrentPlayback();

        /// <summary>
        /// Remove tracks from current users playlist.
        /// </summary>
        /// <param name="playlistId">Id of Spotify playlist.</param>
        /// <param name="trackIds">Collection if track Ids to remove.</param>
        /// <returns></returns>
        Task<SnapshotResponse> DeletePlaylistTracks(string playlistId, IEnumerable<string> trackIds);

        /// <summary>
        /// Check for Spotify credentials
        /// </summary>
        void CheckSpotifyCredentials();
    }
}
