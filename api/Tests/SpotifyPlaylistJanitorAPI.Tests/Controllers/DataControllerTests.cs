using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SpotifyPlaylistJanitorAPI.Controllers;
using SpotifyPlaylistJanitorAPI.Models.Database;
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
        public async Task DataController_GetTrackedPlaylists_Returns_DatabasePlaylistModels()
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

        [Test]
        public async Task DataController_GetTrackedPlaylists_Returns_DatabasePlaylistModel()
        {
            // Arrange
            var databasePlaylist = Fixture.Build<DatabasePlaylistModel>().Create();

            _databaseServiceMock
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(databasePlaylist);

            //Act
            var result = await _dataController.GetTrackedPlaylist("testId");

            // Assert
            result.Should().BeOfType<ActionResult<DatabasePlaylistModel>>();
            result?.Value?.Should().BeEquivalentTo(databasePlaylist);
        }

        [Test]
        public async Task DataController_GetTrackedPlaylists_Returns_Not_Found()
        {
            // Arrange
            DatabasePlaylistModel? databasePlaylist = null;
            var playlistId = Guid.NewGuid().ToString();

            _databaseServiceMock
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(databasePlaylist);

            var expectedMessage = new { Message = $"Could not find playlist with id: {playlistId}" };

            //Act
            var result = await _dataController.GetTrackedPlaylist(playlistId);

            // Assert
            var objResult = result.Result as NotFoundObjectResult;
            objResult?.Value.Should().BeEquivalentTo(expectedMessage);
        }
    }
}
