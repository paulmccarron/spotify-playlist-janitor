using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.DataAccess.Models;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Services;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class DatabaseServiceTests : TestBase
    {
        private DatabaseService _databaseService;
        private Mock<SpotifyPlaylistJanitorDatabaseContext> _dbContextMock;
        private Mock<DbSet<SpotifyPlaylist>> _dbSetPlaylistMock;
        private Mock<DbSet<SpotifyArtist>> _dbSetArtistMock;
        private Mock<DbSet<SpotifyAlbum>> _dbSetAlbumMock;
        private Mock<DbSet<SpotifyTrack>> _dbSetTrackMock;
        private Mock<DbSet<SkippedTrack>> _dbSetSkippedMock;

        [SetUp]
        public void Init()
        {
            _dbSetPlaylistMock = new Mock<DbSet<SpotifyPlaylist>>();
            _dbSetArtistMock = new Mock<DbSet<SpotifyArtist>>();
            _dbSetAlbumMock = new Mock<DbSet<SpotifyAlbum>>();
            _dbSetTrackMock = new Mock<DbSet<SpotifyTrack>>();
            _dbSetSkippedMock = new Mock<DbSet<SkippedTrack>>();
            _dbContextMock = new Mock<SpotifyPlaylistJanitorDatabaseContext>();

            _dbSetPlaylistMock.AddIQueryables(new List<SpotifyPlaylist>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistMock.Object);

            _dbSetArtistMock.AddIQueryables(new List<SpotifyArtist>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.SpotifyArtists)
                .Returns(_dbSetArtistMock.Object);

            _dbSetAlbumMock.AddIQueryables(new List<SpotifyAlbum>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.SpotifyAlbums)
                .Returns(_dbSetAlbumMock.Object);

            _dbSetTrackMock.AddIQueryables(new List<SpotifyTrack>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.SpotifyTracks)
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
            var dbPlaylists = Fixture.Build<SpotifyPlaylist>()
                .CreateMany()
                .AsQueryable();

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistMock.Object);

            var expectedResults = dbPlaylists
                .Select(x => new DatabasePlaylistModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Href = x.Href,
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
            var dbPlaylists = Fixture.Build<SpotifyPlaylist>()
                .CreateMany()
                .AsQueryable();

            var playlistId = dbPlaylists.First().Id;

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            var expectedResult = dbPlaylists
                .Select(x => new DatabasePlaylistModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Href = x.Href,
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
            var dbPlaylists = Fixture.Build<SpotifyPlaylist>()
                .CreateMany()
                .AsQueryable();

            var playlistId = "RANDOM_ID";

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
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
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyPlaylist>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_AddPlaylist_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabasePlaylistRequest>().Create();

            var dbPlaylists = Fixture.Build<SpotifyPlaylist>()
                .With(playlist => playlist.Id, databaseRequest.Id)
                .CreateMany()
                .AsQueryable();

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            //Act
            var result = await _databaseService.AddPlaylist(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyPlaylist>(), It.IsAny<CancellationToken>()), Times.Never);
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

            var dbPlaylists = Fixture.Build<SpotifyPlaylist>()
                .CreateMany()
                .AsQueryable();

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            //Act
            await _databaseService.DeletePlaylist(id);

            // Assert
            _dbContextMock.Verify(context => context.SkippedTracks.RemoveRange(It.IsAny<IEnumerable<SkippedTrack>>()), Times.Once);
            _dbContextMock.Verify(context => context.SpotifyPlaylists.RemoveRange(It.IsAny<IEnumerable<SpotifyPlaylist>>()), Times.Once);
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
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyArtist>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_AddArtist_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseArtistModel>().Create();

            var dbArtists = Fixture.Build<SpotifyArtist>()
                .With(artist => artist.Id, databaseRequest.Id)
                .CreateMany()
                .AsQueryable();

            _dbSetArtistMock.AddIQueryables(dbArtists);

            //Act
            var result = await _databaseService.AddArtist(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyArtist>(), It.IsAny<CancellationToken>()), Times.Never);
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
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyAlbum>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_AddAlbum_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseAlbumModel>().Create();

            var dbAlbums = Fixture.Build<SpotifyAlbum>()
                .With(album => album.Id, databaseRequest.Id)
                .CreateMany()
                .AsQueryable();

            _dbSetAlbumMock.AddIQueryables(dbAlbums);

            //Act
            var result = await _databaseService.AddAlbum(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyAlbum>(), It.IsAny<CancellationToken>()), Times.Never);
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
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyTrack>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_AddTrack_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseTrackModel>().Create();
            
            var dbTracks = Fixture.Build<SpotifyTrack>()
                .With(album => album.Id, databaseRequest.Id)
                .CreateMany()
                .AsQueryable();

            _dbSetTrackMock.AddIQueryables(dbTracks);

            //Act
            var result = await _databaseService.AddTrack(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyTrack>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_AddSkippedTrack_Adds_And_Returns_Data()
        {
            //Arrange
            var playlistId = "playlistId";
            var trackId = "trackId";
            var skippedTime = new DateTimeOffset(2023, 9, 8, 14, 0, 0, 0, TimeSpan.Zero);

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
                It.Is<SkippedTrack>(skipped => skipped.SkippedDate.Equals(skippedTime.ToUnixTimeSeconds())),
                It.IsAny<CancellationToken>()),
            Times.Once());
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
