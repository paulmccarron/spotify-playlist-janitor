using SpotifyPlaylistJanitorAPI.Models.Database;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.SwaggerExamples.Database
{
    /// <summary>
    /// Swagger example.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DatabaseSkippedTrackModelExample : IExamplesProvider<IEnumerable<DatabaseSkippedTrackResponse>>
    {
        /// <summary>
        /// Implementation of Swashbuckle GetExamples() IExamplesProvider method.
        /// </summary>
        /// <returns>Example <see cref="IEnumerable{T}"/> of type <see cref="DatabasePlaylistModel"/>.</returns>
        public IEnumerable<DatabaseSkippedTrackResponse> GetExamples()
        {
            return new List<DatabaseSkippedTrackResponse> 
            {
                new DatabaseSkippedTrackResponse
                {
                    TrackId = "track_id_1",
                    PlaylistId = "playlist_id",
                    SkippedDate = new DateTime(),
                    Name = "Track 1",
                    Artists = new[]
                    {
                        new DatabaseArtistModel
                        {
                            Id = "artist_id_1",
                            Name = "Artist 1",
                            Href = "artist_url"
                        }
                    },
                    Album = new DatabaseAlbumResponse
                    {
                        Id = "album_id",
                        Href = "albim_url",
                        Name = "Album 1",
                        Images = new[]
                        {
                            new DatabaseImageModel
                            {
                                Id = 1, 
                                Height = 1,
                                Width = 1,
                                Url = "image_url_1"
                            }
                        }
                    },
                },
                new DatabaseSkippedTrackResponse
                {
                    TrackId = "track_id_2",
                    PlaylistId = "playlist_id",
                    SkippedDate = new DateTime(),
                    Name = "Track 2",
                    Artists = new[]
                    {
                        new DatabaseArtistModel
                        {
                            Id = "artist_id_2",
                            Name = "Artist 2",
                            Href = "artist_url"
                        }
                    },
                    Album = new DatabaseAlbumResponse
                    {
                        Id = "album_id",
                        Href = "albim_url",
                        Name = "Album 2",
                        Images = new[]
                        {
                            new DatabaseImageModel
                            {
                                Id = 2,
                                Height = 1,
                                Width = 1,
                                Url = "image_url_2"
                            }
                        }
                    },
                },
                new DatabaseSkippedTrackResponse
                {
                    TrackId = "track_id_3",
                    PlaylistId = "playlist_id",
                    SkippedDate = new DateTime(),
                    Name = "Track 3",
                    Artists = new[]
                    {
                        new DatabaseArtistModel
                        {
                            Id = "artist_id_1",
                            Name = "Artist 1",
                            Href = "artist_url"
                        }
                    },
                    Album = new DatabaseAlbumResponse
                    {
                        Id = "album_id",
                        Href = "albim_url",
                        Name = "Album 3",
                        Images = new[]
                        {
                            new DatabaseImageModel
                            {
                                Id = 3,
                                Height = 1,
                                Width = 1,
                                Url = "image_url_3"
                            }
                        }
                    },
                },
                new DatabaseSkippedTrackResponse
                {
                    TrackId = "track_id_4",
                    PlaylistId = "playlist_id",
                    SkippedDate = new DateTime(),
                    Name = "Track 4",
                    Artists = new[]
                    {
                        new DatabaseArtistModel
                        {
                            Id = "artist_id_3",
                            Name = "Artist 3",
                            Href = "artist_url_3"
                        }
                    },
                    Album = new DatabaseAlbumResponse
                    {
                        Id = "album_id",
                        Href = "albim_url",
                        Name = "Album 1",
                        Images = new[]
                        {
                            new DatabaseImageModel
                            {
                                Id = 4,
                                Height = 1,
                                Width = 1,
                                Url = "image_url_4"
                            }
                        }
                    },
                },
            };
        }
    }
}
