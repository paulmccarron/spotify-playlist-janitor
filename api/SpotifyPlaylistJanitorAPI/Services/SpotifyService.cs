using Newtonsoft.Json;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Exceptions;
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
        private readonly ISpotifyClientService _spotifyClientService;
        private readonly IUserService _userService;
        private Dictionary<string, ISpotifyClient> _spotifyClients { get; set; }

        /// <summary>
        /// Returns dictionary of Spotify Clients.
        /// </summary>
        public Dictionary<string, ISpotifyClient> SpotifyClients
        {
            get
            {
                return _spotifyClients;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyService"/> class.
        /// </summary>
        /// <param name="spotifyClientService">Service that impliments the <see cref="ISpotifyClientService"/> interface.</param>
        /// <param name="userService">Service that impliments the <see cref="IUserService"/> interface.</param>
        public SpotifyService(ISpotifyClientService spotifyClientService, IUserService userService)
        {
            _spotifyClientService = spotifyClientService;
            _userService = userService;
            _spotifyClients = new Dictionary<string, ISpotifyClient>();

            var users = _userService.GetUsers().Result;
            foreach (var user in users)
            {
                var spotifyToken = GetTokenFromStore(user.Username).Result;

                if (spotifyToken is not null)
                {
                    var spotifyClient = _spotifyClientService.CreateClient(spotifyToken, user.Username).Result;
                    if(spotifyClient is not null)
                    {
                        _spotifyClients.TryAdd(user.SpotifyUsername, spotifyClient);
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if service has a Spotify Client configured for user.
        /// </summary>
        public bool UserIsLoggedIn(string spotifyUsername)
        {
            return _spotifyClients.ContainsKey(spotifyUsername);
        }

        /// <summary>
        /// Create an instance a new instance of the <see cref="SpotifyClient"/> class and sets it to internal field.
        /// Makes an Oauth request to Spotify API using ClientId, ClientSecret,
        /// response code, and callbackUrl.
        /// </summary>
        /// <param name="code">Callback code provide by first part of the Authorization flow.</param>
        /// <param name="callbackUrl">Callback URL provide by first part of the Authorization flow.</param>
        /// <returns><see cref = "SpotifyClient" /> that is authenticated for users Spotify account.</returns>
        public async Task CreateClient(string code, string callbackUrl)
        {
            _spotifyClientService.CheckSpotifyCredentials();

            var spotifyClient = await _spotifyClientService.CreateClient(code, callbackUrl);

            if(spotifyClient is not null)
            {
                var currentUser = await spotifyClient.UserProfile.Current();
                _spotifyClients.TryAdd(currentUser.Email, spotifyClient);
            }
        }

        /// <summary>
        /// Returns current users details.
        /// </summary>
        /// <returns><see cref = "SpotifyUserModel" /> with user details</returns>
        /// <exception cref="SpotifyArgumentException">Thrown if service has no instance of <see cref="SpotifyClient"/> class.</exception>
        public async Task<SpotifyUserModel> GetUserDetails(string spotifyUsername)
        {
            var spotifyClient = GetClientFromDictionary(spotifyUsername);

            var currentUser = await spotifyClient.UserProfile.Current();

            return new SpotifyUserModel
            {
                Id = currentUser.Id,
                DisplayName = currentUser.DisplayName,
                Email = currentUser.Email,
                Href = currentUser.Href,
            };
        }

        /// <summary>
        /// Returns current users playlists.
        /// </summary>
        /// <returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "SpotifyPlaylistModel" />.</returns>
        /// <exception cref="SpotifyArgumentException">Thrown if service has no instance of <see cref="SpotifyClient"/> class.</exception>
        public async Task<IEnumerable<SpotifyPlaylistModel>> GetUserPlaylists(string spotifyUsername)
        {
            var spotifyClient = GetClientFromDictionary(spotifyUsername);

            var page = await spotifyClient.Playlists.CurrentUsers();
            var allPages = await spotifyClient.PaginateAll(page);
            var playlists = allPages
                .Select(playlist => new SpotifyPlaylistModel
                {
                    Id = playlist.Id,
                    Name = playlist.Name,
                    Href = playlist.ExternalUrls["spotify"],
                    Images = playlist.Images.Select(image => new SpotifyImageModel
                    {
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
        public async Task<SpotifyPlaylistModel?> GetUserPlaylist(string spotifyUsername, string id)
        {
            var spotifyClient = GetClientFromDictionary(spotifyUsername);

            try
            {

                var spotifyPlaylist = await spotifyClient.Playlists.Get(id);

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
            catch (APIException)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns tracks from current users playlist by id.
        /// </summary>
        ///<returns>Returns an <see cref="IEnumerable{T}" /> of type <see cref = "SpotifyTrackModel" />.</returns>
        /// <exception cref="SpotifyArgumentException"></exception>
        public async Task<IEnumerable<SpotifyTrackModel>> GetUserPlaylistTracks(string spotifyUsername, string id)
        {
            var spotifyClient = GetClientFromDictionary(spotifyUsername);

            var request = new PlaylistGetItemsRequest();
            request.Fields.Add("href,limit,next,offset,previous,total,items(track(name,type,id,artists(id,name,external_urls),album(id,name,external_urls,images),duration_ms,is_local))");

            var page = await spotifyClient.Playlists.GetItems(id, request);
            var allPages = await spotifyClient.PaginateAll(page);

            var tracks = allPages
                .Where(item => item.Track is FullTrack)
                .Select(item => (FullTrack)item.Track)
                .Select(track =>
                {
                    track.Album.ExternalUrls.TryGetValue("spotify", out string? albumHref);
                    return new SpotifyTrackModel
                    {
                        Id = track.Id,
                        Name = track.Name,
                        Artists = track.Artists.Select(artist =>
                        {
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
                            Id = track.Album.Id,
                            Name = track.Album.Name,
                            Href = albumHref,
                            Images = track.Album.Images.Select(image => new SpotifyImageModel
                            {
                                Height = image.Height,
                                Width = image.Width,
                                Url = image.Url,
                            })
                        },
                        Duration = track.DurationMs,
                        IsLocal = track.IsLocal,
                    };
                });

            return tracks;
        }

        /// <summary>
        /// Remove tracks from current users playlist.
        /// </summary>
        /// <param name="spotifyUsername">Spotify username.</param>
        /// <param name="playlistId">Id of Spotify playlist.</param>
        /// <param name="trackIds">Collection if track Ids to remove.</param>
        /// <returns></returns>
        public async Task<SnapshotResponse> DeletePlaylistTracks(string spotifyUsername, string playlistId, IEnumerable<string> trackIds)
        {
            var spotifyClient = GetClientFromDictionary(spotifyUsername);

            var request = new PlaylistRemoveItemsRequest
            {
                Tracks = trackIds
                    .Take(100)
                    .Select(trackId => new PlaylistRemoveItemsRequest.Item { Uri = $"spotify:track:{trackId}" })
                    .ToList()
            };

            return await spotifyClient.Playlists.RemoveItems(playlistId, request);
        }

        /// <summary>
        /// Returns current playback state.
        /// </summary>
        /// <returns><see cref = "SpotifyPlayingState" /> Current playback state.</returns>
        /// <exception cref="SpotifyArgumentException"></exception>
        public async Task<SpotifyPlayingState> GetCurrentPlayback(string spotifyUsername)
        {
            var spotifyClient = GetClientFromDictionary(spotifyUsername);

            var playingState = new SpotifyPlayingState
            {
                IsPlaying = false,
            };

            var currently = await spotifyClient.Player.GetCurrentPlayback();

            if (currently is not null)
            {
                playingState.IsPlaying = currently.IsPlaying
                    && currently.CurrentlyPlayingType.Equals("track")
                    && currently.Context.Type.Equals("playlist");

                if (playingState.IsPlaying && currently.Item is FullTrack track)
                {
                    track.Album.ExternalUrls.TryGetValue("spotify", out string? albumHref);
                    playingState.Track = new SpotifyCurrentlyPlayingTrackModel
                    {
                        Id = track.Id,
                        PlaylistId = currently.Context.Uri.Split(":").Last(),
                        ListeningOn = currently.Device.Name,
                        Name = track.Name,
                        Artists = track.Artists.Select(artist =>
                        {
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
                            Id = track.Album.Id,
                            Name = track.Album.Name,
                            Href = albumHref,
                            Images = track.Album.Images.Select(image => new SpotifyImageModel
                            {
                                Height = image.Height,
                                Width = image.Width,
                                Url = image.Url,
                            })
                        },
                        Duration = track.DurationMs,
                        Progress = currently.ProgressMs,
                        IsLocal = track.IsLocal,
                    };
                }
            }

            return playingState;
        }

        [ExcludeFromCodeCoverage]
        private async Task<AuthorizationCodeTokenResponse?> GetTokenFromStore(string user)
        {
            var tokenString = await _userService.GetUserSpotifyToken(user);
            return tokenString is null ? null : JsonConvert.DeserializeObject<AuthorizationCodeTokenResponse>(tokenString.SpotifyToken);
        }

        private ISpotifyClient GetClientFromDictionary(string spotifyUsername)
        {
            ISpotifyClient spotifyClient;
            if (!_spotifyClients.TryGetValue(spotifyUsername, out spotifyClient))
            {
                throw new SpotifyArgumentException("No Spotify Client configured");
            }

            return spotifyClient;
        }
    }
}
