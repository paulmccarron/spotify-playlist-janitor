using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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

            return;
        }
    }
}
