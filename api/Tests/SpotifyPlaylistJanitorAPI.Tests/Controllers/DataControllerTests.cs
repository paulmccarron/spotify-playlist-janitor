﻿using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SpotifyPlaylistJanitorAPI.Controllers;
using SpotifyPlaylistJanitorAPI.Models.Database;

namespace SpotifyPlaylistJanitorAPI.Tests.Controllers
{
    [TestFixture]
    public class DataControllerTests : TestBase
    {
        private DataController _dataController;

        [SetUp]
        public void Init()
        {
            _dataController = new DataController(MockDatabaseService.Object);
        }

        [Test]
        public async Task DataController_GetMonitoredPlaylists_Returns_DatabasePlaylistModels()
        {
            // Arrange
            var databasePlaylists = Fixture.Build<DatabasePlaylistModel>().CreateMany().ToList();

            MockDatabaseService
                .Setup(mock => mock.GetPlaylists())
                .ReturnsAsync(databasePlaylists);

            //Act
            var result = await _dataController.GetMonitoredPlaylists();

            // Assert
            MockDatabaseService.Verify(mock => mock.GetPlaylists(), Times.Once);
            result.Should().BeOfType<ActionResult<IEnumerable<DatabasePlaylistModel>>>();
            result?.Value?.Should().BeEquivalentTo(databasePlaylists);
        }

        [Test]
        public async Task DataController_GetMonitoredPlaylist_Returns_DatabasePlaylistModel()
        {
            // Arrange
            var id = "id";
            var databasePlaylist = Fixture.Build<DatabasePlaylistModel>().Create();

            MockDatabaseService
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(databasePlaylist);

            //Act
            var result = await _dataController.GetMonitoredPlaylist(id);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetPlaylist(It.Is<string>(s => s.Equals(id))), Times.Once);
            result.Should().BeOfType<ActionResult<DatabasePlaylistModel>>();
            result?.Value?.Should().BeEquivalentTo(databasePlaylist);
        }

