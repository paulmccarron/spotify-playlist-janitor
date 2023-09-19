using AutoFixture;
using FluentAssertions;
using Moq;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Services;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class DatabaseUserServiceTests : TestBase
    {
        private DatabaseUserService _databaseUserService;
        private Mock<IDatabaseService> _databaseServiceMock;

        [SetUp]
        public void Init()
        {
            _databaseServiceMock = new Mock<IDatabaseService>();

            _databaseUserService = new DatabaseUserService(_databaseServiceMock.Object);
        }

        [Test]
        public async Task DatabaseUserService_GetUser_Returns_UserDataModel()
        {
            // Arrange
            var userName = "username";
            var userModel = Fixture.Build<UserDataModel>().Create();

            _databaseServiceMock
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userModel);

            //Act
            var result = await _databaseUserService.GetUser(userName);

            // Assert
            _databaseServiceMock.Verify(mock => mock.GetUser(userName), Times.Once);
            result.Should().BeEquivalentTo(userModel);
        }

        [Test]
        public async Task DatabaseUserService_GetUser_Returns_Null()
        {
            // Arrange
            var userName = "username";
            UserDataModel userModel = null;

            _databaseServiceMock
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userModel);

            //Act
            var result = await _databaseUserService.GetUser(userName);

            // Assert
            _databaseServiceMock.Verify(mock => mock.GetUser(userName), Times.Once);
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
            _databaseServiceMock.Verify(mock => mock.AddUser(userName, password), Times.Once);
        }
    }
}
