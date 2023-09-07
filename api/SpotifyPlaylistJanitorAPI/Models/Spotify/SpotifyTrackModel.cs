namespace SpotifyPlaylistJanitorAPI.Models.Spotify
{
    public class SpotifyTrackModel
    {
        public string? Id { get; set; }

        public string? PlaylistId { get; set; }

        public string? ListeningOn { get; set; }

        public string? Name { get; set; }

        public IEnumerable<SpotifyArtistModel> Artists { get; set; }

        public SpotifyAlbumModel? Album { get; set; }

        public int? Duration { get; set; }

        public int? Progress { get; set; }
    }
}
