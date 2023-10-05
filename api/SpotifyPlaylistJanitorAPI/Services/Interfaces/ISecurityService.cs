using Microsoft.IdentityModel.Tokens;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SpotifyPlaylistJanitorAPI.Services.Interfaces
{
    /// <summary>
    /// Service that handling security artifacts i.e Web Tokens, Encrypted passwords, encrytable/decryptable strings.
    /// Interface introduced to simplify unit testing as well as to provide flexibility for future.
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Generate a JSON Web Token for the supplied User Model.
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="now"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <param name="key"></param>
        ///<returns>Returns a JWT <see cref = "string" />.</returns>
        string GenerateJSONWebToken(UserModel userInfo, DateTime now, string issuer, string audience, string key);

        /// <summary>
        /// Generate a random string to use as a Refresh Token
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Hash the supplied password to safely store.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key">Encrpytion key.</param>
        ///<returns>Returns a <see cref = "string" />.</returns>
        string HashPasword(string password, string key);

        /// <summary>
        /// Compare supplied password to stored hashed password.
        /// </summary>
        /// <param name="password">User password.</param>
        /// <param name="hashedPassword">Stored hashed password to compare.</param>
        /// <param name="key">Encrpytion key.</param>
        ///<returns>Returns a <see cref = "bool" />.</returns>
        bool VerifyPassword(string password, string hashedPassword, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="key">Encrpytion key.</param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        ClaimsPrincipal? GetPrincipalFromToken(string token, string key);

        /// <summary>
        /// Encrypt a string using a supplied key.
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string EncryptString(string plainText, string key);

        /// <summary>
        /// Decrypt a string using a supplied key.
        /// </summary>
        /// <param name="secureString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string DecryptString(string secureString, string key);
    }
}
