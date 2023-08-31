using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.Exceptions
{
    /// <summary>
    /// Exception to throw when Spotify credentials are not provided
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class SpotifyArgumentException : Exception
    {
        /// <summary>
        /// Overide contructor
        /// </summary>
        /// <param name="message">Error message</param>
        public SpotifyArgumentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Overide contructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public SpotifyArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
