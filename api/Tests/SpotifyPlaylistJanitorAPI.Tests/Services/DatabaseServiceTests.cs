using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.DataAccess.Models;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Services;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    public class DatabaseServiceTests : TestBase
    {
        private DatabaseService _databaseService;
        private Mock<SpotifyPlaylistJanitorDatabaseContext> _dbContextMock;
        private Mock<DbSet<SpotifyPlaylist>> _dbSetPlaylists;

        public DatabaseServiceTests()
        {
            _dbContextMock = new Mock<SpotifyPlaylistJanitorDatabaseContext>();
            _dbSetPlaylists = new Mock<DbSet<SpotifyPlaylist>>();

            _databaseService = new DatabaseService(_dbContextMock.Object);
        }

        [Test]
        public async Task DatabaseService_GetPlaylists_Returns_Data()
        {
            //Arrange
            var dbPlaylists = Fixture.Build<SpotifyPlaylist>()
                .CreateMany()
                .AsQueryable();

            _dbSetPlaylists.AddIQueryables(dbPlaylists);

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylists.Object);

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

            _dbSetPlaylists.AddIQueryables(dbPlaylists);

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylists.Object);

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

            _dbSetPlaylists.AddIQueryables(dbPlaylists);

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(_dbSetPlaylists.Object);

            //Act
            var result = await _databaseService.GetPlaylist(playlistId);

            // Assert
            result.Should().BeNull();
        }


        [Test]
        public async Task DatabaseService_AddPlaylist_Returns_Data()
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

            var mockSet = new Mock<DbSet<SpotifyPlaylist>>();

            _dbContextMock
                .Setup(mock => mock.SpotifyPlaylists)
                .Returns(mockSet.Object);

            var expectedResult = new DatabasePlaylistModel
            {
                Id = id, 
                Name = name, 
                Href = href,
            };

            //Act
            var result = await _databaseService.AddPlaylist(datatbaseRequest);

            // Assert
            _dbContextMock.Verify(m => m.AddAsync(It.IsAny<SpotifyPlaylist>(), It.IsAny<CancellationToken>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
