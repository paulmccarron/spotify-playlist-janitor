using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Services
{
    /// <summary>
    /// Service that handles user information from application database.
    /// </summary>
    public class DatabaseUserService : IUserService
    {
        private readonly IDatabaseService _databaseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseUserService"/> class.
        /// </summary>
        /// <param name="databaseService">The Database Service.</param>
        public DatabaseUserService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
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
        public async Task AddUser(string username, string passwordHash)
        {
            await _databaseService.AddUser(username, passwordHash);
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
    }
}
