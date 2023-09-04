using System;
using System.Collections.Generic;

namespace SpotifyPlaylistJanitorAPI.DataAccess.Models;

public partial class SpotifyAlbum
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Href { get; set; } = null!;

    public virtual ICollection<SpotifyTrack> SpotifyTracks { get; set; } = new List<SpotifyTrack>();
}
