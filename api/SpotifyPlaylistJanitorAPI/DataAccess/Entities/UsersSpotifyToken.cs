namespace SpotifyPlaylistJanitorAPI.DataAccess.Entities;

#pragma warning disable 1591
public partial class UsersSpotifyToken
{
    public string Username { get; set; } = null!;

    public string? EncodedSpotifyToken { get; set; } = null!;

    public virtual User UsernameNavigation { get; set; } = null!;
}
