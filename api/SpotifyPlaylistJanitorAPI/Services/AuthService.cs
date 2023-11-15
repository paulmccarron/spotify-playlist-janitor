using Microsoft.Extensions.Options;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using SpotifyPlaylistJanitorAPI.System;

namespace SpotifyPlaylistJanitorAPI.Services
{
    /// <summary>
    /// Service that handles user authentication functionality.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ISecurityService _securityService;
        private readonly SpotifyOption _spotifyOptions;
        private const int refreshTokenExpiryHours = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userService">Service that impliments the <see cref="IUserService"/> interface.</param>
        /// <param name="securityService">Service that impliments the <see cref="ISecurityService"/> interface.</param>
        /// <param name="spotifyOptions">The Spotify access credentials read from environment vars.</param>
        public AuthService(IUserService userService, ISecurityService securityService, IOptions<SpotifyOption> spotifyOptions)
        {
            _userService = userService;
            _securityService = securityService;
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

            if (storedUser is not null)
            {
                var passwordValid = _securityService.VerifyPassword(login.Password, storedUser.PasswordHash, _spotifyOptions.ClientSecret);

                if (passwordValid)
                {
                    user = new UserModel
                    {
                        Username = storedUser.Username,
                        SpotifyUsername = storedUser.SpotifyUsername,
                        Role = storedUser.IsAdmin ? "Admin" : "User",
                    };
                }
            }

            if (user is not null)
            {
                var now = SystemTime.Now();
                var refreshToken = _securityService.GenerateRefreshToken();

                jwt = new JWTModel
                {
                    AccessToken = _securityService.GenerateJSONWebToken(user, now, _spotifyOptions.ClientId, _spotifyOptions.ClientId, _spotifyOptions.ClientSecret),
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
        public async Task<bool> RegisterUser(UserRegisterRequest login)
        {
            var storedUser = await _userService.GetUser(login.Email);

            if (storedUser is not null)
            {
                return false;
            }

            var passwordHash = _securityService.HashPasword(login.Password, _spotifyOptions.ClientSecret);

            await _userService.AddUser(login.Email, login.SpotifyEmail, passwordHash);

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
            var principal = _securityService.GetPrincipalFromToken(accessToken, _spotifyOptions.ClientSecret);

            if (principal is null || principal.Identity is null || string.IsNullOrWhiteSpace(principal.Identity.Name))
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
                Username = storedUser.Username,
                SpotifyUsername = storedUser.SpotifyUsername,
                Role = storedUser.IsAdmin ? "Admin" : "User",
            };

            var now = SystemTime.Now();
            var newRefreshToken = _securityService.GenerateRefreshToken();

            jwt = new JWTModel
            {
                AccessToken = _securityService.GenerateJSONWebToken(user, now, _spotifyOptions.ClientId, _spotifyOptions.ClientId, _spotifyOptions.ClientSecret),
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
    }
}
