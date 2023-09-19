using SpotifyPlaylistJanitorAPI.Models.Spotify;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.SwaggerExamples.Spotify
{
    /// <summary>
    /// Swagger example.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SpotifyTracksModelExample : IExamplesProvider<IEnumerable<SpotifyTrackModel>>
    {
        /// <summary>
        /// Implementation of Swashbuckle GetExamples() IExamplesProvider method.
        /// </summary>
        /// <returns>Example <see cref="IEnumerable{T}"/> of type <see cref="SpotifyTrackModel"/>.</returns>
        public IEnumerable<SpotifyTrackModel> GetExamples()
        {
            return new[]
            {
                new SpotifyTrackModel
                {
                    Id = "track_id_1",
                    Name = "Track Name 2",
                    Artists = new[]
                    {
                        new SpotifyArtistModel
                        {
                            Id = "artist_id_1",
                            Name = "Artist Name 1",
                            Href = "https://open.spotify.com/artist/artist_id_1",
                        }
                    },
                    Album = new SpotifyAlbumModel
                    {
                        Id = "album_id_1",
                        Name = "Album Name 1",
                        Href = "https://open.spotify.com/album/album_id_1",
                        Images = new[]
                        {
                            new SpotifyImageModel
                            {
                                Height = 200,
                                Width = 200,
                                Url = "https://play-lh.googleusercontent.com/cShys-AmJ93dB0SV8kE6Fl5eSaf4-qMMZdwEDKI5VEmKAXfzOqbiaeAsqqrEBCTdIEs"
                            }
                        },
                    },
                    Duration = 100000,
                    IsLocal = false,
                },
                new SpotifyTrackModel
                {
                    Id = "track_id_2",
                    Name = "Track Name 2",
                    Artists = new[]
                    {
                        new SpotifyArtistModel
                        {
                            Id = "artist_id_2",
                            Name = "Artist Name 2",
                            Href = "https://open.spotify.com/artist/artist_id_2",
                        }
                    },
                    Album = new SpotifyAlbumModel
                    {
                        Id = "album_id_2",
                        Name = "Album Name 2",
                        Href = "https://open.spotify.com/album/album_id_2",
                        Images = new[]
                        {
                            new SpotifyImageModel
                            {
                                Height = 200,
                                Width = 200,
                                Url = "https://play-lh.googleusercontent.com/cShys-AmJ93dB0SV8kE6Fl5eSaf4-qMMZdwEDKI5VEmKAXfzOqbiaeAsqqrEBCTdIEs"
                            }
                        },
                    },
                    Duration = 100000,
                    IsLocal = true,
                }
            };
        }
    }
}
