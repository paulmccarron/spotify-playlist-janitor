using SpotifyAPI.Web;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.Models
{
    /// <summary>
    /// Generic response model for API error examples
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ErrorResponseModel
    {
        /// <summary>
        /// Message for the error.
        /// </summary>
        public string? Message { get; set; }
    }
}
