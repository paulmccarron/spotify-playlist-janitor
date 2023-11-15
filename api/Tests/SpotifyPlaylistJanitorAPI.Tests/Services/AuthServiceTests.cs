using Moq;
using SpotifyPlaylistJanitorAPI.Services;
using AutoFixture;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using FluentAssertions;
using SpotifyPlaylistJanitorAPI.System;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests : TestBase
    {
        private AuthService _authService;

        [SetUp]
        public void Init()
        {
            _authService = new AuthService(MockUserService.Object, MockSecurityService.Object, SpotifyOptions);

            SystemTime.SetDateTime(DateTime.Now);
        }

        [TearDown]
        public void Dispose()
        {
            SystemTime.ResetDateTime();
        }

        [Test]
        public async Task AuthService_AuthenticateUser_Returns_Admin_Data()
        {
            //Arrange
            var password = "test_password";
            var userLogin = Fixture.Build<UserLoginRequest>()
                .With(x => x.Email, USERNAME)
                .With(x => x.Password, SPOTIFY_USERNAME)
                .With(x => x.Password, password)
                .Create();

            var userDataModel = Fixture.Build<UserDataModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.PasswordHash, PASSWORD_HASH)
                .With(x => x.IsAdmin, true)
                .Create();

            var userModel = Fixture.Build<UserModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.Role, "Admin")
                .Create();

            MockUserService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            var expectedResult = new JWTModel
            {
                AccessToken = ACCESS_TOKEN,
                ExpiresIn = (int)TimeSpan.FromHours(1).TotalMilliseconds,
                RefreshToken = REFRESH_TOKEN,
            };

            //Act
            var result = await _authService.AuthenticateUser(userLogin);

            //Assert
            result.AccessToken.Should().BeEquivalentTo(ACCESS_TOKEN);
            result.ExpiresIn.Should().Be(expectedResult.ExpiresIn);
            result.RefreshToken.Should().BeEquivalentTo(REFRESH_TOKEN);
        }

        [Test]
        public async Task AuthService_AuthenticateUser_Returns_User_Data()
        {
            //Arrange
            var password = "test_password";
            var userLogin = Fixture.Build<UserLoginRequest>()
                .With(x => x.Email, USERNAME)
                .With(x => x.Password, SPOTIFY_USERNAME)
                .With(x => x.Password, password)
                .Create();

            var userDataModel = Fixture.Build<UserDataModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.PasswordHash, PASSWORD_HASH)
                .With(x => x.IsAdmin, false)
                .Create();

            var userModel = Fixture.Build<UserModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.Role, "User")
                .Create();

            MockUserService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            var expectedResult = new JWTModel
            {
                AccessToken = ACCESS_TOKEN,
                ExpiresIn = (int)TimeSpan.FromHours(1).TotalMilliseconds,
                RefreshToken = REFRESH_TOKEN,
            };

            //Act
            var result = await _authService.AuthenticateUser(userLogin);

            //Assert
            result.AccessToken.Should().BeEquivalentTo(ACCESS_TOKEN);
            result.ExpiresIn.Should().Be(expectedResult.ExpiresIn);
            result.RefreshToken.Should().BeEquivalentTo(REFRESH_TOKEN);
        }

        [Test]
        public async Task AuthService_AuthenticateUser_Returns_Null_For_Incorrect_Password()
        {
            //Arrange
            var userLogin = Fixture.Build<UserLoginRequest>()
                .With(x => x.Email, USERNAME)
                .With(x => x.Password, SPOTIFY_USERNAME)
                .With(x => x.Password, "incorrect_password")
                .Create();

            var userDataModel = Fixture.Build<UserDataModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.PasswordHash, PASSWORD_HASH)
                .With(x => x.IsAdmin, true)
                .Create();

            MockUserService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            MockSecurityService
                .Setup(mock => mock.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            //Act
            var result = await _authService.AuthenticateUser(userLogin);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task AuthService_AuthenticateUser_Returns_Null_For_Incorrect_User()
        {
            //Arrange
            UserDataModel userDataModel = null;
            var userLogin = Fixture.Build<UserLoginRequest>()
                .With(x => x.Email, USERNAME)
                .With(x => x.Password, SPOTIFY_USERNAME)
                .Create();

            MockUserService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            //Act
            var result = await _authService.AuthenticateUser(userLogin);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task AuthService_RegisterUser_Returns_True()
        {
            //Arrange
            UserDataModel user = null;
            var password = "test_password";

            var userLogin = Fixture.Build<UserRegisterRequest>()
                .With(x => x.Email, USERNAME)
                .With(x => x.SpotifyEmail, SPOTIFY_USERNAME)
                .With(x => x.Password, password)
                .Create();

            MockUserService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(user);

            //Act
            var result = await _authService.RegisterUser(userLogin);

            //Assert
            MockUserService.Verify(mock => mock.AddUser(USERNAME.ToLower(), SPOTIFY_USERNAME.ToLower(), PASSWORD_HASH), Times.Once);
            result.Should().BeTrue();
        }

        [Test]
        public async Task AuthService_RegisterUser_Returns_False()
        {
            //Arrange
            var password = "test_password";

            var userLogin = Fixture.Build<UserRegisterRequest>()
                .With(x => x.Email, USERNAME)
                .With(x => x.SpotifyEmail, SPOTIFY_USERNAME)
                .With(x => x.Password, password)
                .Create();

            var userDataModel = Fixture.Build<UserDataModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, USERNAME)
                .With(x => x.PasswordHash, PASSWORD_HASH)
                .With(x => x.IsAdmin, false)
                .Create();

            MockUserService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            //Act
            var result = await _authService.RegisterUser(userLogin);

            //Assert
            MockUserService.Verify(mock => mock.AddUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            result.Should().BeFalse();
        }

        [Test]
        public async Task AuthService_RefreshUserToken_Returns_Admin_Data()
        {
            //Arrange
            var userModel = Fixture.Build<UserModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.Role, "Admin")
                .Create();

            var refreshTokenExpiry = SystemTime.Now().AddHours(1);

            var userDataModel = Fixture.Build<UserDataModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.PasswordHash, PASSWORD_HASH)
                .With(x => x.IsAdmin, true)
                .With(x => x.RefreshToken, REFRESH_TOKEN)
                .With(x => x.RefreshTokenExpiry, refreshTokenExpiry)
                .Create();

            MockUserService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            var expectedResult = new JWTModel
            {
                AccessToken = ACCESS_TOKEN,
                ExpiresIn = (int)TimeSpan.FromHours(1).TotalMilliseconds,
                RefreshToken = REFRESH_TOKEN,
            };

            //Act
            var result = await _authService.RefreshUserToken(ACCESS_TOKEN, REFRESH_TOKEN);

            //Assert
            MockUserService.Verify(mock => mock.SetUserRefreshToken(USERNAME, REFRESH_TOKEN, SystemTime.Now().AddHours(1)), Times.Once);
            result.AccessToken.Should().BeEquivalentTo(expectedResult.AccessToken);
            result.ExpiresIn.Should().Be(expectedResult.ExpiresIn);
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public async Task AuthService_RefreshUserToken_Returns_User_Data()
        {
            //Arrange
            var spotifyUsername = "spotifyUsername";

            var userModel = Fixture.Build<UserModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.Role, "User")
                .Create();

            var refreshTokenExpiry = SystemTime.Now().AddHours(1);

            var userDataModel = Fixture.Build<UserDataModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.PasswordHash, PASSWORD_HASH)
                .With(x => x.IsAdmin, false)
                .With(x => x.RefreshToken, REFRESH_TOKEN)
                .With(x => x.RefreshTokenExpiry, refreshTokenExpiry)
                .Create();

            MockUserService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            var expectedResult = new JWTModel
            {
                AccessToken = ACCESS_TOKEN,
                ExpiresIn = (int)TimeSpan.FromHours(1).TotalMilliseconds,
                RefreshToken = REFRESH_TOKEN,
            };

            //Act
            var result = await _authService.RefreshUserToken(ACCESS_TOKEN, REFRESH_TOKEN);

            //Assert
            MockUserService.Verify(mock => mock.SetUserRefreshToken(USERNAME, REFRESH_TOKEN, SystemTime.Now().AddHours(1)), Times.Once);
            result.AccessToken.Should().BeEquivalentTo(expectedResult.AccessToken);
            result.ExpiresIn.Should().Be(expectedResult.ExpiresIn);
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public async Task AuthService_RefreshUserToken_Returns_Null_When_No_Username()
        {
            //Arrange
            var userModel = Fixture.Build<UserModel>()
                .With(x => x.Username, string.Empty)
                .With(x => x.SpotifyUsername, string.Empty)
                .With(x => x.Role, "User")
                .Create();

            var refreshTokenExpiry = SystemTime.Now().AddHours(1);

            //Act
            var result = await _authService.RefreshUserToken(ACCESS_TOKEN, REFRESH_TOKEN);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task AuthService_RefreshUserToken_Returns_Null_When_User_Wrong()
        {
            //Arrange
            UserDataModel? userDataModel = null;

            MockUserService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            //Act
            var result = await _authService.RefreshUserToken(ACCESS_TOKEN, REFRESH_TOKEN);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task AuthService_RefreshUserToken_Returns_Null_When_Refresh_Token_Wrong()
        {
            //Arrange
            var userModel = Fixture.Build<UserModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.Role, "User")
                .Create();

            var refreshTokenExpiry = SystemTime.Now().AddHours(1);

            var userDataModel = Fixture.Build<UserDataModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.PasswordHash, PASSWORD_HASH)
                .With(x => x.IsAdmin, false)
                .With(x => x.RefreshTokenExpiry, refreshTokenExpiry)
                .Create();

            MockUserService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            //Act
            var result = await _authService.RefreshUserToken(ACCESS_TOKEN, REFRESH_TOKEN);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task AuthService_RefreshUserToken_Returns_Null_When_Refresh_Token_Expired()
        {
            //Arrange
            var password = "test_password";

            var userModel = Fixture.Build<UserModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.Role, "User")
                .Create();

            var refreshTokenExpiry = SystemTime.Now().AddHours(-1);

            var userDataModel = Fixture.Build<UserDataModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.PasswordHash, PASSWORD_HASH)
                .With(x => x.IsAdmin, false)
                .With(x => x.RefreshToken, REFRESH_TOKEN)
                .With(x => x.RefreshTokenExpiry, refreshTokenExpiry)
                .Create();

            MockUserService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            //Act
            var result = await _authService.RefreshUserToken(ACCESS_TOKEN, REFRESH_TOKEN);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task AuthService_ExpireUserRefreshToken_Returns_Task()
        {
            //Arrange
            var userDataModel = Fixture.Build<UserDataModel>()
                .With(x => x.Username, USERNAME)
                .Create();

            MockUserService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            //Act
            await _authService.ExpireUserRefreshToken(USERNAME);

            //Assert
            MockUserService.Verify(mock => mock.ExpireUserRefreshToken(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task AuthService_ExpireUserRefreshToken_Skips_And_Returns_Task()
        {
            //Arrange
            UserDataModel userDataModel = null;

            MockUserService
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            //Act
            await _authService.ExpireUserRefreshToken(USERNAME);

            //Assert
            MockUserService.Verify(mock => mock.ExpireUserRefreshToken(It.IsAny<string>()), Times.Never);
        }
    }
}
