using Quartz;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Jobs
{
    /// <summary>
    /// Job to check if any skipped tracks need to be removed from users playlists.
    /// </summary>
    [DisallowConcurrentExecution]
    public class SkippedTrackRemoveJob : IJob
    {
        private readonly ISpotifyService _spotifyService;
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<SkippedTrackRemoveJob> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkippedTrackRemoveJob"/> class.
        /// </summary>
        /// <param name="spotifyService">The Spotify Service.</param>
        /// <param name="databaseService">The Database Service.</param>
        /// <param name="logger">The Application Logger.</param>
        public SkippedTrackRemoveJob(ISpotifyService spotifyService, IDatabaseService databaseService, ILogger<SkippedTrackRemoveJob> logger)
        {
            _spotifyService = spotifyService;
            _databaseService = databaseService;
            _logger = logger;
        }

        /// <summary>
        /// Checks if the Spotify Service is logged in. If it is then the method
        /// will iterate through each user playlist and check if any of the Skippped Tracks
        /// meet the criteria for automatic removal.
        /// </summary>
        /// <param name="context">Quartz Job Execution Context</param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            if (_spotifyService.IsLoggedIn)
            {
                var playlists = await _databaseService.GetPlaylists();

                foreach (var playlist in playlists)
                {
                    if (playlist.AutoCleanupLimit is not null)
                    {
                        var skippedTracks = await _databaseService.GetPlaylistSkippedTracks(playlist.Id);
                        var autoDeleteTrackIds = skippedTracks
                            .GroupBy(skippedTrack => skippedTrack.TrackId)
                            .Where(grp => grp.Count() > playlist.AutoCleanupLimit)
                            .Select(grp => grp.Key);

                        if(autoDeleteTrackIds.Count() > 0)
                        {
                            await _spotifyService.DeletePlaylistTracks(playlist.Id, autoDeleteTrackIds);
                            await _databaseService.DeleteSkippedTracks(playlist.Id, autoDeleteTrackIds);

                        }
                    }
                }
            }

            return;
        }
    }
}

