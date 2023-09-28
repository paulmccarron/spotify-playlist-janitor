namespace SpotifyPlaylistJanitorAPI.DataAccess.Entities;

#pragma warning disable 1591
public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsAdmin { get; set; }

    public string? RefreshToken { get; set; } = null;

    public DateTime? RefreshTokenExpiry { get; set; } = null;

    public virtual UsersSpotifyToken? UsersSpotifyToken { get; set; }
}
