using AutoFixture;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.DataAccess.Entities;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using SpotifyPlaylistJanitorAPI.System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SpotifyPlaylistJanitorAPI.Tests
{
    public abstract class TestBase
    {
        protected IFixture Fixture { get; }

        //Http Mocks
        protected Mock<IHttpContextAccessor> MockHttpContextAccessor { get; set; }

        //Database mocks
        protected Mock<SpotifyPlaylistJanitorDatabaseContext> MockDbContext { get; set; }
        protected Mock<DbSet<Playlist>> MockDbSetPlaylist { get; set; }
        protected Mock<DbSet<Artist>> MockDbSetArtist { get; set; }
        protected Mock<DbSet<Album>> MockDbSetAlbum { get; set; }
        protected Mock<DbSet<Track>> MockDbSetTrack { get; set; }
        protected Mock<DbSet<DataAccess.Entities.Image>> MockDbSetImage { get; set; }
        protected Mock<DbSet<SkippedTrack>> MockDbSetSkipped { get; set; }
        protected Mock<DbSet<User>> MockDbSetUser { get; set; }

        //Services mocks
        protected Mock<ISecurityService> MockSecurityService { get; set; }
        protected Mock<ISpotifyService> MockSpotifyService { get; set; }
        protected Mock<IAuthService> MockAuthService { get; set; }
        protected Mock<IDatabaseService> MockDatabaseService { get; set; }
        protected Mock<IPlayingStateService> MockPlayingStateService { get; set; }
        protected Mock<IUserService> MockUserService { get; set; }
        protected Mock<ISpotifyClientService> MockSpotifyClientService { get; set; }

        //Client mocks
        protected Mock<ISpotifyClient> MockSpotifyClient;

        //Configuration mocks
        protected const string CLIENT_ID = "mockClientId";
        protected const string CLIENT_SECRET = "mockClientSecret-mockClientSecret";
        protected const string ACCESS_TOKEN = "ACCESS_TOKEN";
        protected const string REFRESH_TOKEN = "REFRESH_TOKEN";
        protected const string PASSWORD_HASH = "PASSWORD_HASH";
        
        protected IOptions<SpotifyOption> SpotifyOptions { get; set; }
        

        protected TestBase()
        {
            Fixture = new Fixture();
            Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public void Init()
        {
            //Http Mocks Setup
            MockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            MockHttpContextAccessor
                .Setup(x => x.HttpContext.RequestServices.GetService(typeof(IAuthenticationService)))
                .Returns(authenticationServiceMock.Object);

            var claim = new Claim(ClaimTypes.Name, "username");
            var identity = new ClaimsIdentity(new List<Claim>{ claim });
            var principal = new ClaimsPrincipal(identity);

            MockHttpContextAccessor
                .Setup(x => x.HttpContext.User)
                .Returns(principal);

            var authResult = AuthenticateResult.Success(
                new AuthenticationTicket(new ClaimsPrincipal(), null));

            authResult.Properties.StoreTokens(new[]
                {
                new AuthenticationToken { Name = "access_token", Value = "access_token_value" }
            });

            authenticationServiceMock
                .Setup(x => x.AuthenticateAsync(MockHttpContextAccessor.Object.HttpContext, It.IsAny<string>()))
                .ReturnsAsync(authResult);

            //Database mocks setup
            MockDbSetPlaylist = new Mock<DbSet<Playlist>>();
            MockDbSetArtist = new Mock<DbSet<Artist>>();
            MockDbSetAlbum = new Mock<DbSet<Album>>();
            MockDbSetTrack = new Mock<DbSet<Track>>();
            MockDbSetImage = new Mock<DbSet<DataAccess.Entities.Image>>();
            MockDbSetSkipped = new Mock<DbSet<SkippedTrack>>();
            MockDbSetUser = new Mock<DbSet<User>>();
            MockDbContext = new Mock<SpotifyPlaylistJanitorDatabaseContext>();

            MockDbSetPlaylist.AddIQueryables(new List<Playlist>().AsQueryable());
            MockDbContext
                .Setup(mock => mock.Playlists)
                .Returns(MockDbSetPlaylist.Object);

            MockDbSetArtist.AddIQueryables(new List<Artist>().AsQueryable());
            MockDbContext
                .Setup(mock => mock.Artists)
                .Returns(MockDbSetArtist.Object);

            MockDbSetAlbum.AddIQueryables(new List<Album>().AsQueryable());
            MockDbContext
                .Setup(mock => mock.Albums)
                .Returns(MockDbSetAlbum.Object);

            MockDbSetTrack.AddIQueryables(new List<Track>().AsQueryable());
            MockDbContext
                .Setup(mock => mock.Tracks)
                .Returns(MockDbSetTrack.Object);

            MockDbSetImage.AddIQueryables(new List<DataAccess.Entities.Image>().AsQueryable());
            MockDbContext
                .Setup(mock => mock.Images)
                .Returns(MockDbSetImage.Object);

            MockDbSetSkipped.AddIQueryables(new List<SkippedTrack>().AsQueryable());
            MockDbContext
                .Setup(mock => mock.SkippedTracks)
                .Returns(MockDbSetSkipped.Object);

            MockDbSetUser.AddIQueryables(new List<User>().AsQueryable());
            MockDbContext
                .Setup(mock => mock.Users)
                .Returns(MockDbSetUser.Object);


            //Services mocks setup
            MockSecurityService = new Mock<ISecurityService>();
            MockSpotifyService = new Mock<ISpotifyService>();
            MockAuthService = new Mock<IAuthService>();
            MockDatabaseService = new Mock<IDatabaseService>();
            MockPlayingStateService = new Mock<IPlayingStateService>();
            MockUserService = new Mock<IUserService>();
            MockSpotifyClientService = new Mock<ISpotifyClientService>();

            MockSecurityService
                .Setup(mock => mock.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            MockSecurityService
                .Setup(mock => mock.GenerateJSONWebToken(It.IsAny<UserModel>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(ACCESS_TOKEN);

            MockSecurityService
                .Setup(mock => mock.GenerateRefreshToken())
                .Returns(REFRESH_TOKEN);

            MockSecurityService
                .Setup(mock => mock.HashPasword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PASSWORD_HASH);

            MockSecurityService
                .Setup(mock => mock.GetPrincipalFromToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(principal);

            //Client mocks
            MockSpotifyClient = new Mock<ISpotifyClient>();

            //Configuration mocks setup
            SpotifyOptions = Options.Create(new SpotifyOption
                {
                    ClientId = CLIENT_ID,
                    ClientSecret = CLIENT_SECRET,
                });
            }

        protected void VerifyLog<T>(Mock<ILogger<T>> mockLogger, LogLevel logLevel, string message)
        {
            mockLogger.Verify(logger => logger.Log(
                It.Is<LogLevel>(level => level == logLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == message && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }
        //protected string HashString(string stringToHash)
        //{
        //    var salt = Encoding.ASCII.GetBytes(CLIENT_SECRET);
        //    var hash = Rfc2898DeriveBytes.Pbkdf2(
        //        Encoding.UTF8.GetBytes(stringToHash),
        //        salt,
        //        350000,
        //        HashAlgorithmName.SHA512,
        //        64);

        //    return Convert.ToHexString(hash);
        //}

        //protected string GenerateJSONWebToken(UserModel userInfo)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(CLIENT_SECRET));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name, userInfo.Username),
        //        new Claim(ClaimTypes.Role, userInfo.Role)
        //    };

        //    var token = new JwtSecurityToken(
        //        CLIENT_ID,
        //        CLIENT_ID,
        //        claims,
        //        expires: SystemTime.Now().AddHours(1),
        //        signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
