namespace SpotifyPlaylistJanitorAPI.DataAccess.Entities;

#pragma warning disable 1591
public partial class Image
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public int Height { get; set; }

    public int Width { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
}
