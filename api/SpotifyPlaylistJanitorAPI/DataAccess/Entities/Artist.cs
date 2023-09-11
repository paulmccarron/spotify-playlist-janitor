namespace SpotifyPlaylistJanitorAPI.DataAccess;

#pragma warning disable 1591
public partial class Artist
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Href { get; set; } = null!;

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
