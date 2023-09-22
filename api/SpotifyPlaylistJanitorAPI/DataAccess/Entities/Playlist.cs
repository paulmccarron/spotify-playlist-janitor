namespace SpotifyPlaylistJanitorAPI.DataAccess.Entities;

#pragma warning disable 1591
public partial class Playlist
{
    public string Id { get; set; } = null!;

    public int? SkipThreshold { get; set; }

    public bool IgnoreInitialSkips { get; set; }

    public int? AutoCleanupLimit { get; set; }

    public virtual ICollection<SkippedTrack> SkippedTracks { get; set; } = new List<SkippedTrack>();
}
