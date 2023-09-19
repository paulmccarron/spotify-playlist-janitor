using Moq;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using SpotifyPlaylistJanitorAPI.Services;
using System.Text;
using Microsoft.Extensions.Options;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using System.Security.Cryptography;
using AutoFixture;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using SpotifyPlaylistJanitorAPI.System;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests : TestBase
    {
        private AuthService _authService;
        private Mock<IUserService> _userServiceMock;
        private IOptions<SpotifyOption> _spotifyOptions;
        private const string CLIENT_ID = "mockClientId";
        private const string CLIENT_SECRET = "mockClientSecret-mockClientSecret";

        [SetUp]
        public void Init()
        {
            _userServiceMock = new Mock<IUserService>();
            _spotifyOptions = Options.Create(new SpotifyOption
            {
                ClientId = CLIENT_ID,
                ClientSecret = CLIENT_SECRET,
            });

            _authService = new AuthService(_userServiceMock.Object, _spotifyOptions);

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
            var username = "username";
            var password = "test_password";
            var userLogin = Fixture.Build<UserLoginRequest>()
                .With(x => x.Username, username)
                .With(x => x.Password, password)
                .Create();

            var userDataModel = Fixture.Build<UserDataModel>()
                .With(x => x.UserName, username)
                .With(x => x.PasswordHash, HashString(password))
                .With(x => x.IsAdmin, true)
                .Create();

            var userModel = Fixture.Build<UserModel>()
                .With(x => x.Username, username)
                .With(x => x.Role, "Admin")
                .Create();

            _userServiceMock
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            var expectedResult = new JWTModel
            {
                Token = GenerateJSONWebToken(userModel)
            };

            //Act
            var result = await _authService.AuthenticateUser(userLogin);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task AuthService_AuthenticateUser_Returns_User_Data()
        {
            //Arrange
            var username = "username";
            var password = "test_password";
            var userLogin = Fixture.Build<UserLoginRequest>()
                .With(x => x.Username, username)
                .With(x => x.Password, password)
                .Create();

            var userDataModel = Fixture.Build<UserDataModel>()
                .With(x => x.UserName, username)
                .With(x => x.PasswordHash, HashString(password))
                .With(x => x.IsAdmin, false)
                .Create();

            var userModel = Fixture.Build<UserModel>()
                .With(x => x.Username, username)
                .With(x => x.Role, "User")
                .Create();

            _userServiceMock
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            var expectedResult = new JWTModel
            {
                Token = GenerateJSONWebToken(userModel)
            };

            //Act
            var result = await _authService.AuthenticateUser(userLogin);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task AuthService_AuthenticateUser_Returns_Null_For_Incorrect_Password()
        {
            //Arrange
            var username = "username";
            var password = "test_password";
            var userLogin = Fixture.Build<UserLoginRequest>()
                .With(x => x.Username, username)
                .With(x => x.Password, "incorrect_password")
                .Create();

            var userDataModel = Fixture.Build<UserDataModel>()
                .With(x => x.UserName, username)
                .With(x => x.PasswordHash, HashString(password))
                .With(x => x.IsAdmin, true)
                .Create();

            _userServiceMock
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

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
            var username = "username";
            var password = "test_password";
            var userLogin = Fixture.Build<UserLoginRequest>()
                .With(x => x.Username, username)
                .With(x => x.Password, password)
                .Create();

            _userServiceMock
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
            var username = "username";
            var password = "test_password";
            var passwordHash = HashString(password);

            var userLogin = Fixture.Build<UserLoginRequest>()
                .With(x => x.Username, username)
                .With(x => x.Password, password)
                .Create();

            _userServiceMock
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(user);

            //Act
            var result = await _authService.RegisterUser(userLogin);

            //Assert
            _userServiceMock.Verify(mock => mock.AddUser(username, passwordHash), Times.Once);
            result.Should().BeTrue();
        }

        [Test]
        public async Task AuthService_RegisterUser_Returns_False()
        {
            //Arrange
            var username = "username";
            var password = "test_password";

            var userLogin = Fixture.Build<UserLoginRequest>()
                .With(x => x.Username, username)
                .With(x => x.Password, password)
                .Create();

            var userDataModel = Fixture.Build<UserDataModel>()
                .With(x => x.UserName, username)
                .With(x => x.PasswordHash, HashString(password))
                .With(x => x.IsAdmin, false)
                .Create();

            _userServiceMock
                .Setup(mock => mock.GetUser(It.IsAny<string>()))
                .ReturnsAsync(userDataModel);

            //Act
            var result = await _authService.RegisterUser(userLogin);

            //Assert
            _userServiceMock.Verify(mock => mock.AddUser(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            result.Should().BeFalse();
        }

        private string HashString(string stringToHash)
        {
            var salt = Encoding.ASCII.GetBytes(CLIENT_SECRET);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(stringToHash),
                salt,
                350000,
                HashAlgorithmName.SHA512,
                64);

            return Convert.ToHexString(hash);
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(CLIENT_SECRET));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userInfo.Username),
                new Claim(ClaimTypes.Role,userInfo.Role)
            };

            var token = new JwtSecurityToken(
                CLIENT_ID,
                CLIENT_ID,
                claims,
                expires: SystemTime.Now().AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
