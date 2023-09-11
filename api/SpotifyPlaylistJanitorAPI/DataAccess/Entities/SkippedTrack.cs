namespace SpotifyPlaylistJanitorAPI.DataAccess.Entities;

#pragma warning disable 1591
public partial class SkippedTrack
{
    public int Id { get; set; }

    public DateTime? SkippedDate { get; set; }

    public string PlaylistId { get; set; } = null!;

    public string TrackId { get; set; } = null!;

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual Track Track { get; set; } = null!;
}
