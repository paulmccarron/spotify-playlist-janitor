namespace SpotifyPlaylistJanitorAPI.Models.Database
{
    /// <summary>
    /// Model for Database album information.
    /// </summary>
    public class DatabaseImageModel
    {
        /// <summary>
        /// Id of image.
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// Height of image.
        /// </summary>
        public required int Height { get; set; }

        /// <summary>
        /// Width of image.
        /// </summary>
        public required int Width { get; set; }

        /// <summary>
        /// Url to image.
        /// </summary>
        public required string Url { get; set; }
    }
}
