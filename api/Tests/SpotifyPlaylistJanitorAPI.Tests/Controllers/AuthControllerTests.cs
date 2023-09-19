using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using SpotifyPlaylistJanitorAPI.Controllers;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Models.Spotify;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests : TestBase
    {
        private AuthController _authController;
        private Mock<ISpotifyService> _spotifyServiceMock;
        private Mock<IAuthService> _authService;
        private IOptions<SpotifyOption> _spotifyOptions;

        [SetUp]
        public void Init()
        {
            _spotifyServiceMock = new Mock<ISpotifyService>();
            _authService = new Mock<IAuthService>();
            _spotifyOptions = Options.Create(new SpotifyOption
            {
                ClientId = "mockClientId",
                ClientSecret = "mockClientSecret",
            });

            _authController = new AuthController(
                _spotifyServiceMock.Object, 
                _authService.Object, 
                _spotifyOptions);
        }

        [Test]
        public async Task AuthController_RegisterUser_Returns_NoContent()
        {
            // Arrange
            var userLoginRequest = Fixture.Build<UserLoginRequest>().Create();

            _authService
                .Setup(mock => mock.RegisterUser(It.IsAny<UserLoginRequest>()))
                .ReturnsAsync(true);

            //Act
            var result = await _authController.RegisterUser(userLoginRequest);

            // Assert
            _authService.Verify(mock => mock.RegisterUser(userLoginRequest), Times.Once);
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task AuthController_RegisterUser_Returns_BadRequest()
        {
            // Arrange
            var userLoginRequest = Fixture.Build<UserLoginRequest>().Create();

            _authService
                .Setup(mock => mock.RegisterUser(It.IsAny<UserLoginRequest>()))
                .ReturnsAsync(false);

            var expectedMessage = new { Message = "User already exists." };

            //Act
            var result = await _authController.RegisterUser(userLoginRequest);

            // Assert
            _authService.Verify(mock => mock.RegisterUser(userLoginRequest), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
            var objResult = result as BadRequestObjectResult;
            objResult?.Value.Should().BeEquivalentTo(expectedMessage);
        }

        [Test]
        public async Task AuthController_Login_Returns_Data()
        {
            // Arrange
            var userLoginRequest = Fixture.Build<UserLoginRequest>().Create();
            var jwtModel = Fixture.Build<JWTModel>().Create();

            _authService
                .Setup(mock => mock.AuthenticateUser(It.IsAny<UserLoginRequest>()))
                .ReturnsAsync(jwtModel);

            //Act
            var result = await _authController.Login(userLoginRequest);

            // Assert
            _authService.Verify(mock => mock.AuthenticateUser(userLoginRequest), Times.Once);
            result.Should().BeOfType<ActionResult<JWTModel>>();
            result?.Value?.Should().BeEquivalentTo(jwtModel);
        }

        [Test]
        public async Task AuthController_Login_Returns_Unauthorized()
        {
            // Arrange
            var userLoginRequest = Fixture.Build<UserLoginRequest>().Create();
            JWTModel jwtModel = null;

            _authService
                .Setup(mock => mock.AuthenticateUser(It.IsAny<UserLoginRequest>()))
                .ReturnsAsync(jwtModel);

            //Act
            var result = await _authController.Login(userLoginRequest);

            // Assert
            _authService.Verify(mock => mock.AuthenticateUser(userLoginRequest), Times.Once);
            result.Result.Should().BeOfType<UnauthorizedResult>();
        }
    }
}
