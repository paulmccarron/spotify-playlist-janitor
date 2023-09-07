using SpotifyPlaylistJanitorAPI.Models.Spotify;

namespace SpotifyPlaylistJanitorAPI.Services.Interfaces
{
    public interface IPlayingStateService
    {
        SpotifyPlayingState PlayingState { get; set; }

        bool CheckSkipHasHappened(SpotifyPlayingState newPlayingState);
    }
}
