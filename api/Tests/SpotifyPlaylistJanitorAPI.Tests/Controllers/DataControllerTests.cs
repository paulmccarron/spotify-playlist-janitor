using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SpotifyPlaylistJanitorAPI.Controllers;
using SpotifyPlaylistJanitorAPI.DataAccess.Models;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Models.Spotify;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Tests.Controllers
{
    public class DataControllerTests : TestBase
    {
        private DataController _dataController;
        private Mock<IDatabaseService> _databaseServiceMock;

        public DataControllerTests()
        {
            _databaseServiceMock = new Mock<IDatabaseService>();

            _dataController = new DataController(_databaseServiceMock.Object);
        }

        [Test]
        public async Task DataController_GetUser_Returns_SpotifyUserModel()
        {
            // Arrange
            var databasePlaylists = Fixture.Build<DatabasePlaylistModel>().CreateMany().ToList();

            _databaseServiceMock
                .Setup(mock => mock.GetPlaylists())
                .ReturnsAsync(databasePlaylists);

            //Act
            var result = await _dataController.GetTrackedPlaylists();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<DatabasePlaylistModel>>>();
            result?.Value?.Should().BeEquivalentTo(databasePlaylists);
        }
    }
}
