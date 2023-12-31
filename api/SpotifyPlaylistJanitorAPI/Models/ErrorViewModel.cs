using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.Models
{
    /// <summary>
    /// Error view model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ErrorViewModel
    {
        /// <summary>
        /// Request Id
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Show Request Id
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}