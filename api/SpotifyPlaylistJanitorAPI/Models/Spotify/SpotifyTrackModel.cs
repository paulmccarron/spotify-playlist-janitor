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
        public string? Name { get; set; }

        /// <summary>
        /// Spotify playlist id.
        /// </summary>
        public string? PlaylistId { get; set; }

        /// <summary>
        /// List of Spotify artists associated with the track.
        /// </summary>
        public IEnumerable<SpotifyArtistModel> Artists { get; set; } = new List<SpotifyArtistModel>();

        /// <summary>
        /// Spotify album associated with the track.
        /// </summary>
        public SpotifyAlbumModel? Album { get; set; }

        /// <summary>
        /// Length of the song in milliseconds.
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// Playback progress of the song in milliseconds.
        /// </summary>
        public int? Progress { get; set; }

        /// <summary>
        /// Device track is being played on.
        /// </summary>
        public string? ListeningOn { get; set; }
    }
}
