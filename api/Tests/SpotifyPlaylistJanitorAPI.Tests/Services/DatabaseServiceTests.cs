using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpotifyPlaylistJanitorAPI.DataAccess;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Services;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class DatabaseServiceTests : TestBase
    {
        private DatabaseService _databaseService;
        private Mock<SpotifyPlaylistJanitorDatabaseContext> _dbContextMock;
        private Mock<DbSet<Playlist>> _dbSetPlaylistMock;
        private Mock<DbSet<Artist>> _dbSetArtistMock;
        private Mock<DbSet<Album>> _dbSetAlbumMock;
        private Mock<DbSet<Track>> _dbSetTrackMock;
        private Mock<DbSet<SkippedTrack>> _dbSetSkippedMock;

        [SetUp]
        public void Init()
        {
            _dbSetPlaylistMock = new Mock<DbSet<Playlist>>();
            _dbSetArtistMock = new Mock<DbSet<Artist>>();
            _dbSetAlbumMock = new Mock<DbSet<Album>>();
            _dbSetTrackMock = new Mock<DbSet<Track>>();
            _dbSetSkippedMock = new Mock<DbSet<SkippedTrack>>();
            _dbContextMock = new Mock<SpotifyPlaylistJanitorDatabaseContext>();

            _dbSetPlaylistMock.AddIQueryables(new List<Playlist>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.Playlists)
                .Returns(_dbSetPlaylistMock.Object);

            _dbSetArtistMock.AddIQueryables(new List<Artist>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.Artists)
                .Returns(_dbSetArtistMock.Object);

            _dbSetAlbumMock.AddIQueryables(new List<Album>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.Albums)
                .Returns(_dbSetAlbumMock.Object);

            _dbSetTrackMock.AddIQueryables(new List<Track>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.Tracks)
                .Returns(_dbSetTrackMock.Object);

            _dbSetSkippedMock.AddIQueryables(new List<SkippedTrack>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.SkippedTracks)
                .Returns(_dbSetSkippedMock.Object);

            _databaseService = new DatabaseService(_dbContextMock.Object);
        }

        [Test]
        public async Task DatabaseService_GetPlaylists_Returns_Data()
        {
            //Arrange
            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .AsQueryable();

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            _dbContextMock
                .Setup(mock => mock.Playlists)
                .Returns(_dbSetPlaylistMock.Object);

            var expectedResults = dbPlaylists
                .Select(x => new DatabasePlaylistModel
                {
                    Id = x.Id,
                });

            //Act
            var result = await _databaseService.GetPlaylists();

            // Assert
            result.Should().BeOfType<List<DatabasePlaylistModel>>();
            result.Should().BeEquivalentTo(expectedResults);
        }

        [Test]
        public async Task DatabaseService_GetPlaylist_Returns_Data()
        {
            //Arrange
            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .AsQueryable();

            var playlistId = dbPlaylists.First().Id;

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            var expectedResult = dbPlaylists
                .Select(x => new DatabasePlaylistModel
                {
                    Id = x.Id,
                })
                .First();

            //Act
            var result = await _databaseService.GetPlaylist(playlistId);

            // Assert
            result.Should().BeOfType<DatabasePlaylistModel>();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task DatabaseService_GetPlaylist_Returns_Null_For_Invalid_Id()
        {
            //Arrange
            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .AsQueryable();

            var playlistId = "RANDOM_ID";

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            _dbContextMock
                .Setup(mock => mock.Playlists)
                .Returns(_dbSetPlaylistMock.Object);

            //Act
            var result = await _databaseService.GetPlaylist(playlistId);

            // Assert
            result.Should().BeNull();
        }


        [Test]
        public async Task DatabaseService_AddPlaylist_Adds_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabasePlaylistRequest>().Create();

            //Act
            var result = await _databaseService.AddPlaylist(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Playlist>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_AddPlaylist_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabasePlaylistRequest>().Create();

            var dbPlaylists = Fixture.Build<Playlist>()
                .With(playlist => playlist.Id, databaseRequest.Id)
                .CreateMany()
                .AsQueryable();

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            //Act
            var result = await _databaseService.AddPlaylist(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Playlist>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_DeletePlaylist_Removes_Data()
        {
            //Arrange
            var id = "id";

            var dbSkippedTracks = Fixture.Build<SkippedTrack>()
                .CreateMany()
                .AsQueryable();

            _dbSetSkippedMock.AddIQueryables(dbSkippedTracks);

            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .AsQueryable();

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            //Act
            await _databaseService.DeletePlaylist(id);

            // Assert
            _dbContextMock.Verify(context => context.SkippedTracks.RemoveRange(It.IsAny<IEnumerable<SkippedTrack>>()), Times.Once);
            _dbContextMock.Verify(context => context.Playlists.RemoveRange(It.IsAny<IEnumerable<Playlist>>()), Times.Once);
            _dbContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddArtist_Adds_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseArtistModel>().Create();

            //Act
            var result = await _databaseService.AddArtist(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Artist>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_AddArtist_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseArtistModel>().Create();

            var dbArtists = Fixture.Build<Artist>()
                .With(artist => artist.Id, databaseRequest.Id)
                .CreateMany()
                .AsQueryable();

            _dbSetArtistMock.AddIQueryables(dbArtists);

            //Act
            var result = await _databaseService.AddArtist(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Artist>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_AddAlbum_Adds_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseAlbumModel>().Create();

            //Act
            var result = await _databaseService.AddAlbum(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Album>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_AddAlbum_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseAlbumModel>().Create();

            var dbAlbums = Fixture.Build<Album>()
                .With(album => album.Id, databaseRequest.Id)
                .CreateMany()
                .AsQueryable();

            _dbSetAlbumMock.AddIQueryables(dbAlbums);

            //Act
            var result = await _databaseService.AddAlbum(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Album>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_AddTrack_Adds_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseTrackModel>().Create();

            //Act
            var result = await _databaseService.AddTrack(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Track>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_AddTrack_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseTrackModel>().Create();
            
            var dbTracks = Fixture.Build<Track>()
                .With(album => album.Id, databaseRequest.Id)
                .CreateMany()
                .AsQueryable();

            _dbSetTrackMock.AddIQueryables(dbTracks);

            //Act
            var result = await _databaseService.AddTrack(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Track>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_AddSkippedTrack_Adds_And_Returns_Data()
        {
            //Arrange
            var playlistId = "playlistId";
            var trackId = "trackId";
            var skippedTime = new DateTime(2023, 9, 8, 14, 0, 0, 0);

            var datatbaseRequest = new DatabaseSkippedTrackModel
            {
                PlaylistId = playlistId,
                TrackId = trackId,
                SkippedDate = skippedTime,
            };

            _dbContextMock
                .Setup(mock => mock.SkippedTracks)
                .Returns(_dbSetSkippedMock.Object);

            var expectedResult = new DatabaseSkippedTrackModel
            {
                PlaylistId = playlistId,
                TrackId = trackId,
                SkippedDate = skippedTime,
            };

            //Act
            var result = await _databaseService.AddSkippedTrack(datatbaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(
                It.Is<SkippedTrack>(skipped => skipped.SkippedDate.Equals(skippedTime)),
                It.IsAny<CancellationToken>()),
            Times.Once());
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(expectedResult);
        }
        
        [Test]
        public async Task DatabaseService_GetPlaylistSkippedTracks_Returns_Data()
        {
            //Arrange
            var playlistId = "playlist_id";
            var dateTime = DateTime.UtcNow;

            var dbSkippedTracks = Fixture.Build<SkippedTrack>()
                .With(x => x.SkippedDate, dateTime)
                .With(x => x.PlaylistId, playlistId)
                .CreateMany()
                .AsQueryable();

            _dbSetSkippedMock.AddIQueryables(dbSkippedTracks);

            _dbContextMock
                .Setup(mock => mock.Playlists)
                .Returns(_dbSetPlaylistMock.Object);

            var expectedResults = dbSkippedTracks
                .Select(track => new DatabaseSkippedTrackModel
                {
                    PlaylistId = playlistId,
                    TrackId = track.TrackId,
                    SkippedDate = track.SkippedDate,
                });

            //Act
            var result = await _databaseService.GetPlaylistSkippedTracks(playlistId);

            // Assert
            result.Should().BeOfType<List<DatabaseSkippedTrackModel>>();
            result.Should().BeEquivalentTo(expectedResults);
        }
    }
}
