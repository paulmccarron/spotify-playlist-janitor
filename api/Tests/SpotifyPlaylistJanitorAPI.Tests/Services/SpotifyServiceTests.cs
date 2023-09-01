using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Exceptions;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models;
using SpotifyPlaylistJanitorAPI.Services;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    public class SpotifyServiceTests : TestBase
    {
        private SpotifyService _spotifyService;
        private IOptions<SpotifyOption> _spotifyOptions;
        private Mock<ISpotifyClient> _spotifyClientMock;

        public SpotifyServiceTests()
        {
            _spotifyClientMock = new Mock<ISpotifyClient>();
            _spotifyOptions = Options.Create(new SpotifyOption
            {
                ClientId = "mockClientId",
                ClientSecret = "mockClientSecret",
            });

            _spotifyService = new SpotifyService(_spotifyOptions);
        }

        [SetUp]
        public void Setup()
        {
            _spotifyService.SetClient(_spotifyClientMock.Object);
        }

        [Test]
        public void SpotifyService_IsLoggedIn_Defaults_to_False()
        {
            _spotifyService.SetClient(null);
            // Assert
            _spotifyService.IsLoggedIn.Should().BeFalse();
        }

        [Test]
        public void SpotifyService_SetClient_Sets_IsLoggedIn_True()
        {
            // Assert
            _spotifyService.IsLoggedIn.Should().BeTrue();
        }

        [Test]
        public async Task SpotifyService_GetCurrentUser_Returns_Data()
        {
            //Arrange
            var userId = "userId";
            var userDisplayName = "userDisplayName";
            var userEmail = "user@gmail.com";
            var userHref = "www.spotify.com/href";

            var mockUserProfile = new Mock<IUserProfileClient>();
            mockUserProfile
                .Setup(mock => mock.Current(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PrivateUser
                {
                    Id = userId,
                    DisplayName = userDisplayName,
                    Email = userEmail,
                    Href = userHref,
                });

            _spotifyClientMock
                .Setup(mock => mock.UserProfile)
                .Returns(mockUserProfile.Object);

            //Act
            var result = await _spotifyService.GetCurrentUser();

            // Assert
            result.Should().BeOfType<SpotifyUserModel>();
            result.Id.Should().Be(userId);
            result.DisplayName.Should().Be(userDisplayName);
            result.Email.Should().Be(userEmail);
            result.Href.Should().Be(userHref);
        }

        [Test]
        public void SpotifyService_GetCurrentUser_Throws_Exception_If_No_Spotify_Client_Configured()
        {
            //Arrange
            _spotifyService.SetClient(null);

            //Act
            var ex = Assert.ThrowsAsync<SpotifyArgumentException>(_spotifyService.GetCurrentUser);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No Spotify Client configured"));
        }

        [Test]
        public void SpotifyService_CheckSpotifyCredentials_Throws_Exception_If_No_Spotify_Client_Id_And_Secret_Configured()
        {
            //Arrange
            _spotifyOptions = Options.Create(new SpotifyOption
            {
                ClientId = "",
                ClientSecret = "",
            });

            _spotifyService = new SpotifyService(_spotifyOptions);

            //Act
            var ex = Assert.Throws<SpotifyArgumentException>(_spotifyService.CheckSpotifyCredentials);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No Spotify ClientId or ClientSecret configured"));
        }

        [Test]
        public void SpotifyService_CheckSpotifyCredentials_Throws_Exception_If_No_Spotify_Client_Id_Configured()
        {
            //Arrange
            _spotifyOptions = Options.Create(new SpotifyOption
            {
                ClientId = "",
                ClientSecret = "mockClientSecret",
            });

            _spotifyService = new SpotifyService(_spotifyOptions);

            //Act
            var ex = Assert.Throws<SpotifyArgumentException>(_spotifyService.CheckSpotifyCredentials);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No Spotify ClientId configured"));
        }

        [Test]
        public void SpotifyService_CheckSpotifyCredentials_Throws_Exception_If_No_Spotify_Client_Secret_Configured()
        {
            //Arrange
            _spotifyOptions = Options.Create(new SpotifyOption
            {
                ClientId = "mockClientId",
                ClientSecret = "",
            });

            _spotifyService = new SpotifyService(_spotifyOptions);

            //Act
            var ex = Assert.Throws<SpotifyArgumentException>(_spotifyService.CheckSpotifyCredentials);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No Spotify ClientSecret configured"));
        }
    }
}
