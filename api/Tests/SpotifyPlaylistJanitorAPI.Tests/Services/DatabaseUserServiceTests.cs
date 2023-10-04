using AutoFixture;
using FluentAssertions;
using Moq;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Services;
using SpotifyPlaylistJanitorAPI.System;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class DatabaseUserServiceTests : TestBase
    {
        private DatabaseUserService _databaseUserService;

        [SetUp]
        public void Init()
        {

            _databaseUserService = new DatabaseUserService(MockDatabaseService.Object, MockSecurityService.Object, SpotifyOptions);
        }

        [Test]
        public async Task DatabaseUserService_GetUser_Returns_UserDataModel()
        {
            // Arrange
            var userName = "username";
            var userModel = Fixture.Build<UserDataModel>().Create();

            MockDatabaseService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userModel);

            //Act
            var result = await _databaseUserService.GetUser(userName);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetUser(userName), Times.Once);
            result.Should().BeEquivalentTo(userModel);
        }

        [Test]
        public async Task DatabaseUserService_GetUser_Returns_Null()
        {
            // Arrange
            var userName = "username";
            UserDataModel userModel = null;

            MockDatabaseService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userModel);

            //Act
            var result = await _databaseUserService.GetUser(userName);

            // Assert
            MockDatabaseService.Verify(mock => mock.GetUser(userName), Times.Once);
            result.Should().BeNull();
        }

        [Test]
        public async Task DatabaseUserService_AddUser_Returns_Task()
        {
            // Arrange
            var userName = "username";
            var password = "password";

            //Act
            await _databaseUserService.AddUser(userName, password);

            // Assert
            MockDatabaseService.Verify(mock => mock.AddUser(userName, password), Times.Once);
        }

        [Test]
        public async Task DatabaseUserService_SetUserRefreshToken_Returns_Task()
        {
            // Arrange
            var userName = "username";
            var refreshToken = "refreshToken";
            var tokenExpiry = SystemTime.Now();

            //Act
            await _databaseUserService.SetUserRefreshToken(userName, refreshToken, tokenExpiry);

            // Assert
            MockDatabaseService.Verify(mock => mock.UpdateUserRefreshToken(userName, refreshToken, tokenExpiry), Times.Once);
        }

        [Test]
        public async Task DatabaseUserService_ExpireUserRefreshToken_Returns_Task()
        {
            // Arrange
            var userName = "username";

            //Act
            await _databaseUserService.ExpireUserRefreshToken(userName);

            // Assert
            MockDatabaseService.Verify(mock => mock.UpdateUserRefreshToken(userName, null, null), Times.Once);
        }

        [Test]
        public async Task DatabaseUserService_AddUserSpotifyToken_Adds_Encoded_String_To_Database_Returns_Task()
        {
            // Arrange
            var userName = "username";
            var tokenString = "tokenString";

            //Act
            await _databaseUserService.AddUserSpotifyToken(userName, tokenString);

            // Assert
            MockDatabaseService.Verify(mock => mock.AddUserEncodedSpotifyToken(userName, null), Times.Once);
        }
    }
}
