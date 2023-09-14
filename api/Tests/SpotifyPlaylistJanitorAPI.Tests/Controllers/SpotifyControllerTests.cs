using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SpotifyPlaylistJanitorAPI.Controllers;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Models.Spotify;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Tests.Controllers
{
    [TestFixture]
    public class SpotifyControllerTests : TestBase
    {
        private SpotifyController _spotifyController;
        private Mock<ISpotifyService> _spotifyServiceMock;
        private Mock<IDatabaseService> _databaseServiceMock;

        [SetUp]
        public void Setup()
        {
            _spotifyServiceMock = new Mock<ISpotifyService>();
            _spotifyServiceMock
                .SetupGet(mock => mock.IsLoggedIn)
                .Returns(true);

            _databaseServiceMock = new Mock<IDatabaseService>();

            _spotifyController = new SpotifyController(
                _spotifyServiceMock.Object, 
                _databaseServiceMock.Object
                );
        }

        [Test]
        public async Task SpotifyController_GetUser_Returns_Data()
        {
            // Arrange
            var userId = "userId";
            var userDisplayName = "userDisplayName";
            var userEmail = "user@gmail.com";
            var userHref = "www.spotify.com/href";

            _spotifyServiceMock
                .Setup(mock => mock.GetUserDetails())
                .ReturnsAsync(new SpotifyUserModel
                {
                    Id = userId,
                    DisplayName = userDisplayName,
                    Email = userEmail,
                    Href = userHref,
                });

            //Act
            var result = await _spotifyController.GetUser();

            // Assert
            result.Should().BeOfType<ActionResult<SpotifyUserModel>>();

            result?.Value?.Id.Should().Be(userId);
            result?.Value?.DisplayName.Should().Be(userDisplayName);
            result?.Value?.Email.Should().Be(userEmail);
            result?.Value?.Href.Should().Be(userHref);
        }

        [Test]
        public async Task SpotifyController_GetUser_Returns_Error_When_Not_Logged_In()
        {
            // Arrange
            _spotifyServiceMock
                .SetupGet(mock => mock.IsLoggedIn)
                .Returns(false);

            //Act
            var result = await _spotifyController.GetUser();

            // Assert
            var objResult = result.Result as ObjectResult;
            objResult?.StatusCode.Should().Be(500);
        }

        [Test]
        public async Task SpotifyController_GetPlaylists_Returns_Data()
        {
            //Arrange
            var spotifyPlaylists = Fixture.Build<SpotifyPlaylistModel>().CreateMany().ToList();

            _spotifyServiceMock
                .Setup(mock => mock.GetUserPlaylists())
                .ReturnsAsync(spotifyPlaylists);

            //Act
            var result = await _spotifyController.GetPlaylists();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<SpotifyPlaylistModel>>>();

            result?.Value?.Should().BeEquivalentTo(spotifyPlaylists);
        }

        [Test]
        public async Task SpotifyController_GetPlaylists_Returns_Error_When_Not_Logged_In()
        {
            // Arrange
            _spotifyServiceMock
                .SetupGet(mock => mock.IsLoggedIn)
                .Returns(false);

            //Act
            var result = await _spotifyController.GetPlaylists();

            // Assert
            var objResult = result.Result as ObjectResult;
            objResult?.StatusCode.Should().Be(500);
        }

        [Test]
        public async Task SpotifyController_GetPlaylist_Returns_Data()
        {
            //Arrange
            var spotifyPlaylist = Fixture.Build<SpotifyPlaylistModel>().Create();

            _spotifyServiceMock
                .Setup(mock => mock.GetUserPlaylist(It.IsAny<string>()))
                .ReturnsAsync(spotifyPlaylist);

            //Act
            var result = await _spotifyController.GetPlaylist("id");

            // Assert
            result.Should().BeOfType<ActionResult<SpotifyPlaylistModel>>();

            result?.Value?.Should().BeEquivalentTo(spotifyPlaylist);
        }

        [Test]
        public async Task SpotifyController_GetPlaylist_Returns_Not_Found()
        {
            //Arrange
            var id = "playlist_id";
            SpotifyPlaylistModel spotifyPlaylist = null;

            _spotifyServiceMock
                .Setup(mock => mock.GetUserPlaylist(It.IsAny<string>()))
                .ReturnsAsync(spotifyPlaylist);

            var expectedMessage = new { Message = $"Could not find Spotify playlist with id: {id}" };

            //Act
            var result = await _spotifyController.GetPlaylist(id);

            // Assert
            var objResult = result.Result as NotFoundObjectResult;
            objResult?.Value.Should().BeEquivalentTo(expectedMessage);
        }

        [Test]
        public async Task SpotifyController_GetPlaylist_Returns_Error_When_Not_Logged_In()
        {
            // Arrange
            _spotifyServiceMock
                .SetupGet(mock => mock.IsLoggedIn)
                .Returns(false);

            //Act
            var result = await _spotifyController.GetPlaylist("id");

            // Assert
            var objResult = result.Result as ObjectResult;
            objResult?.StatusCode.Should().Be(500);
        }

        [Test]
        public async Task SpotifyController_GetPlaylistTracks_Returns_Data()
        {
            //Arrange
            var spotifyPlaylist = Fixture.Build<SpotifyPlaylistModel>().Create();

            _spotifyServiceMock
                .Setup(mock => mock.GetUserPlaylist(It.IsAny<string>()))
                .ReturnsAsync(spotifyPlaylist);

            var spotifyTracks = Fixture.Build<SpotifyTrackModel>()
                .CreateMany()
                .ToList();

            _spotifyServiceMock
                .Setup(mock => mock.GetUserPlaylistTracks(It.IsAny<string>()))
                .ReturnsAsync(spotifyTracks);

            //Act
            var result = await _spotifyController.GetPlaylistTracks("id");

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<SpotifyTrackModel>>>();

            result?.Value?.Should().BeEquivalentTo(spotifyTracks);
        }

        [Test]
        public async Task SpotifyController_GetPlaylistTracks_Returns_Not_Found()
        {
            //Arrange
            var id = "playlist_id";
            SpotifyPlaylistModel spotifyPlaylist = null;

            _spotifyServiceMock
                .Setup(mock => mock.GetUserPlaylist(It.IsAny<string>()))
                .ReturnsAsync(spotifyPlaylist);

            var expectedMessage = new { Message = $"Could not find Spotify playlist with id: {id}" };

            //Act
            var result = await _spotifyController.GetPlaylistTracks(id);

            // Assert
            var objResult = result.Result as NotFoundObjectResult;
            objResult?.Value.Should().BeEquivalentTo(expectedMessage);
        }

        [Test]
        public async Task SpotifyController_GetPlaylistTracks_Returns_Error_When_Not_Logged_In()
        {
            // Arrange
            _spotifyServiceMock
                .SetupGet(mock => mock.IsLoggedIn)
                .Returns(false);

            //Act
            var result = await _spotifyController.GetPlaylistTracks("id");

            // Assert
            var objResult = result.Result as ObjectResult;
            objResult?.StatusCode.Should().Be(500);
        }

        [Test]
        public async Task SpotifyController_DeletePlaylistTracks_Returns_No_Content()
        {
            //Arrange
            var trackIds = Fixture.Build<string>().CreateMany();
            var spotifyPlaylist = Fixture.Build<SpotifyPlaylistModel>().Create();

            _spotifyServiceMock
                .Setup(mock => mock.GetUserPlaylist(It.IsAny<string>()))
                .ReturnsAsync(spotifyPlaylist);

            //Act
            var result = await _spotifyController.DeletePlaylistTracks("id", trackIds);

            // Assert
            _spotifyServiceMock.Verify(mock => mock.DeletePlaylistTracks(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Once());
            _databaseServiceMock.Verify(mock => mock.DeleteSkippedTracks(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Once());
            result.Result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task SpotifyController_DeletePlaylistTracks_Returns_Not_Found()
        {
            //Arrange
            var id = "playlist_id";
            var trackIds = Fixture.Build<string>().CreateMany();
            SpotifyPlaylistModel spotifyPlaylist = null;

            _spotifyServiceMock
                .Setup(mock => mock.GetUserPlaylist(It.IsAny<string>()))
                .ReturnsAsync(spotifyPlaylist);

            var expectedMessage = new { Message = $"Could not find Spotify playlist with id: {id}" };

            //Act
            var result = await _spotifyController.DeletePlaylistTracks(id, trackIds);

            // Assert
            var objResult = result.Result as NotFoundObjectResult;
            objResult?.Value.Should().BeEquivalentTo(expectedMessage);
        }

        [Test]
        public async Task SpotifyController_DeletePlaylistTracks_Returns_Error_When_Not_Logged_In()
        {
            // Arrange
            var trackIds = Fixture.Build<string>().CreateMany();

            _spotifyServiceMock
                .SetupGet(mock => mock.IsLoggedIn)
                .Returns(false);

            //Act
            var result = await _spotifyController.DeletePlaylistTracks("id", trackIds);

            // Assert
            var objResult = result.Result as ObjectResult;
            objResult?.StatusCode.Should().Be(500);
        }
    }
}
