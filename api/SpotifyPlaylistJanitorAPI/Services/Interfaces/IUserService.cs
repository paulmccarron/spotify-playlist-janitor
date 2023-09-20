﻿using SpotifyPlaylistJanitorAPI.Models.Auth;

namespace SpotifyPlaylistJanitorAPI.Services.Interfaces
{
    /// <summary>
    /// Service that handles user information from store.
    /// Interface introduced to simplify unit testing as well as to provide flexibility for future.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Returns user from store.
        /// </summary>
        ///<returns>Returns a <see cref = "UserDataModel" />.</returns>
        Task<UserDataModel?> GetUser(string username);

        /// <summary>
        /// Adds user to store.
        /// </summary>
        Task AddUser(string username, string passwordHash);

        /// <summary>
        /// Set user refresh token in store.
        /// </summary>
        Task SetUserRefreshToken(string username, string refreshToken, DateTime tokenExpiry);

        /// <summary>
        /// Remove user refresh token from store.
        /// </summary>
        Task ExpireUserRefreshToken(string username);
    }
}
