namespace SpotifyPlaylistJanitorAPI.DataAccess.Entities;

#pragma warning disable 1591
public partial class Album
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Href { get; set; } = null!;

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}
