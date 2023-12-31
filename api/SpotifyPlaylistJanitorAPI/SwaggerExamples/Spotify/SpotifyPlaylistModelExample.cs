﻿using SpotifyPlaylistJanitorAPI.Models.Spotify;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.SwaggerExamples.Spotify
{
    /// <summary>
    /// Swagger example.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SpotifyPlaylistModelExample : IExamplesProvider<SpotifyPlaylistModel>
    {
        /// <summary>
        /// Implementation of Swashbuckle GetExamples() IExamplesProvider method.
        /// </summary>
        /// <returns>Example <see cref="SpotifyPlaylistModel"/>.</returns>
        public SpotifyPlaylistModel GetExamples()
        {
            return new SpotifyPlaylistModel
            {
                Id = "playlist_id",
                Name = "Playlist Name",
                Href = "https://open.spotify.com/playlist/playlist_id",
                Images = new[]
                {
                    new SpotifyImageModel
                    {
                        Height = 200,
                        Width = 200,
                        Url = "https://play-lh.googleusercontent.com/cShys-AmJ93dB0SV8kE6Fl5eSaf4-qMMZdwEDKI5VEmKAXfzOqbiaeAsqqrEBCTdIEs"
                    }
                }
            };
        }
    }
}
