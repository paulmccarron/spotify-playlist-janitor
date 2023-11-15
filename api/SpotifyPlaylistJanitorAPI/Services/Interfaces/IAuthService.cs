using SpotifyPlaylistJanitorAPI.Models.Auth;

namespace SpotifyPlaylistJanitorAPI.Services.Interfaces
{
    /// <summary>
    /// Service that handles user authentication functionality.
    /// Interface introduced to simplify unit testing as well as to provide flexibility for future.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticate user login request.
        /// </summary>
        /// <param name="login"></param>
        ///<returns>Returns a <see cref = "JWTModel" />.</returns>
        Task<JWTModel?> AuthenticateUser(UserLoginRequest login);

        /// <summary>
        /// Register user with application.
        /// </summary>
        /// <param name="login"></param>
        ///<returns>Returns a <see cref = "bool" /> signifying successfully registering requested user.</returns>
        Task<bool> RegisterUser(UserRegisterRequest login);

        /// <summary>
        /// Re-authenticate user with token refresh request.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        ///<returns>Returns a <see cref = "JWTModel" />.</returns>
        Task<JWTModel?> RefreshUserToken(string accessToken, string refreshToken);

        /// <summary>
        /// Expire any refresh token assigned to user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task ExpireUserRefreshToken(string username);
    }
}
