using Moq;
using SpotifyPlaylistJanitorAPI.Services;
using AutoFixture;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using FluentAssertions;
using SpotifyPlaylistJanitorAPI.System;
using System.Security.Claims;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class SecurityServiceTests : TestBase
    {
        private SecurityService _securityService;

        [SetUp]
        public void Init()
        {
            _securityService = new SecurityService();

            SystemTime.SetDateTime(new DateTime(2022, 12, 4, 0, 0, 0));
        }

        [TearDown]
        public void Dispose()
        {
            SystemTime.ResetDateTime();
        }

        [Test]
        public void SecurityService_GenerateJSONWebToken_Returns_Data()
        {
            //Arrange
            var userModel = Fixture.Build<UserModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .With(x => x.Role, "Admin")
                .Create();

            var now = SystemTime.Now();

            var expectedResult = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiVVNFUk5BTUUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoiU1BPVElGWV9VU0VSTkFNRSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNjcwMTE1NjAwLCJpc3MiOiJtb2NrQ2xpZW50SWQiLCJhdWQiOiJtb2NrQ2xpZW50SWQifQ.qvPyq_ddZz-mPLNYAyJpDSafKxuwd8_Cv6Xb3-s_NEo";

            //Act
            var result = _securityService.GenerateJSONWebToken(userModel, now, CLIENT_ID, CLIENT_ID, CLIENT_SECRET);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void SecurityService_GenerateRefreshToken_Returns_Data()
        {
            //Act
            var result = _securityService.GenerateRefreshToken();

            //Assert
            result.Length.Should().BeGreaterThan(0);
        }

        [Test]
        public void SecurityService_HashPasword_Returns_Data()
        {
            //Arrange
            var password = "password";

            var expectedResult = "BA23B02ABEAB0D74C414C5638B1D0B99E16C070A982A3851FFD6F685F57A97568C6B830F44F13677477AD0216077BFA3EE04DE5B91D4C57F2B44C6A3769994B6";

            //Act
            var result = _securityService.HashPasword(password, CLIENT_SECRET);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void SecurityService_VerifyPassword_Returns_True()
        {
            //Arrange
            var password = "password";
            var hashedPassword = "BA23B02ABEAB0D74C414C5638B1D0B99E16C070A982A3851FFD6F685F57A97568C6B830F44F13677477AD0216077BFA3EE04DE5B91D4C57F2B44C6A3769994B6";

            //Act
            var result = _securityService.VerifyPassword(password, hashedPassword, CLIENT_SECRET);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void SecurityService_VerifyPassword_Returns_False()
        {
            //Arrange
            var password = "password";
            var hashedPassword = "BA23B02ABEAB0D74C414C5638B1D0B99E16C070A982A3851FFD6F685F57A97568C6B830F44F13677477AD0216077BFA3EE04DE5B91D4C57F2B44C6A3769994B7";

            //Act
            var result = _securityService.VerifyPassword(password, hashedPassword, CLIENT_SECRET);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void SecurityService_GetPrincipalFromToken_Returns_Data()
        {
            //Arrange
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlcm5hbWUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTY3MDExNTYwMCwiaXNzIjoibW9ja0NsaWVudElkIiwiYXVkIjoibW9ja0NsaWVudElkIn0.jOfrscbtoBjC_Z6TMqerSBrQWSI6deFXOlazdlXHCCY";

            var name = new Claim(ClaimTypes.Name, "username");
            var role = new Claim(ClaimTypes.Role, "Admin");
            var claims = new ClaimsIdentity(new List<Claim> {
                name,
                role
            });
            var expectedPrincipal = new ClaimsPrincipal(claims);
            
            //Act
            var result = _securityService.GetPrincipalFromToken(token, CLIENT_SECRET);

            //Assert
            result.Claims.Should().ContainSingle(x => x.Type == name.Type && x.Value == name.Value);
            result.Claims.Should().ContainSingle(x => x.Type == role.Type && x.Value == role.Value);
        }

        [Test]
        public void SecurityService_EncryptString_Returns_False()
        {
            //Arrange
            var plainString = "plain";
            var encryptedString = "PaetJ1lhEVbkVBp9w3Z5Rw==";

            //Act
            var result = _securityService.EncryptString(plainString, "00000000000000000000000000000000");

            //Assert
            result.Should().BeEquivalentTo(encryptedString);
        }

        [Test]
        public void SecurityService_DecryptString_Returns_False()
        {
            //Arrange
            var encryptedString = "PaetJ1lhEVbkVBp9w3Z5Rw==";
            var plainString = "plain";

            //Act
            var result = _securityService.DecryptString(encryptedString, "00000000000000000000000000000000");

            //Assert
            result.Should().BeEquivalentTo(plainString);
        }

        //[Test]
        //public async Task AuthService_AuthenticateUser_Returns_User_Data()
        //{
        //    //Arrange
        //    var username = "username";
        //    var password = "test_password";
        //    var userLogin = Fixture.Build<UserLoginRequest>()
        //        .With(x => x.Email, username)
        //        .With(x => x.Password, password)
        //        .Create();

        //    var userDataModel = Fixture.Build<UserDataModel>()
        //        .With(x => x.Username, username)
        //        .With(x => x.PasswordHash, PASSWORD_HASH)
        //        .With(x => x.IsAdmin, false)
        //        .Create();

        //    var userModel = Fixture.Build<UserModel>()
        //        .With(x => x.Username, username)
        //        .With(x => x.Role, "User")
        //        .Create();

        //    MockUserService
        //        .Setup(mock => mock.GetUser(It.IsAny<string>()))
        //        .ReturnsAsync(userDataModel);

        //    var expectedResult = new JWTModel
        //    {
        //        AccessToken = ACCESS_TOKEN,
        //        ExpiresIn = (int)TimeSpan.FromHours(1).TotalMilliseconds,
        //        RefreshToken = REFRESH_TOKEN,
        //    };

        //    //Act
        //    var result = await _authService.AuthenticateUser(userLogin);

        //    //Assert
        //    result.AccessToken.Should().BeEquivalentTo(ACCESS_TOKEN);
        //    result.ExpiresIn.Should().Be(expectedResult.ExpiresIn);
        //    result.RefreshToken.Should().BeEquivalentTo(REFRESH_TOKEN);
        //}

        //[Test]
        //public async Task AuthService_AuthenticateUser_Returns_Null_For_Incorrect_Password()
        //{
        //    //Arrange
        //    var username = "username";
        //    var password = "test_password";
        //    var userLogin = Fixture.Build<UserLoginRequest>()
        //        .With(x => x.Email, username)
        //        .With(x => x.Password, "incorrect_password")
        //        .Create();

        //    var userDataModel = Fixture.Build<UserDataModel>()
        //        .With(x => x.Username, username)
        //        .With(x => x.PasswordHash, PASSWORD_HASH)
        //        .With(x => x.IsAdmin, true)
        //        .Create();

        //    MockUserService
        //        .Setup(mock => mock.GetUser(It.IsAny<string>()))
        //        .ReturnsAsync(userDataModel);

        //    MockSecurityService
        //        .Setup(mock => mock.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //        .Returns(false);

        //    //Act
        //    var result = await _authService.AuthenticateUser(userLogin);

        //    //Assert
        //    result.Should().BeNull();
        //}

        //[Test]
        //public async Task AuthService_AuthenticateUser_Returns_Null_For_Incorrect_User()
        //{
        //    //Arrange
        //    UserDataModel userDataModel = null;
        //    var username = "username";
        //    var password = "test_password";
        //    var userLogin = Fixture.Build<UserLoginRequest>()
        //        .With(x => x.Email, username)
        //        .With(x => x.Password, password)
        //        .Create();

        //    MockUserService
        //        .Setup(mock => mock.GetUser(It.IsAny<string>()))
        //        .ReturnsAsync(userDataModel);

        //    //Act
        //    var result = await _authService.AuthenticateUser(userLogin);

        //    //Assert
        //    result.Should().BeNull();
        //}

        //[Test]
        //public async Task AuthService_RegisterUser_Returns_True()
        //{
        //    //Arrange
        //    UserDataModel user = null;
        //    var username = "username";
        //    var password = "test_password";

        //    var userLogin = Fixture.Build<UserLoginRequest>()
        //        .With(x => x.Email, username)
        //        .With(x => x.Password, password)
        //        .Create();

        //    MockUserService
        //        .Setup(mock => mock.GetUser(It.IsAny<string>()))
        //        .ReturnsAsync(user);

        //    //Act
        //    var result = await _authService.RegisterUser(userLogin);

        //    //Assert
        //    MockUserService.Verify(mock => mock.AddUser(username, PASSWORD_HASH), Times.Once);
        //    result.Should().BeTrue();
        //}

        //[Test]
        //public async Task AuthService_RegisterUser_Returns_False()
        //{
        //    //Arrange
        //    var username = "username";
        //    var password = "test_password";

        //    var userLogin = Fixture.Build<UserLoginRequest>()
        //        .With(x => x.Email, username)
        //        .With(x => x.Password, password)
        //        .Create();

        //    var userDataModel = Fixture.Build<UserDataModel>()
        //        .With(x => x.Username, username)
        //        .With(x => x.PasswordHash, PASSWORD_HASH)
        //        .With(x => x.IsAdmin, false)
        //        .Create();

        //    MockUserService
        //        .Setup(mock => mock.GetUser(It.IsAny<string>()))
        //        .ReturnsAsync(userDataModel);

        //    //Act
        //    var result = await _authService.RegisterUser(userLogin);

        //    //Assert
        //    MockUserService.Verify(mock => mock.AddUser(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        //    result.Should().BeFalse();
        //}

        //[Test]
        //public async Task AuthService_RefreshUserToken_Returns_Admin_Data()
        //{
        //    //Arrange
        //    var username = "username";

        //    var userModel = Fixture.Build<UserModel>()
        //        .With(x => x.Username, username)
        //        .With(x => x.Role, "Admin")
        //        .Create();

        //    var refreshTokenExpiry = SystemTime.Now().AddHours(1);

        //    var userDataModel = Fixture.Build<UserDataModel>()
        //        .With(x => x.Username, username)
        //        .With(x => x.PasswordHash, PASSWORD_HASH)
        //        .With(x => x.IsAdmin, true)
        //        .With(x => x.RefreshToken, REFRESH_TOKEN)
        //        .With(x => x.RefreshTokenExpiry, refreshTokenExpiry)
        //        .Create();

        //    MockUserService
        //        .Setup(mock => mock.GetUser(It.IsAny<string>()))
        //        .ReturnsAsync(userDataModel);

        //    var expectedResult = new JWTModel
        //    {
        //        AccessToken = ACCESS_TOKEN,
        //        ExpiresIn = (int)TimeSpan.FromHours(1).TotalMilliseconds,
        //        RefreshToken = REFRESH_TOKEN,
        //    };

        //    //Act
        //    var result = await _authService.RefreshUserToken(ACCESS_TOKEN, REFRESH_TOKEN);

        //    //Assert
        //    MockUserService.Verify(mock => mock.SetUserRefreshToken(username, REFRESH_TOKEN, SystemTime.Now().AddHours(1)), Times.Once);
        //    result.AccessToken.Should().BeEquivalentTo(expectedResult.AccessToken);
        //    result.ExpiresIn.Should().Be(expectedResult.ExpiresIn);
        //    result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        //}

        //[Test]
        //public async Task AuthService_RefreshUserToken_Returns_User_Data()
        //{
        //    //Arrange
        //    var username = "username";

        //    var userModel = Fixture.Build<UserModel>()
        //        .With(x => x.Username, username)
        //        .With(x => x.Role, "User")
        //        .Create();

        //    var refreshTokenExpiry = SystemTime.Now().AddHours(1);

        //    var userDataModel = Fixture.Build<UserDataModel>()
        //        .With(x => x.Username, username)
        //        .With(x => x.PasswordHash, PASSWORD_HASH)
        //        .With(x => x.IsAdmin, false)
        //        .With(x => x.RefreshToken, REFRESH_TOKEN)
        //        .With(x => x.RefreshTokenExpiry, refreshTokenExpiry)
        //        .Create();

        //    MockUserService
        //        .Setup(mock => mock.GetUser(It.IsAny<string>()))
        //        .ReturnsAsync(userDataModel);

        //    var expectedResult = new JWTModel
        //    {
        //        AccessToken = ACCESS_TOKEN,
        //        ExpiresIn = (int)TimeSpan.FromHours(1).TotalMilliseconds,
        //        RefreshToken = REFRESH_TOKEN,
        //    };

        //    //Act
        //    var result = await _authService.RefreshUserToken(ACCESS_TOKEN, REFRESH_TOKEN);

        //    //Assert
        //    MockUserService.Verify(mock => mock.SetUserRefreshToken(username, REFRESH_TOKEN, SystemTime.Now().AddHours(1)), Times.Once);
        //    result.AccessToken.Should().BeEquivalentTo(expectedResult.AccessToken);
        //    result.ExpiresIn.Should().Be(expectedResult.ExpiresIn);
        //    result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        //}

        //[Test]
        //public async Task AuthService_RefreshUserToken_Returns_Null_When_No_Username()
        //{
        //    //Arrange
        //    var userModel = Fixture.Build<UserModel>()
        //        .With(x => x.Username, string.Empty)
        //        .With(x => x.Role, "User")
        //        .Create();

        //    var refreshTokenExpiry = SystemTime.Now().AddHours(1);

        //    //Act
        //    var result = await _authService.RefreshUserToken(ACCESS_TOKEN, REFRESH_TOKEN);

        //    //Assert
        //    result.Should().BeNull();
        //}

        //[Test]
        //public async Task AuthService_RefreshUserToken_Returns_Null_When_User_Wrong()
        //{
        //    //Arrange
        //    UserDataModel? userDataModel = null;

        //    MockUserService
        //        .Setup(mock => mock.GetUser(It.IsAny<string>()))
        //        .ReturnsAsync(userDataModel);

        //    //Act
        //    var result = await _authService.RefreshUserToken(ACCESS_TOKEN, REFRESH_TOKEN);

        //    //Assert
        //    result.Should().BeNull();
        //}

        //[Test]
        //public async Task AuthService_RefreshUserToken_Returns_Null_When_Refresh_Token_Wrong()
        //{
        //    //Arrange
        //    var username = "username";

        //    var userModel = Fixture.Build<UserModel>()
        //        .With(x => x.Username, username)
        //        .With(x => x.Role, "User")
        //        .Create();

        //    var refreshTokenExpiry = SystemTime.Now().AddHours(1);

        //    var userDataModel = Fixture.Build<UserDataModel>()
        //        .With(x => x.Username, username)
        //        .With(x => x.PasswordHash, PASSWORD_HASH)
        //        .With(x => x.IsAdmin, false)
        //        .With(x => x.RefreshTokenExpiry, refreshTokenExpiry)
        //        .Create();

        //    MockUserService
        //        .Setup(mock => mock.GetUser(It.IsAny<string>()))
        //        .ReturnsAsync(userDataModel);

        //    //Act
        //    var result = await _authService.RefreshUserToken(ACCESS_TOKEN, REFRESH_TOKEN);

        //    //Assert
        //    result.Should().BeNull();
        //}

        //[Test]
        //public async Task AuthService_RefreshUserToken_Returns_Null_When_Refresh_Token_Expired()
        //{
        //    //Arrange
        //    var username = "username";
        //    var password = "test_password";

        //    var userModel = Fixture.Build<UserModel>()
        //        .With(x => x.Username, username)
        //        .With(x => x.Role, "User")
        //        .Create();

        //    var refreshTokenExpiry = SystemTime.Now().AddHours(-1);

        //    var userDataModel = Fixture.Build<UserDataModel>()
        //        .With(x => x.Username, username)
        //        .With(x => x.PasswordHash, PASSWORD_HASH)
        //        .With(x => x.IsAdmin, false)
        //        .With(x => x.RefreshToken, REFRESH_TOKEN)
        //        .With(x => x.RefreshTokenExpiry, refreshTokenExpiry)
        //        .Create();

        //    MockUserService
        //        .Setup(mock => mock.GetUser(It.IsAny<string>()))
        //        .ReturnsAsync(userDataModel);

        //    //Act
        //    var result = await _authService.RefreshUserToken(ACCESS_TOKEN, REFRESH_TOKEN);

        //    //Assert
        //    result.Should().BeNull();
        //}

        //[Test]
        //public async Task AuthService_ExpireUserRefreshToken_Returns_Task()
        //{
        //    //Arrange
        //    var username = "username";

        //    var userDataModel = Fixture.Build<UserDataModel>()
        //        .With(x => x.Username, username)
        //        .Create();

        //    MockUserService
        //        .Setup(mock => mock.GetUser(It.IsAny<string>()))
        //        .ReturnsAsync(userDataModel);

        //    //Act
        //    await _authService.ExpireUserRefreshToken(username);

        //    //Assert
        //    MockUserService.Verify(mock => mock.ExpireUserRefreshToken(It.IsAny<string>()), Times.Once);
        //}

        //[Test]
        //public async Task AuthService_ExpireUserRefreshToken_Skips_And_Returns_Task()
        //{
        //    //Arrange
        //    var username = "username";

        //    UserDataModel userDataModel = null;

        //    MockUserService
        //        .Setup(mock => mock.GetUser(It.IsAny<string>()))
        //        .ReturnsAsync(userDataModel);

        //    //Act
        //    await _authService.ExpireUserRefreshToken(username);

        //    //Assert
        //    MockUserService.Verify(mock => mock.ExpireUserRefreshToken(It.IsAny<string>()), Times.Never);
        //}
    }
}
