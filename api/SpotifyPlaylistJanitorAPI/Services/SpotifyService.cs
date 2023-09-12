using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Exceptions;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models.Spotify;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.Services
{
    /// <summary>
    /// Service to interact with Spotify API.
    /// </summary>
    public class SpotifyService : ISpotifyService
    {
        private readonly SpotifyOption _spotifyOptions;
        private ISpotifyClient? _spotifyClient { get; set; }

        /// <summary>
        /// Sets the internal Spotify Client fro the service.
        /// </summary>
        /// <param name="spotifyClient">The Spotify access credentials read from environment vars.</param>
        public void SetClient(ISpotifyClient? spotifyClient)
        {
            _spotifyClient = spotifyClient;
        }

        /// <summary>
        /// Returns true if service has a Spotify Client configured.
        /// </summary>
        public bool IsLoggedIn
        {
            get
            {
                return _spotifyClient != null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyService"/> class.
        /// </summary>
        /// <param name="spotifyOptions">The Spotify access credentials read from environment vars.</param>
        public SpotifyService(IOptions<SpotifyOption> spotifyOptions)
        {
            _spotifyOptions = spotifyOptions.Value;
        }

        /// <summary>
        /// Create an instance a new instance of the <see cref="SpotifyClient"/> class.
        /// Makes an Oauth request to Spotify API using ClientId, ClientSecret,
        /// response code, and callbackUrl.
        /// </summary>
        /// <param name="code">Callback code provide by first part of the Authorization flow.</param>
        /// <param name="callbackUrl">Callback URL provide by first part of the Authorization flow.</param>
        /// <returns><see cref = "SpotifyClient" /> that is authenticated for users Spotify account.</returns>
        [ExcludeFromCodeCoverage]
        public async Task<ISpotifyClient> CreateClient(string code, string callbackUrl)
        {
            CheckSpotifyCredentials();

            var response = await new OAuthClient().RequestToken(
              new AuthorizationCodeTokenRequest(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret, code, new Uri(callbackUrl))
            );

            var config = SpotifyClientConfig
              .CreateDefault()
              .WithAuthenticator(new AuthorizationCodeAuthenticator(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret, response));

            return new SpotifyClient(config);
        }

        /// <summary>
        /// Returns current users details.
        /// </summary>
        /// <returns><see cref = "SpotifyUserModel" /> with user details</returns>
        /// <exception cref="SpotifyArgumentException">Thrown if service has no instance of <see cref="SpotifyClient"/> class.</exception>
        public async Task<SpotifyUserModel> GetUserDetails()
        {
            if (_spotifyClient is null)
            {
                throw new SpotifyArgumentException("No Spotify Client configured");
            }
            
            var currentUser =  await _spotifyClient.UserProfile.Current();

            return new SpotifyUserModel
            {
                Id = currentUser.Id,
                DisplayName = currentUser.DisplayName,
                Email = currentUser.Email,
                Href    = currentUser.Href,
            };
        }

        /// <summary>
        /// Returns current users playlists.
        /// </summary>
        /// <returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "SpotifyPlaylistModel" />.</returns>
        /// <exception cref="SpotifyArgumentException">Thrown if service has no instance of <see cref="SpotifyClient"/> class.</exception>
        public async Task<IEnumerable<SpotifyPlaylistModel>> GetUserPlaylists()
        {
            if (_spotifyClient is null)
            {
                throw new SpotifyArgumentException("No Spotify Client configured");
            }

            var request = new PlaylistCurrentUsersRequest
            {
                Limit = 50
            };

            var page = await _spotifyClient.Playlists.CurrentUsers(request);
            var allPages = await _spotifyClient.PaginateAll(page);
            var playlists = allPages
                .Select(playlist => new SpotifyPlaylistModel
                {
                    Id = playlist.Id,
                    Name = playlist.Name,
                    Href = playlist.ExternalUrls["spotify"],
                    Images = playlist.Images.Select(image => new SpotifyImageModel {
                        Height = image.Height,
                        Width = image.Width,
                        Url = image.Url,
                    })
                })
                .OrderBy(playlist => playlist.Name?.ToLower());

            return playlists;
        }

        /// <summary>
        /// Returns current users playlist by id.
        /// </summary>
        /// <returns><see cref = "SpotifyPlaylistModel" /></returns>
        /// <exception cref="SpotifyArgumentException">Thrown if service has no instance of <see cref="SpotifyClient"/> class.</exception>
        public async Task<SpotifyPlaylistModel?> GetUserPlaylist(string id)
        {
            if (_spotifyClient is null)
            {
                throw new SpotifyArgumentException("No Spotify Client configured");
            }

            try
            {

                var spotifyPlaylist = await _spotifyClient.Playlists.Get(id);

                var playlistModel = new SpotifyPlaylistModel
                {
                    Id = spotifyPlaylist?.Id,
                    Name = spotifyPlaylist?.Name,
                    Href = spotifyPlaylist?.ExternalUrls["spotify"],
                    Images = spotifyPlaylist?.Images?.Select(image => new SpotifyImageModel
                    {
                        Height = image.Height,
                        Width = image.Width,
                        Url = image.Url,
                    })
                };

                return playlistModel;
            }
            catch(APIException)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns tracks from current users playlist by id.
        /// </summary>
        ///<returns>Returns an <see cref="IEnumerable{T}" /> of type <see cref = "SpotifyTrackModel" />.</returns>
        /// <exception cref="SpotifyArgumentException"></exception>
        public async Task<IEnumerable<SpotifyTrackModel>> GetUserPlaylistTracks(string id)
        {
            if (_spotifyClient is null)
            {
                throw new SpotifyArgumentException("No Spotify Client configured");
            }

            var request = new PlaylistGetItemsRequest
            {
                Limit = 50,
            };
            //request.Fields.Add("items(track(id,name,artists(id,name,external_urls),album(id,name,external_urls,images)),is_local)");
            request.Fields.Add("items(track(id),is_local)");

            var thing = request.BuildQueryParams();

            var page = await _spotifyClient.Playlists.GetItems(id, request);
            var allPages = await _spotifyClient.PaginateAll(page);

            var tracks = allPages
                .Select(track => {
                    var item = (FullTrack)track.Track;
                    item.Album.ExternalUrls.TryGetValue("spotify", out string? albumHref);
                    return new SpotifyTrackModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Artists = item.Artists.Select(artist => {
                            artist.ExternalUrls.TryGetValue("spotify", out string? artistHref);
                            return new SpotifyArtistModel
                            {
                                Name = artist.Name,
                                Id = artist.Id,
                                Href = artistHref,
                            };
                        }),
                        Album = new SpotifyAlbumModel
                        {
                            Id = item.Album.Id,
                            Name = item.Album.Name,
                            Href = albumHref,
                            Images = item.Album.Images.Select(image => new SpotifyImageModel
                            {
                                Height = image.Height,
                                Width = image.Width,
                                Url = image.Url,
                            })
                        },
                        IsLocal = item.IsLocal,
                    };
                });

            return tracks;
        }

        /// <summary>
        /// Returns current playback state.
        /// </summary>
        /// <returns><see cref = "SpotifyPlayingState" /> Current playback state.</returns>
        /// <exception cref="SpotifyArgumentException"></exception>
        public async Task<SpotifyPlayingState> GetCurrentPlayback()
        {
            if (_spotifyClient is null)
            {
                throw new SpotifyArgumentException("No Spotify Client configured");
            }

            var playingState = new SpotifyPlayingState
            {
                IsPlaying = false,
            };

            var currently = await _spotifyClient.Player.GetCurrentPlayback();

            if (currently is not null)
            {
                playingState.IsPlaying = currently.IsPlaying
                    && currently.CurrentlyPlayingType.Equals("track")
                    && currently.Context.Type.Equals("playlist");

                if (playingState.IsPlaying)
                {
                    var item = (FullTrack)currently.Item;
                    item.Album.ExternalUrls.TryGetValue("spotify", out string? albumHref);
                    playingState.Track = new SpotifyCurrentlyPlayingTrackModel
                    {
                        Id = item.Id,
                        PlaylistId = currently.Context.Uri.Split(":").Last(),
                        ListeningOn = currently.Device.Name,
                        Name = item.Name,
                        Artists = item.Artists.Select(artist => {
                            artist.ExternalUrls.TryGetValue("spotify", out string? artistHref);
                            return new SpotifyArtistModel
                            {
                                Name = artist.Name,
                                Id = artist.Id,
                                Href = artistHref,
                            };
                        }),
                        Album = new SpotifyAlbumModel
                        {
                            Id = item.Album.Id,
                            Name = item.Album.Name,
                            Href = albumHref,
                            Images = item.Album.Images.Select(image => new SpotifyImageModel
                            {
                                Height = image.Height,
                                Width = image.Width,
                                Url = image.Url,
                            })
                        },
                        Duration = item.DurationMs,
                        Progress = currently.ProgressMs,
                        IsLocal = item.IsLocal,
                    };
                }
            }

            return playingState;
        }

        /// <summary>
        /// Throws exception if there are any missing Spotify credentials from environment config.
        /// </summary>
        /// <exception cref="SpotifyArgumentException"></exception>
        public void CheckSpotifyCredentials()
        {
            var clientIdEmpty = string.IsNullOrWhiteSpace(_spotifyOptions.ClientId);
            var clientSecretEmpty = string.IsNullOrWhiteSpace(_spotifyOptions.ClientSecret);
            if (clientIdEmpty && clientSecretEmpty){
                throw new SpotifyArgumentException("No Spotify ClientId or ClientSecret configured");
            }
            if (clientIdEmpty)
            {
                throw new SpotifyArgumentException("No Spotify ClientId configured");
            }
            if (clientSecretEmpty)
            {
                throw new SpotifyArgumentException("No Spotify ClientSecret configured");
            }
        }
    }
}
