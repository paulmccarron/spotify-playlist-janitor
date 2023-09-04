namespace SpotifyPlaylistJanitorAPI.DataAccess.Models;

#pragma warning disable 1591
public partial class SpotifyArtist
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Href { get; set; } = null!;

    public virtual ICollection<SpotifyTrack> SpotifyTracks { get; set; } = new List<SpotifyTrack>();
}
