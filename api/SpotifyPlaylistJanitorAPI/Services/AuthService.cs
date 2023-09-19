using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using SpotifyPlaylistJanitorAPI.System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SpotifyPlaylistJanitorAPI.Services
{
    /// <summary>
    /// Service that handles user authentication functionality.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly SpotifyOption _spotifyOptions;
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userService">Service that impliments the <see cref="IUserService"/> interface.</param>
        /// <param name="spotifyOptions">The Spotify access credentials read from environment vars.</param>
        public AuthService(IUserService userService, IOptions<SpotifyOption> spotifyOptions)
        {
            _userService = userService;
            _spotifyOptions = spotifyOptions.Value;
        }

        /// <summary>
        /// Authenticate user login request.
        /// </summary>
        /// <param name="login"></param>
        ///<returns>Returns a <see cref = "JWTModel" />.</returns>
        public async Task<JWTModel?> AuthenticateUser(UserLoginRequest login)
        {
            JWTModel? jwt = null;
            UserModel? user = null;
            var storedUser = await _userService.GetUser(login.Username);

            if(storedUser is not null)
            {
                var passwordValid = VerifyPassword(login.Password, storedUser.PasswordHash);

                if (passwordValid)
                {
                    user = new UserModel 
                    { 
                        Username = storedUser.UserName, 
                        Role = storedUser.IsAdmin ? "Admin" : "User",
                    };
                }
            }

            if (user is not null)
            {
                jwt = new JWTModel
                {
                    Token = GenerateJSONWebToken(user)
                };
            }

            return jwt;
        }

        /// <summary>
        /// Register user with application.
        /// </summary>
        /// <param name="login"></param>
        ///<returns>Returns a <see cref = "bool" /> signifying successfully registering requested user.</returns>
        public async Task<bool> RegisterUser(UserLoginRequest login)
        {
            var storedUser = await _userService.GetUser(login.Username);

            if (storedUser is not null)
            {
                return false;
            }

            var passwordHash = HashPasword(login.Password);

            await _userService.AddUser(login.Username, passwordHash);

            return true;
        }

        /// <summary>
        /// Generate a JSON Web Token for the supplied User Model.
        /// </summary>
        /// <param name="userInfo"></param>
        ///<returns>Returns a JWT <see cref = "string" />.</returns>
        public string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_spotifyOptions.ClientSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userInfo.Username),
                new Claim(ClaimTypes.Role,userInfo.Role)
            };

            var token = new JwtSecurityToken(
                _spotifyOptions.ClientId,
                _spotifyOptions.ClientId,
                claims,
                expires: SystemTime.Now().AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Hash the supplied password to safely store.
        /// </summary>
        /// <param name="password"></param>
        ///<returns>Returns a <see cref = "string" />.</returns>
        public string HashPasword(string password)
        {
            var salt = Encoding.ASCII.GetBytes(_spotifyOptions.ClientSecret);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

        /// <summary>
        /// Compare supplied password to stored hashed password.
        /// </summary>
        /// <param name="password">User password.</param>
        /// <param name="hashedPassword">Stored hashed password to compare.</param>
        ///<returns>Returns a <see cref = "bool" />.</returns>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            var salt = Encoding.ASCII.GetBytes(_spotifyOptions.ClientSecret);
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
                password, 
                salt, 
                iterations, 
                hashAlgorithm, 
                keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hashedPassword));
        }
    }
}
