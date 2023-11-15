using Quartz;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Jobs
{
    /// <summary>
    /// Job that polls users Spotify playback activity to monitor when skips occur.
    /// </summary>
    [DisallowConcurrentExecution]
    public class SpotifyPollingJob : IJob
    {
        private readonly ISpotifyService _spotifyService;
        private readonly IPlayingStateService _playingStateService;
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<SpotifyPollingJob> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyPollingJob"/> class.
        /// </summary>
        /// <param name="spotifyService">The Spotify Service.</param>
        /// <param name="playingStateService">The Playing State Service.</param>
        /// <param name="databaseService">The Database Service.</param>
        /// <param name="logger">The Application Logger.</param>
        public SpotifyPollingJob(ISpotifyService spotifyService,
            IPlayingStateService playingStateService,
            IDatabaseService databaseService,
            ILogger<SpotifyPollingJob> logger)
        {
            _spotifyService = spotifyService;
            _playingStateService = playingStateService;
            _databaseService = databaseService;
            _logger = logger;
        }

        /// <summary>
        /// Poll users current Spotify playback activity.
        /// </summary>
        /// <param name="context">Quartz Job Execution Context</param>
        public async Task Execute(IJobExecutionContext context)
        {
            if (_spotifyService.SpotifyClients.Keys.Count > 0)
            {
                foreach (var spotifyUsername in _spotifyService.SpotifyClients.Keys)
                {
                    var users = await _databaseService.GetUsers();
                    var user = users.First(us => us.SpotifyUsername == spotifyUsername);

                    var currentlyPlaying = await _spotifyService.GetCurrentPlayback(user.SpotifyUsername);

                    if (currentlyPlaying.IsPlaying && currentlyPlaying.Track is not null)
                    {
                        _logger.LogDebug($"{user.Username}: currently listening to playlist: {currentlyPlaying.Track.PlaylistId}");
                        _logger.LogDebug($"{user.Username}: currently playing: {currentlyPlaying.Track.Name}");

                        var databasePlaylist = await _databaseService.GetPlaylist(user.Username, currentlyPlaying.Track.PlaylistId);

                        if (databasePlaylist is not null)
                        {
                            var skip = _playingStateService.CheckSkipHasHappened(currentlyPlaying, databasePlaylist);
                            _playingStateService.UpdatePlayingState(currentlyPlaying);

                            if (skip)
                            {
                                if (!currentlyPlaying.Track.IsLocal)
                                {
                                    foreach (var artist in currentlyPlaying.Track.Artists)
                                    {
                                        var artistRequest = new DatabaseArtistModel
                                        {
                                            Id = artist.Id,
                                            Name = artist.Name,
                                            Href = artist.Href,
                                        };
                                        await _databaseService.AddArtist(artistRequest);
                                    }

                                    var albumRequest = new DatabaseAlbumRequest
                                    {
                                        Id = currentlyPlaying.Track.Album.Id,
                                        Name = currentlyPlaying.Track.Album.Name,
                                        Href = currentlyPlaying.Track.Album.Href,
                                    };
                                    await _databaseService.AddAlbum(albumRequest);

                                    var trackRequest = new DatabaseTrackModel
                                    {
                                        Id = currentlyPlaying.Track.Id,
                                        Name = currentlyPlaying.Track.Name,
                                        AlbumId = currentlyPlaying.Track.Album.Id,
                                        Length = currentlyPlaying.Track.Duration,
                                    };
                                    await _databaseService.AddTrack(trackRequest);

                                    //add artists to track
                                    //add artists to album
                                    foreach (var artist in currentlyPlaying.Track.Artists)
                                    {
                                        await _databaseService.AddArtistToTrack(artist.Id, currentlyPlaying.Track.Id);
                                        await _databaseService.AddArtistToAlbum(artist.Id, currentlyPlaying.Track.Album.Id);
                                    }

                                    //add images and add images to album
                                    foreach (var image in currentlyPlaying.Track.Album.Images)
                                    {
                                        var imageResult = await _databaseService.AddImage(image.Height, image.Width, image.Url);
                                        await _databaseService.AddImageToAlbum(imageResult.Id, currentlyPlaying.Track.Album.Id);
                                    }

                                    _logger.LogInformation($"{user.Username}: skipped track: {currentlyPlaying.Track?.Name} was playing from monitored playlist: {currentlyPlaying.Track?.PlaylistId}");

                                    var skippedTrack = new DatabaseSkippedTrackRequest
                                    {
                                        PlaylistId = currentlyPlaying.Track.PlaylistId,
                                        TrackId = currentlyPlaying.Track.Id,
                                        SkippedDate = DateTime.Now
                                    };
                                    await _databaseService.AddSkippedTrack(skippedTrack);
                                }
                                else
                                {
                                    _logger.LogDebug($"{user.Username}: local track: ${currentlyPlaying.Track?.Name} was skipped while playing from monitored playlist: {currentlyPlaying.Track?.PlaylistId}");
                                }
                            }
                        }
                    }
                    else
                    {
                        _logger.LogTrace($"{user.Username}: not currently listening to a monitored playlist");
                        _playingStateService.UpdatePlayingState(currentlyPlaying);
                    }
                }
            }
            else
            {
                _logger.LogTrace("No users currently logged into Spotify");
            }
        }
    }
}
