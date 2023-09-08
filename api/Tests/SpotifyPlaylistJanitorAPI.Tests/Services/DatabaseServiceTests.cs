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
        private Mock<DbSet<SpotifyPlaylist>> _dbSetPlaylistsMock;
        private Mock<DbSet<SkippedTrack>> _dbSetSkippedMock;

        [SetUp]
        public void Init()
        {
            _dbSetPlaylistsMock = new Mock<DbSet<SpotifyPlaylist>>();
            _dbSetSkippedMock = new Mock<DbSet<SkippedTrack>>();
            _dbContextMock = new Mock<SpotifyPlaylistJanitorDatabaseContext>();
            _databaseService = new DatabaseService(_dbContextMock.Object);
        }

        [Test]
        public async Task DatabaseService_GetPlaylists_Returns_Data()
        {
            //Arrange
            var dbPlaylists = Fixture.Build<SpotifyPlaylist>()
                .CreateMany()
                .AsQueryable();

            _dbSetPlaylistsMock.AddIQueryables(dbPlaylists);

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistsMock.Object);

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

            _dbSetPlaylistsMock.AddIQueryables(dbPlaylists);

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistsMock.Object);

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

            _dbSetPlaylistsMock.AddIQueryables(dbPlaylists);

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistsMock.Object);

            //Act
            var result = await _databaseService.GetPlaylist(playlistId);

            // Assert
            result.Should().BeNull();
        }


        [Test]
        public async Task DatabaseService_AddPlaylist_Adds_And_Returns_Data()
        {
            //Arrange
            var id = "id";
            var name = "name";
            var href = "href";

            var datatbaseRequest = new DatabasePlaylistRequest
            {
                Id = id,
                Name = name,
                Href = href,
            };

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistsMock.Object);

            var expectedResult = new DatabasePlaylistModel
            {
                Id = id, 
                Name = name, 
                Href = href,
            };

            //Act
            var result = await _databaseService.AddPlaylist(datatbaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyPlaylist>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task DatabaseService_AddPlaylist_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var id = "id";
            var name = "name";
            var href = "href";

            var datatbaseRequest = new DatabasePlaylistRequest
            {
                Id = id,
                Name = name,
                Href = href,
            };

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistsMock.Object);

            var expectedResult = new DatabasePlaylistModel
            {
                Id = id,
                Name = name,
                Href = href,
            };

            //Act
            var result = await _databaseService.AddPlaylist(datatbaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyPlaylist>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeEquivalentTo(expectedResult);
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

            _dbContextMock
                .Setup(mock => mock.SkippedTracks)
                .Returns(_dbSetSkippedMock.Object);

            var dbPlaylists = Fixture.Build<SpotifyPlaylist>()
                .CreateMany()
                .AsQueryable();

            _dbSetPlaylistsMock.AddIQueryables(dbPlaylists);

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistsMock.Object);

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
            var id = "id";
            var name = "name";
            var href = "href";

            var datatbaseRequest = new DatabasePlaylistRequest
            {
                Id = id,
                Name = name,
                Href = href,
            };

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistsMock.Object);

            var expectedResult = new DatabasePlaylistModel
            {
                Id = id,
                Name = name,
                Href = href,
            };

            //Act
            var result = await _databaseService.AddPlaylist(datatbaseRequest);

            // Assert
            Assert.Fail();
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyPlaylist>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task DatabaseService_AddArtist_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var id = "id";
            var name = "name";
            var href = "href";

            var datatbaseRequest = new DatabasePlaylistRequest
            {
                Id = id,
                Name = name,
                Href = href,
            };

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistsMock.Object);

            var expectedResult = new DatabasePlaylistModel
            {
                Id = id,
                Name = name,
                Href = href,
            };

            //Act
            var result = await _databaseService.AddPlaylist(datatbaseRequest);

            // Assert
            Assert.Fail();
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyPlaylist>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task DatabaseService_AddAlbum_Adds_And_Returns_Data()
        {
            //Arrange
            var id = "id";
            var name = "name";
            var href = "href";

            var datatbaseRequest = new DatabasePlaylistRequest
            {
                Id = id,
                Name = name,
                Href = href,
            };

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistsMock.Object);

            var expectedResult = new DatabasePlaylistModel
            {
                Id = id,
                Name = name,
                Href = href,
            };

            //Act
            var result = await _databaseService.AddPlaylist(datatbaseRequest);

            // Assert
            Assert.Fail();
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyPlaylist>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task DatabaseService_AddAlbum_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var id = "id";
            var name = "name";
            var href = "href";

            var datatbaseRequest = new DatabasePlaylistRequest
            {
                Id = id,
                Name = name,
                Href = href,
            };

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistsMock.Object);

            var expectedResult = new DatabasePlaylistModel
            {
                Id = id,
                Name = name,
                Href = href,
            };

            //Act
            var result = await _databaseService.AddPlaylist(datatbaseRequest);

            // Assert
            Assert.Fail();
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyPlaylist>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task DatabaseService_AddTrack_Adds_And_Returns_Data()
        {
            //Arrange
            var id = "id";
            var name = "name";
            var href = "href";

            var datatbaseRequest = new DatabasePlaylistRequest
            {
                Id = id,
                Name = name,
                Href = href,
            };

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistsMock.Object);

            var expectedResult = new DatabasePlaylistModel
            {
                Id = id,
                Name = name,
                Href = href,
            };

            //Act
            var result = await _databaseService.AddPlaylist(datatbaseRequest);

            // Assert
            Assert.Fail();
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyPlaylist>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task DatabaseService_AddTrack_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var id = "id";
            var name = "name";
            var href = "href";

            var datatbaseRequest = new DatabasePlaylistRequest
            {
                Id = id,
                Name = name,
                Href = href,
            };

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylistsMock.Object);

            var expectedResult = new DatabasePlaylistModel
            {
                Id = id,
                Name = name,
                Href = href,
            };

            //Act
            var result = await _databaseService.AddPlaylist(datatbaseRequest);

            // Assert
            Assert.Fail();
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<SpotifyPlaylist>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeEquivalentTo(expectedResult);
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
