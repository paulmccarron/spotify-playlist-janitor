using Microsoft.Extensions.Options;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SpotifyPlaylistJanitorAPI.Services
{
    /// <summary>
    /// Service that handles user information from application database.
    /// </summary>
    public class DatabaseUserService : IUserService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ISecurityService _securityService;
        private readonly SpotifyOption _spotifyOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseUserService"/> class.
        /// </summary>
        /// <param name="databaseService">Service that impliments the <see cref="IDatabaseService"/> interface.</param>
        /// <param name="securityService">Service that impliments the <see cref="ISecurityService"/> interface.</param>
        /// <param name="spotifyOptions">The Spotify access credentials read from environment vars.</param>
        public DatabaseUserService(IDatabaseService databaseService, ISecurityService securityService, IOptions<SpotifyOption> spotifyOptions)
        {
            _databaseService = databaseService;
            _securityService = securityService;
            _spotifyOptions = spotifyOptions.Value;
        }

        /// <summary>
        /// Returns user from database.
        /// </summary>
        ///<returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "UserDataModel" />.</returns>
        public async Task<IEnumerable<UserDataModel>> GetUsers()
        {
            return await _databaseService.GetUsers();
        }

        /// <summary>
        /// Returns user from database.
        /// </summary>
        /// <param name="username"></param>
        ///<returns>Returns a <see cref = "UserDataModel" />.</returns>
        public async Task<UserDataModel?> GetUser(string username)
        {
            return await _databaseService.GetUser(username);
        }

        /// <summary>
        /// Adds user to databse.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public async Task AddUser(string username, string spotifyUsername, string passwordHash)
        {
            await _databaseService.AddUser(username, spotifyUsername, passwordHash);
        }

        /// <summary>
        /// Set user refresh token in store.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="refreshToken"></param>
        /// <param name="tokenExpiry"></param>
        /// <returns></returns>
        public async Task SetUserRefreshToken(string username, string refreshToken, DateTime tokenExpiry)
        {
            await _databaseService.UpdateUserRefreshToken(username, refreshToken, tokenExpiry);
        }

        /// <summary>
        /// Remove user refresh token from store.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task ExpireUserRefreshToken(string username)
        {
            await _databaseService.UpdateUserRefreshToken(username, null, null);
        }

        /// <summary>
        /// Store user spotify client token in database.
        /// </summary>
        public async Task AddUserSpotifyToken(string username, string? spotifyToken)
        {
            var dataString = spotifyToken is null ? null : _securityService.EncryptString(spotifyToken, _spotifyOptions.ClientSecret);

            await _databaseService.AddUserEncodedSpotifyToken(username, dataString);
        }

        /// <summary>
        /// Retrieve user spotify client token from database.
        /// </summary>
        public async Task<UserSpotifyTokenModel?> GetUserSpotifyToken(string username)
        {
            var userEncodedTokenModel = await _databaseService.GetUserEncodedSpotifyToken(username);
            var decodedModel = userEncodedTokenModel is null || userEncodedTokenModel.EncodedSpotifyToken is null ? null : new UserSpotifyTokenModel
            {
                Username = userEncodedTokenModel.Username,
                SpotifyToken = _securityService.DecryptString(userEncodedTokenModel.EncodedSpotifyToken, _spotifyOptions.ClientSecret)
            };

            return decodedModel;
        }
    }
}
