using System;
using System.Collections.Generic;

namespace SpotifyPlaylistJanitorAPI.DataAccess.Models;

public partial class SpotifyTrack
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Length { get; set; }

    public string SpotifyArtistId { get; set; } = null!;

    public string SpotifyAlbumId { get; set; } = null!;

    public virtual ICollection<SkippedTrack> SkippedTracks { get; set; } = new List<SkippedTrack>();

    public virtual SpotifyAlbum SpotifyAlbum { get; set; } = null!;

    public virtual SpotifyArtist SpotifyArtist { get; set; } = null!;
}
