namespace SpotifyPlaylistJanitorAPI.Models.Spotify
{
    public class SpotifyPlayingState
    {
        public bool IsPlaying { get; set; }
        public SpotifyTrackModel? Track { get; set; }
    }
}
