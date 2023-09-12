namespace SpotifyPlaylistJanitorAPI.Models.Spotify
{
    /// <summary>
    /// Model for Spotify Track.
    /// </summary>
    public class SpotifyTrackModel
    {
        /// <summary>
        /// Spotify track id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Spotify track name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// List of Spotify artists associated with the track.
        /// </summary>
        public IEnumerable<SpotifyArtistModel> Artists { get; set; } = new List<SpotifyArtistModel>();

        /// <summary>
        /// Spotify album associated with the track.
        /// </summary>
        public SpotifyAlbumModel? Album { get; set; }

        /// <summary>
        /// Track is local file.
        /// </summary>
        public bool IsLocal { get; set; }
    }
}
