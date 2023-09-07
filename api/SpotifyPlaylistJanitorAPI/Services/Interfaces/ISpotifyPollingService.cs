namespace SpotifyPlaylistJanitorAPI.Services.Interfaces
{
    public interface ISpotifyPollingService
    {
        void PollSpotifyPlayback(object? state);
    }
}
