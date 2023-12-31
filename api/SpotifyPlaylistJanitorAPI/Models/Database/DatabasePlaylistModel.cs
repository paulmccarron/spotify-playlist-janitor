﻿namespace SpotifyPlaylistJanitorAPI.Models.Database
{
    /// <summary>
    /// Model for database Playlist information.
    /// </summary>
    public class DatabasePlaylistModel
    {
        /// <summary>
        /// Spotify playlist id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Skip threshold for playlist tracks in seconds.
        /// </summary>
        public int? SkipThreshold { get; set; }

        /// <summary>
        /// Ignore intial skips for playlist playback.
        /// </summary>
        public bool IgnoreInitialSkips { get; set; }

        /// <summary>
        /// Auto track-cleanup limit for playlist tracks.
        /// </summary>
        public int? AutoCleanupLimit { get; set; }
    }
}
