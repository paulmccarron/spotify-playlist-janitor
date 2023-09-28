using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal;
using Quartz;
using SpotifyPlaylistJanitorAPI.Jobs;
using SpotifyPlaylistJanitorAPI.Models.Database;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class SkippedTrackRemoveJobTests : TestBase
    {
        private SkippedTrackRemoveJob _skippedTrackRemoveJob;
        private Mock<IJobExecutionContext> _mockJobContext;

        [SetUp]
        public void Setup()
        {
            _mockJobContext = new Mock<IJobExecutionContext>();
            _skippedTrackRemoveJob = new SkippedTrackRemoveJob(MockSpotifyService.Object, MockDatabaseService.Object, new Mock<ILogger<SkippedTrackRemoveJob>>().Object);

            MockSpotifyService
                .Setup(mock => mock.IsLoggedIn)
                .Returns(true);
        }

        [Test]
        public async Task SkippedTrackRemoveJobTest_Execute_Skips_Check_If_SpotifyService_Not_Logged_In()
        {
            //Arrange
            MockSpotifyService
                .Setup(mock => mock.IsLoggedIn)
                .Returns(false);

            //Act
            await _skippedTrackRemoveJob.Execute(_mockJobContext.Object);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetPlaylists(), Times.Never);
        }

        [Test]
        public async Task SkippedTrackRemoveJobTest_Execute_Gets_Playlists_If_SpotifyService_Logged_In()
        {
            //Act
            await _skippedTrackRemoveJob.Execute(_mockJobContext.Object);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetPlaylists(), Times.Once);
            MockDatabaseService.Verify(mock => mock.GetPlaylistSkippedTracks(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task SkippedTrackRemoveJobTest_Execute_Gets_Playlist_Skipped_Tracks_If_Playlist_Has_AutoCleanupLimit()
        {
            //Arrange
            var autoCleanupPlaylist = Fixture
                .Build<DatabasePlaylistModel>()
                .With(x => x.AutoCleanupLimit, 5)
                .Create();

            var nonCleanupPlaylist = Fixture
                .Build<DatabasePlaylistModel>()
                .Without(x => x.AutoCleanupLimit)
                .Create();

            MockDatabaseService
                .Setup(mock => mock.GetPlaylists())
                .ReturnsAsync(new[] { autoCleanupPlaylist, nonCleanupPlaylist });

            //Act
            await _skippedTrackRemoveJob.Execute(_mockJobContext.Object);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetPlaylists(), Times.Once);
            MockDatabaseService.Verify(mock => mock.GetPlaylistSkippedTracks(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task SkippedTrackRemoveJobTest_Execute_Removes_Skipped_Tracks_If_Counts_Exceed_AutoCleanupLimit()
        {
            //Arrange
            var autoCleanupPlaylist = Fixture
                .Build<DatabasePlaylistModel>()
                .With(x => x.AutoCleanupLimit, 5)
                .Create();

            var nonCleanupPlaylist = Fixture
                .Build<DatabasePlaylistModel>()
                .Without(x => x.AutoCleanupLimit)
                .Create();

            MockDatabaseService
                .Setup(mock => mock.GetPlaylists())
                .ReturnsAsync(new[] { autoCleanupPlaylist, nonCleanupPlaylist });

            var trackId1 = "trackId1";
            var trackId2 = "trackId2";
            var trackId3 = "trackId3";

            var skippedTracks1 = Fixture
                .Build<DatabaseSkippedTrackResponse>()
                .With(x => x.PlaylistId, autoCleanupPlaylist.Id)
                .With(x => x.TrackId, trackId1)
                .CreateMany(10);

            var skippedTracks2 = Fixture
                .Build<DatabaseSkippedTrackResponse>()
                .With(x => x.PlaylistId, autoCleanupPlaylist.Id)
                .With(x => x.TrackId, trackId2)
                .CreateMany(3);

            var skippedTracks3 = Fixture
                .Build<DatabaseSkippedTrackResponse>()
                .With(x => x.PlaylistId, autoCleanupPlaylist.Id)
                .With(x => x.TrackId, trackId3)
                .CreateMany(6);

            var skippedTracksCollection = new List<DatabaseSkippedTrackResponse>();
            skippedTracksCollection.AddRange(skippedTracks1);
            skippedTracksCollection.AddRange(skippedTracks2);
            skippedTracksCollection.AddRange(skippedTracks3);

            MockDatabaseService
                .Setup(mock => mock.GetPlaylistSkippedTracks(autoCleanupPlaylist.Id))
                .ReturnsAsync(skippedTracksCollection);

            var expectedTrackIds = new[] { trackId1, trackId3 };

            //Act
            await _skippedTrackRemoveJob.Execute(_mockJobContext.Object);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetPlaylists(), Times.Once);
            MockDatabaseService.Verify(mock => mock.GetPlaylistSkippedTracks(It.IsAny<string>()), Times.Once);
            MockSpotifyService.Verify(mock => mock.DeletePlaylistTracks(autoCleanupPlaylist.Id, expectedTrackIds), Times.Once);
            MockDatabaseService.Verify(mock => mock.DeleteSkippedTracks(autoCleanupPlaylist.Id, expectedTrackIds), Times.Once);
        }
    }
}
