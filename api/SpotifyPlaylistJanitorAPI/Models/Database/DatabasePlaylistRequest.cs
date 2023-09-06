using System.ComponentModel.DataAnnotations;

namespace SpotifyPlaylistJanitorAPI.Models.Database
{
    /// <summary>
    /// Model for creating database playlist.
    /// </summary>
    public class DatabasePlaylistRequest
    {
        /// <summary>
        /// Spotify playlist id.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public required string Id { get; set; }

        /// <summary>
        /// Spotify playlist name.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public required string Name { get; set; }

        /// <summary>
        /// Href link to Spotify playlist.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public required string Href { get; set; }
    }
}
