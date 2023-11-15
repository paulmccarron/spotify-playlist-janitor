using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SpotifyPlaylistJanitorAPI.Controllers;
using SpotifyPlaylistJanitorAPI.Models.Auth;

namespace SpotifyPlaylistJanitorAPI.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests : TestBase
    {
        private AuthController _authController;

        [SetUp]
        public void Init()
        {
            _authController = new AuthController(
                MockSpotifyService.Object,
                MockSpotifyClientService.Object,
                MockAuthService.Object,
                SpotifyOptions,
                MockHttpContextAccessor.Object);
        }

        [Test]
        public async Task AuthController_RegisterUser_Returns_NoContent()
        {
            // Arrange
            var userRegisterRequest = Fixture.Build<UserRegisterRequest>().Create();

            MockAuthService
                .Setup(mock => mock.RegisterUser(It.IsAny<UserRegisterRequest>()))
                .ReturnsAsync(true);

            //Act
            var result = await _authController.RegisterUser(userRegisterRequest);

            // Assert
            MockAuthService.Verify(mock => mock.RegisterUser(userRegisterRequest), Times.Once);
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task AuthController_RegisterUser_Returns_BadRequest()
        {
            // Arrange
            var userRegisterRequest = Fixture.Build<UserRegisterRequest>().Create();

            MockAuthService
                .Setup(mock => mock.RegisterUser(It.IsAny<UserRegisterRequest>()))
                .ReturnsAsync(false);

            var expectedMessage = new { Message = "User already exists." };

            //Act
            var result = await _authController.RegisterUser(userRegisterRequest);

            // Assert
            MockAuthService.Verify(mock => mock.RegisterUser(userRegisterRequest), Times.Once);
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

            MockAuthService
                .Setup(mock => mock.AuthenticateUser(It.IsAny<UserLoginRequest>()))
                .ReturnsAsync(jwtModel);

            //Act
            var result = await _authController.Login(userLoginRequest);

            // Assert
            MockAuthService.Verify(mock => mock.AuthenticateUser(userLoginRequest), Times.Once);
            result.Should().BeOfType<ActionResult<JWTModel>>();
            result?.Value?.Should().BeEquivalentTo(jwtModel);
        }

        [Test]
        public async Task AuthController_Login_Returns_Unauthorized()
        {
            // Arrange
            var userLoginRequest = Fixture.Build<UserLoginRequest>().Create();
            JWTModel jwtModel = null;

            MockAuthService
                .Setup(mock => mock.AuthenticateUser(It.IsAny<UserLoginRequest>()))
                .ReturnsAsync(jwtModel);

            //Act
            var result = await _authController.Login(userLoginRequest);

            // Assert
            MockAuthService.Verify(mock => mock.AuthenticateUser(userLoginRequest), Times.Once);
            result.Result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task AuthController_Refresh_Returns_Data()
        {
            // Arrange
            var refreshRequest = Fixture.Build<TokenRefreshModel>().Create();
            var jwtModel = Fixture.Build<JWTModel>().Create();

            MockAuthService
                .Setup(mock => mock.RefreshUserToken(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(jwtModel);

            //Act
            var result = await _authController.Refresh(refreshRequest);

            // Assert
            result.Should().BeOfType<ActionResult<JWTModel>>();
            result?.Value?.Should().BeEquivalentTo(jwtModel);
        }

        [Test]
        public async Task AuthController_Refresh_Returns_Bad_Request()
        {
            // Arrange
            var refreshRequest = Fixture.Build<TokenRefreshModel>().Create();
            JWTModel jwtModel = null;

            MockAuthService
                .Setup(mock => mock.RefreshUserToken(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(jwtModel);

            var expectedMessage = new { Message = "Invalid refresh request." };

            //Act
            var result = await _authController.Refresh(refreshRequest);

            // Assert
            var objResult = result.Result as BadRequestObjectResult;
            objResult?.Value.Should().BeEquivalentTo(expectedMessage);
        }

        [Test]
        public async Task AuthController_Revoke_Returns_NoContent()
        {
            //Act
            var result = await _authController.Revoke();

            // Assert
            MockAuthService.Verify(mock => mock.ExpireUserRefreshToken(It.IsAny<string>()), Times.Once);
            result.Result.Should().BeOfType<NoContentResult>();
        }
    }
}
