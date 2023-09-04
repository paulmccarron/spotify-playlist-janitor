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
    }
}
