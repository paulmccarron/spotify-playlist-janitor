using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SpotifyPlaylistJanitorAPI.Controllers;
using SpotifyPlaylistJanitorAPI.Models;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Tests.Controllers
{
    public class SpotifyControllerTests : TestBase
    {
        private SpotifyController _spotifyController;
        private Mock<ISpotifyService> _spotifyServiceMock;

        public SpotifyControllerTests()
        {
            _spotifyServiceMock = new Mock<ISpotifyService>();

            _spotifyController = new SpotifyController(_spotifyServiceMock.Object);
        }

        [SetUp]
        public void Setup()
        {
            _spotifyServiceMock
                .SetupGet(mock => mock.IsLoggedIn)
                .Returns(true);
        }

        [Test]
        public async Task SpotifyController_GetUser_Returns_SpotifyUserModel()
        {
            // Arrange
            var userId = "userId";
            var userDisplayName = "userDisplayName";
            var userEmail = "user@gmail.com";
            var userHref = "www.spotify.com/href";

            _spotifyServiceMock
                .Setup(mock => mock.GetCurrentUser())
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
        public async Task SpotifyController_GetUser_Throws_When_Not_Logged_In()
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
    }
}
