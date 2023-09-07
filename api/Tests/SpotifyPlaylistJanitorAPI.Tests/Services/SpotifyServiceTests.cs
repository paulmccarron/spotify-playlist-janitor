using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Exceptions;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models.Spotify;
using SpotifyPlaylistJanitorAPI.Services;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class SpotifyServiceTests : TestBase
    {
        private SpotifyService _spotifyService;
        private IOptions<SpotifyOption> _spotifyOptions;
        private Mock<ISpotifyClient> _spotifyClientMock;

        [SetUp]
        public void Setup()
        {
            _spotifyClientMock = new Mock<ISpotifyClient>();
            _spotifyOptions = Options.Create(new SpotifyOption
            {
                ClientId = "mockClientId",
                ClientSecret = "mockClientSecret",
            });

            _spotifyService = new SpotifyService(_spotifyOptions);
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
        public async Task SpotifyService_GetUserDetails_Returns_Data()
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
            var result = await _spotifyService.GetUserDetails();

            // Assert
            result.Should().BeOfType<SpotifyUserModel>();
            result.Id.Should().Be(userId);
            result.DisplayName.Should().Be(userDisplayName);
            result.Email.Should().Be(userEmail);
            result.Href.Should().Be(userHref);
        }

        [Test]
        public void SpotifyService_GetUserDetails_Throws_Exception_If_No_Spotify_Client_Configured()
        {
            //Arrange
            _spotifyService.SetClient(null);

            //Act
            var ex = Assert.ThrowsAsync<SpotifyArgumentException>(_spotifyService.GetUserDetails);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No Spotify Client configured"));
        }

        [Test]
        public async Task SpotifyService_GetUserPlaylists_Returns_Data()
        {
            //Arrange
            var spotifyPlaylists = Fixture.Build<SimplePlaylist>().Without(x => x.Tracks).CreateMany().ToList();

            var mockPlaylists = new Mock<IPlaylistsClient>();
            mockPlaylists
                .Setup(mock => mock.CurrentUsers(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Paging<SimplePlaylist>());

            _spotifyClientMock
                .Setup(mock => mock.Playlists)
                .Returns(mockPlaylists.Object);

            _spotifyClientMock
                .Setup(mock => mock.PaginateAll(It.IsAny<IPaginatable<SimplePlaylist>>(), It.IsAny<IPaginator>()))
                .ReturnsAsync(spotifyPlaylists);

            var expectedPlaylists = spotifyPlaylists
                .Select(playlist => new SpotifyPlaylistModel
                {
                    Id = playlist.Id,
                    Name = playlist.Name,
                    Href = playlist.Href,
                    Images = playlist.Images.Select(image => new SpotifyImageModel
                    {
                        Height = image.Height,
                        Width = image.Width,
                        Url = image.Url,
                    })
                });

            //Act
            var result = await _spotifyService.GetUserPlaylists();

            // Assert
            result.Should().BeEquivalentTo(expectedPlaylists);
        }

        [Test]
        public void SpotifyService_GetUserPlaylists_Throws_Exception_If_No_Spotify_Client_Configured()
        {
            //Arrange
            _spotifyService.SetClient(null);

            //Act
            var ex = Assert.ThrowsAsync<SpotifyArgumentException>(_spotifyService.GetUserPlaylists);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No Spotify Client configured"));
        }

        [Test]
        public void SpotifyService_CheckSpotifyCredentials_Doesnt_Throw_Exception()
        {
            //Arrange
            _spotifyOptions = Options.Create(new SpotifyOption
            {
                ClientId = "mockClientId",
                ClientSecret = "mockClientSecret",
            });

            _spotifyService = new SpotifyService(_spotifyOptions);

            //Act
            //Assert
            Assert.DoesNotThrow(_spotifyService.CheckSpotifyCredentials);
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
