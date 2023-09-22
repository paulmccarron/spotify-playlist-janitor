using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpotifyPlaylistJanitorAPI.DataAccess.Entities;
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
        private const int refreshTokenExpiryHours = 1;
        private const int keySize = 64;
        private const int iterations = 350000;
        private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

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
            var storedUser = await _userService.GetUser(login.Email);

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
                var now = SystemTime.Now();
                var refreshToken = GenerateRefreshToken();

                jwt = new JWTModel
                {
                    AccessToken = GenerateJSONWebToken(user, now),
                    ExpiresIn = (int)TimeSpan.FromHours(refreshTokenExpiryHours).TotalMilliseconds,
                    RefreshToken = refreshToken,
                };

                var refreshTokenExpiry = now.AddHours(refreshTokenExpiryHours);

                await _userService.SetUserRefreshToken(login.Email, refreshToken, refreshTokenExpiry);
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
            var storedUser = await _userService.GetUser(login.Email);

            if (storedUser is not null)
            {
                return false;
            }

            var passwordHash = HashPasword(login.Password);

            await _userService.AddUser(login.Email, passwordHash);

            return true;
        }

        /// <summary>
        /// Re-authenticate user with token refresh request.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        ///<returns>Returns a <see cref = "JWTModel" />.</returns>
        public async Task<JWTModel?> RefreshUserToken(string accessToken, string refreshToken)
        {
            JWTModel? jwt = null;
            var principal = GetPrincipalFromToken(accessToken);

            if(principal is null || principal.Identity is null || string.IsNullOrWhiteSpace(principal.Identity.Name))
            {
                return jwt;
            }

            var username = principal.Identity.Name;

            var storedUser = await _userService.GetUser(username);

            if (storedUser is null || storedUser.RefreshToken != refreshToken || storedUser.RefreshTokenExpiry <= SystemTime.Now())
            {
                return jwt;
            }

            var user = new UserModel
            {
                Username = storedUser.UserName,
                Role = storedUser.IsAdmin ? "Admin" : "User",
            };

            var now = SystemTime.Now();
            var newRefreshToken = GenerateRefreshToken();

            jwt = new JWTModel
            {
                AccessToken = GenerateJSONWebToken(user, now),
                ExpiresIn = (int)TimeSpan.FromHours(refreshTokenExpiryHours).TotalMilliseconds,
                RefreshToken = newRefreshToken,
            };

            var refreshTokenExpiry = now.AddHours(refreshTokenExpiryHours);

            await _userService.SetUserRefreshToken(username, newRefreshToken, refreshTokenExpiry);

            return jwt;
        }

        /// <summary>
        /// Expire any refresh token assigned to user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task ExpireUserRefreshToken(string username)
        {
            var storedUser = await _userService.GetUser(username);

            if (storedUser is not null)
            {
                await _userService.ExpireUserRefreshToken(username);
            }
        }


        /// <summary>
        /// Generate a JSON Web Token for the supplied User Model.
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="now"></param>
        ///<returns>Returns a JWT <see cref = "string" />.</returns>
        private string GenerateJSONWebToken(UserModel userInfo, DateTime now)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_spotifyOptions.ClientSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userInfo.Username),
                new Claim(ClaimTypes.Role, userInfo.Role)
            };

            var token = new JwtSecurityToken(
                _spotifyOptions.ClientId,
                _spotifyOptions.ClientId,
                claims,
                expires: now.AddHours(refreshTokenExpiryHours),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generate a random string to use as a Refresh Token
        /// </summary>
        /// <returns></returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Hash the supplied password to safely store.
        /// </summary>
        /// <param name="password"></param>
        ///<returns>Returns a <see cref = "string" />.</returns>
        private string HashPasword(string password)
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
        private bool VerifyPassword(string password, string hashedPassword)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        private ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_spotifyOptions.ClientSecret));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, 
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
    }
}
