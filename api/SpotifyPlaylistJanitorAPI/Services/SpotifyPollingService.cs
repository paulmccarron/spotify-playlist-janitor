using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class SpotifyPollingService : IHostedService, IDisposable, ISpotifyPollingService
    {
        private int executionCount = 0;
        private readonly ISpotifyService _spotifyService;
        private readonly IPlayingStateService _playingStateService;
        private readonly ILogger<SpotifyPollingService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer? _timer = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spotifyService"></param>
        /// <param name="playingStateService"></param>
        /// <param name="logger"></param>
        /// <param name="scopeFactory"></param>
        [ExcludeFromCodeCoverage]
        public SpotifyPollingService(ISpotifyService spotifyService, 
            IPlayingStateService playingStateService,
            IServiceScopeFactory scopeFactory,
            ILogger<SpotifyPollingService> logger)
        {
            _spotifyService = spotifyService;
            _playingStateService = playingStateService;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Polling Service running.");

            _timer = new Timer(PollSpotifyPlayback, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(500));

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void PollSpotifyPlayback(object? state)
        {
            Interlocked.Increment(ref executionCount);
            if (_spotifyService.IsLoggedIn)
            {
                var currentlyPlaying = _spotifyService.GetCurrentPlayback().Result;
                
                if (currentlyPlaying.IsPlaying)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider
                        .GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();
                    var databaseService = new DatabaseService(context);

                    _logger.LogDebug($"Currently listening to playlist: {currentlyPlaying.Track?.PlaylistId}");
                    _logger.LogDebug($"Currently playing: {currentlyPlaying.Track?.Name}");
                    
                    var skip = _playingStateService.CheckSkipHasHappened(currentlyPlaying);
                    if (skip)
                    {
                        var databasePlaylist = databaseService.GetPlaylist(currentlyPlaying.Track?.PlaylistId ?? "").Result;

                        if (databasePlaylist is not null)
                        {
                            _logger.LogInformation($"Skipped track: {currentlyPlaying.Track?.Name} was playing from monitored playlist: {currentlyPlaying.Track?.PlaylistId}");
                        }
                    }
                }
                else if (executionCount % 10 == 0)
                {
                    _logger.LogInformation($"Not currently listening to a monitored playlist");
                }

                _playingStateService.PlayingState = currentlyPlaying;
            }
            else
            {
                if (executionCount % 200 == 0)
                {
                    _logger.LogInformation($"Not currently logged into Spotify");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Polling Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