        [Test]
        public async Task DataController_GetMonitoredPlaylist_Returns_Not_Found()
        {
            // Arrange
            var id = "id";
            DatabasePlaylistModel? databasePlaylist = null;

            MockDatabaseService
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(databasePlaylist);

            var expectedMessage = new { Message = $"Could not find playlist with id: {id}" };

            //Act
            var result = await _dataController.GetMonitoredPlaylist(id);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetPlaylist(It.Is<string>(s => s.Equals(id))), Times.Once);
            var objResult = result.Result as NotFoundObjectResult;
            objResult?.Value.Should().BeEquivalentTo(expectedMessage);
        }

        [Test]
        public async Task DataController_CreateMonitoredPlaylist_Returns_DatabasePlaylistModel()
        {
            // Arrange
            var databaseRequest = Fixture.Build<DatabasePlaylistRequest>().Create();
            DatabasePlaylistModel? databasePlaylistNull = null;
            var databasePlaylist = Fixture.Build<DatabasePlaylistModel>().Create();

            MockDatabaseService
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(databasePlaylistNull);

            MockDatabaseService
                .Setup(mock => mock.AddPlaylist(It.IsAny<DatabasePlaylistRequest>()))
                .ReturnsAsync(databasePlaylist);

            //Act
            var result = await _dataController.CreateMonitoredPlaylist(databaseRequest);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetPlaylist(It.Is<string>(s => s.Equals(databaseRequest.Id))), Times.Once);
            MockDatabaseService.Verify(mock => mock.AddPlaylist(It.Is<DatabasePlaylistRequest>(request => request.Equals(databaseRequest))), Times.Once);
            result.Should().BeOfType<ActionResult<DatabasePlaylistModel>>();
            result?.Value?.Should().BeEquivalentTo(databasePlaylist);
        }

        [Test]
        public async Task DataController_CreateMonitoredPlaylist_Returns_Bad_Request()
        {
            // Arrange
            var databaseRequest = Fixture.Build<DatabasePlaylistRequest>().Create();
            var databasePlaylist = Fixture.Build<DatabasePlaylistModel>().Create();

            MockDatabaseService
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(databasePlaylist);

            var expectedMessage = new { Message = $"Playlist with id: {databaseRequest.Id} already exists" };

            //Act
            var result = await _dataController.CreateMonitoredPlaylist(databaseRequest);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetPlaylist(It.Is<string>(s => s.Equals(databaseRequest.Id))), Times.Once);
            MockDatabaseService.Verify(mock => mock.AddPlaylist(It.IsAny<DatabasePlaylistRequest>()), Times.Never);
            var objResult = result.Result as BadRequestObjectResult;
            objResult?.Value.Should().BeEquivalentTo(expectedMessage);
        }

        [Test]
        public async Task DataController_UpdateMonitoredPlaylist_Returns_DatabasePlaylistModel()
        {
            // Arrange
            var id = "id";
            var updateRequest = Fixture.Build<DatabasePlaylistUpdateRequest>().Create();
            var databasePlaylist = Fixture.Build<DatabasePlaylistModel>().Create();

            MockDatabaseService
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(databasePlaylist);

            //Act
            var result = await _dataController.UpdateMonitoredPlaylist(id, updateRequest);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetPlaylist(It.Is<string>(s => s.Equals(id))), Times.Once);
            result.Should().BeOfType<ActionResult<DatabasePlaylistModel>>();
            result?.Value?.Should().BeEquivalentTo(databasePlaylist);
        }

        [Test]
        public async Task DataController_UpdateMonitoredPlaylist_Returns_Not_Found()
        {
            // Arrange
            var id = "id";
            var updateRequest = Fixture.Build<DatabasePlaylistUpdateRequest>().Create();
            DatabasePlaylistModel? databasePlaylist = null;

            MockDatabaseService
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(databasePlaylist);

            var expectedMessage = new { Message = $"Could not find playlist with id: {id}" };

            //Act
            var result = await _dataController.UpdateMonitoredPlaylist(id, updateRequest);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetPlaylist(It.Is<string>(s => s.Equals(id))), Times.Once);
            var objResult = result.Result as NotFoundObjectResult;
            objResult?.Value.Should().BeEquivalentTo(expectedMessage);
        }

        [Test]
        public async Task DataController_DeleteMonitoredPlaylist_Removes_Playlist()
        {
            // Arrange
            var id = "id";
            var databasePlaylist = Fixture.Build<DatabasePlaylistModel>().Create();

            MockDatabaseService
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(databasePlaylist);

            //Act
            var result = await _dataController.DeleteMonitoredPlaylist(id);

            // Assert
            MockDatabaseService.Verify(mock => mock.DeletePlaylist(It.IsAny<string>()), Times.Once());
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task DataController_DeleteMonitoredPlaylist_Returns_Not_Found()
        {
            // Arrange
            var id = "id";
            DatabasePlaylistModel? databasePlaylist = null;

            MockDatabaseService
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(databasePlaylist);

            var expectedMessage = new { Message = $"Could not find playlist with id: {id}" };

            //Act
            var result = await _dataController.DeleteMonitoredPlaylist(id);

            // Assert
            MockDatabaseService.Verify(mock => mock.DeletePlaylist(It.IsAny<string>()), Times.Never);
            result.Should().BeOfType<NotFoundObjectResult>();
            var objResult = result as NotFoundObjectResult;
            objResult?.Value.Should().BeEquivalentTo(expectedMessage);
        }

        [Test]
        public async Task DataController_GetMonitoredPlaylistSkippedTracks_Returns_DatabasePlaylistModel()
        {
            // Arrange
            var id = "id";
            var databasePlaylist = Fixture.Build<DatabasePlaylistModel>().Create();

            MockDatabaseService
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(databasePlaylist);

            var databaseSkippedTracks = Fixture.Build<DatabaseSkippedTrackResponse>().CreateMany();

            MockDatabaseService
                .Setup(mock => mock.GetPlaylistSkippedTracks(It.IsAny<string>()))
                .ReturnsAsync(databaseSkippedTracks);

            //Act
            var result = await _dataController.GetMonitoredPlaylistSkippedTracks(id);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetPlaylistSkippedTracks(It.Is<string>(s => s.Equals(id))), Times.Once);
            result.Should().BeOfType<ActionResult<IEnumerable<DatabaseSkippedTrackResponse>>>();
            result?.Value?.Should().BeEquivalentTo(databaseSkippedTracks);
        }

        [Test]
        public async Task DataController_GetMonitoredPlaylistSkippedTracks_Returns_Not_Found()
        {
            // Arrange
            var id = "id";
            DatabasePlaylistModel? databasePlaylist = null;

            MockDatabaseService
                .Setup(mock => mock.GetPlaylist(It.IsAny<string>()))
                .ReturnsAsync(databasePlaylist);

            var expectedMessage = new { Message = $"Could not find playlist with id: {id}" };

            //Act
            var result = await _dataController.GetMonitoredPlaylistSkippedTracks(id);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetPlaylist(It.Is<string>(s => s.Equals(id))), Times.Once);
            MockDatabaseService.Verify(mock => mock.GetPlaylistSkippedTracks(It.Is<string>(s => s.Equals(id))), Times.Never);
            var objResult = result.Result as NotFoundObjectResult;
            objResult?.Value.Should().BeEquivalentTo(expectedMessage);
        }
    }
}
