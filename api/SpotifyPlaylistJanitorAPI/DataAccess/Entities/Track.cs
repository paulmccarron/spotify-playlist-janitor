using System;
using System.Collections.Generic;

namespace SpotifyPlaylistJanitorAPI.DataAccess;

public partial class Track
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Length { get; set; }

    public string AlbumId { get; set; } = null!;

    public virtual Album Album { get; set; } = null!;

    public virtual ICollection<SkippedTrack> SkippedTracks { get; set; } = new List<SkippedTrack>();

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
}
