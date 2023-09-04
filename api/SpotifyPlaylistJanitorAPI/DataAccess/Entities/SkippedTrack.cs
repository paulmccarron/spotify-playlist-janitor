using System;
using System.Collections.Generic;

namespace SpotifyPlaylistJanitorAPI.DataAccess.Models;

public partial class SkippedTrack
{
    public int Id { get; set; }

    public int SkippedDate { get; set; }

    public string SpotifyPlaylistId { get; set; } = null!;

    public string SpotifyTrackId { get; set; } = null!;

    public virtual SpotifyPlaylist SpotifyPlaylist { get; set; } = null!;

    public virtual SpotifyTrack SpotifyTrack { get; set; } = null!;
}
