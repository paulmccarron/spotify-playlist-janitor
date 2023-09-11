using System.ComponentModel.DataAnnotations;

namespace SpotifyPlaylistJanitorAPI.Models.Database
{
    /// <summary>
    /// Model for creating database Playlist.
    /// </summary>
    public class DatabasePlaylistRequest
    {
        /// <summary>
        /// Spotify playlist id.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public required string Id { get; set; }
    }
}
