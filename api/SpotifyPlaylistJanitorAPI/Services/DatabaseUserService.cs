using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
        private readonly SpotifyOption _spotifyOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseUserService"/> class.
        /// </summary>
        /// <param name="databaseService">The Database Service.</param>
        /// <param name="spotifyOptions">The Spotify access credentials read from environment vars.</param>
        public DatabaseUserService(IDatabaseService databaseService, IOptions<SpotifyOption> spotifyOptions)
        {
            _databaseService = databaseService;
            _spotifyOptions = spotifyOptions.Value;
        }

        /// <summary>
        /// Returns user from database.
        /// </summary>
        /// <param name="username"></param>
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

        /// <summary>
        /// Store user spotify client token in database.
        /// </summary>
        public async Task AddUserSpotifyToken(string username, string? spotifyToken)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_spotifyOptions.ClientSecret);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(spotifyToken);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            var encrpytedString = Convert.ToBase64String(array);

            await _databaseService.AddUserEncodedSpotifyToken(username, encrpytedString);
        }

        /// <summary>
        /// Retrieve user spotify client token from database.
        /// </summary>
        public async Task<UserSpotifyTokenModel?> GetUserSpotifyToken(string username)
        {
            var userEncodedTokenModel = await _databaseService.GetUserEncodedSpotifyToken(username);
            UserSpotifyTokenModel? decodedModel = null;

            if (userEncodedTokenModel is not null)
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(userEncodedTokenModel.EncodedSpotifyToken);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(_spotifyOptions.ClientSecret);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                decodedModel = new UserSpotifyTokenModel
                                {
                                    Username = userEncodedTokenModel.Username,
                                    SpotifyToken = streamReader.ReadToEnd(),
                                };
                            }
                        }
                    }
                }
            }

            return decodedModel;
        }
    }
}
