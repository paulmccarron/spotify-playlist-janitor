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
        /// Returns dictionary of Spotify Clients.
        /// </summary>
        Dictionary<string, ISpotifyClient> SpotifyClients { get; }

        /// <summary>
        /// Returns true if service has a Spotify Client configured for user.
        /// </summary>
        bool UserIsLoggedIn(string spotifyUsername);

        /// <summary>
        /// Create an instance a new instance of the <see cref="SpotifyClient"/> class and sets it to internal field.
        /// using provided code and callbackUrl.
        /// </summary>
        /// <param name="code">Callback code provide by first part of the Authorization flow.</param>
        /// <param name="callbackUrl">Callback URL provide by first part of the Authorization flow.</param>
        Task CreateClient(string code, string callbackUrl);

        /// <summary>
        /// Returns current users details.
        /// </summary>
        /// <returns><see cref = "SpotifyUserModel" /> with user details</returns>
        Task<SpotifyUserModel> GetUserDetails(string spotifyUsername);

        /// <summary>
        /// Returns current users playlists.
        /// </summary>
        ///<returns>Returns an <see cref="IEnumerable{T}" /> of type <see cref = "SpotifyPlaylistModel" />.</returns>
        Task<IEnumerable<SpotifyPlaylistModel>> GetUserPlaylists(string spotifyUsername);

        /// <summary>
        /// Returns current users playlist by id.
        /// </summary>
        /// <returns><see cref = "SpotifyPlaylistModel" /></returns>
        Task<SpotifyPlaylistModel?> GetUserPlaylist(string spotifyUsername, string id);

        /// <summary>
        /// Returns tracks from current users playlist by id.
        /// </summary>
        ///<returns>Returns an <see cref="IEnumerable{T}" /> of type <see cref = "SpotifyTrackModel" />.</returns>
        Task<IEnumerable<SpotifyTrackModel>> GetUserPlaylistTracks(string spotifyUsername, string id);

        /// <summary>
        /// Returns current playback state.
        /// </summary>
        /// <returns><see cref = "SpotifyPlayingState" /> Current playback state.</returns>
        Task<SpotifyPlayingState> GetCurrentPlayback(string spotifyUsername);

        /// <summary>
        /// Remove tracks from current users playlist.
        /// </summary>
        /// <param name="playlistId">Id of Spotify playlist.</param>
        /// <param name="trackIds">Collection if track Ids to remove.</param>
        /// <returns></returns>
        Task<SnapshotResponse> DeletePlaylistTracks(string spotifyUsername, string playlistId, IEnumerable<string> trackIds);
    }
}
