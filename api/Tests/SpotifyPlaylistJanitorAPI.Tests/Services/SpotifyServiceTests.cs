using AutoFixture;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Exceptions;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Models.Spotify;
using SpotifyPlaylistJanitorAPI.Services;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class SpotifyServiceTests : TestBase
    {
        private SpotifyService _spotifyService;

        [SetUp]
        public void Setup()
        {
            _spotifyService = new SpotifyService(MockSpotifyClientService.Object, MockUserService.Object);
            MockSpotifyClientService
                .Setup(mock => mock.CreateClient(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(MockSpotifyClient.Object);
            MockSpotifyClientService
                .Setup(mock => mock.CreateClient(It.IsAny<AuthorizationCodeTokenResponse>(), It.IsAny<string>()))
                .ReturnsAsync(MockSpotifyClient.Object);
            _spotifyService.CreateClient("", "").Wait();
        }

        [Test]
        public void SpotifyService_Constructor_Checks_For_User_Spotify_Token_No_Token_SpotifyClients_Is_Empty()
        {
            //Act
            _spotifyService = new SpotifyService(MockSpotifyClientService.Object, MockUserService.Object);

            // Assert
            _spotifyService.SpotifyClients.Should().BeEmpty();
        }

        [Test]
        public void SpotifyService_Constructor_Checks_For_User_Spotify_Token_Has_Token_Spotify_Client_Service_Returns_Null()
        {
            //Arrange
            var users = Fixture
                .Build<UserDataModel>()
                .With(x => x.Username, USERNAME)
                .With(x => x.SpotifyUsername, SPOTIFY_USERNAME)
                .CreateMany(1);

            MockUserService
                .Setup(mock => mock.GetUsers())
                .ReturnsAsync(users);

            var tokenResponse = Fixture
                .Build<AuthorizationCodeTokenResponse>()
                .Create();
            var userStoredToken = Fixture
                .Build<UserSpotifyTokenModel>()
                .With (x => x.Username, SPOTIFY_USERNAME)
                .With (x => x.SpotifyToken, JsonConvert.SerializeObject(tokenResponse))
                .Create();

            MockUserService
                .Setup(mock => mock.GetUserSpotifyToken(It.IsAny<string>()))
                .ReturnsAsync(userStoredToken);

            SpotifyClient? client = null;
            MockSpotifyClientService
                .Setup(mock => mock.CreateClient(It.IsAny<AuthorizationCodeTokenResponse>(), It.IsAny<string>()))
                .ReturnsAsync(client);

            //Act
            _spotifyService = new SpotifyService(MockSpotifyClientService.Object, MockUserService.Object);

            // Assert
            _spotifyService.SpotifyClients.Should().BeEmpty();
        }

        [Test]
        public void SpotifyService_Constructor_Checks_For_User_Spotify_Token_Has_Token_Spotify_Client_Service_Returns_Client()
        {
            //Arrange
            var username = "username";
            var users = Fixture
                .Build<UserDataModel>()
                .With(x => x.Username, username)
                .CreateMany(1);

            MockUserService
                .Setup(mock => mock.GetUsers())
                .ReturnsAsync(users);

            var tokenResponse = Fixture
                .Build<AuthorizationCodeTokenResponse>()
                .Create();
            var userStoredToken = Fixture
                .Build<UserSpotifyTokenModel>()
                .With(x => x.Username, username)
                .With(x => x.SpotifyToken, JsonConvert.SerializeObject(tokenResponse))
                .Create();

            MockUserService
                .Setup(mock => mock.GetUserSpotifyToken(It.IsAny<string>()))
                .ReturnsAsync(userStoredToken);

            //Act
            _spotifyService = new SpotifyService(MockSpotifyClientService.Object, MockUserService.Object);

            // Assert
            _spotifyService.SpotifyClients.Should().NotBeEmpty();
        }

        [Test]
        public void SpotifyService_SpotifyClients_Defaults_to_Empty()
        {
            //Arrange
            SpotifyClient? client = null;
            MockSpotifyClientService
                .Setup(mock => mock.CreateClient(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(client);

            //Act
            _spotifyService = new SpotifyService(MockSpotifyClientService.Object, MockUserService.Object);

            // Assert
            _spotifyService.SpotifyClients.Should().BeEmpty();
        }

        [Test]
        public void SpotifyService_CreateClient_Sets_SpotifyClients_Not_Empty()
        {
            // Assert
            _spotifyService.SpotifyClients.Should().NotBeEmpty();
        }

        [Test]
        public async Task SpotifyService_GetUserDetails_Returns_Data()
        {
            //Act
            var result = await _spotifyService.GetUserDetails(SPOTIFY_USERNAME);

            // Assert
            result.Should().BeOfType<SpotifyUserModel>();
            result.Id.Should().Be(SPOTIFY_USERNAME);
            result.DisplayName.Should().Be(SPOTIFY_USERNAME);
            result.Email.Should().Be(SPOTIFY_USERNAME);
            result.Href.Should().Be(SPOTIFY_USERNAME);
        }

        [Test]
        public void SpotifyService_GetUserDetails_Throws_Exception_If_No_Spotify_Client_Configured()
        {
            //Arrange
            SpotifyClient? client = null;
            MockSpotifyClientService
                .Setup(mock => mock.CreateClient(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(client);

            //Act
            _spotifyService = new SpotifyService(MockSpotifyClientService.Object, MockUserService.Object);
            
            var ex = Assert.ThrowsAsync<SpotifyArgumentException>(async () => await _spotifyService.GetUserDetails(SPOTIFY_USERNAME));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No Spotify Client configured"));
        }

        [Test]
        public async Task SpotifyService_GetUserPlaylists_Returns_Data()
        {
            //Arrange
            var spotifyPlaylists = Fixture.Build<SimplePlaylist>()
                .With(x => x.ExternalUrls, new Dictionary<string, string>() { { "spotify", "spotify_url" } })
                .Without(x => x.Tracks)
                .CreateMany()
                .ToList();

            var mockPlaylists = new Mock<IPlaylistsClient>();
            mockPlaylists
                .Setup(mock => mock.CurrentUsers(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Paging<SimplePlaylist>());

            MockSpotifyClient
                .Setup(mock => mock.Playlists)
                .Returns(mockPlaylists.Object);

            MockSpotifyClient
                .Setup(mock => mock.PaginateAll(It.IsAny<IPaginatable<SimplePlaylist>>(), It.IsAny<IPaginator>()))
                .ReturnsAsync(spotifyPlaylists);

            var expectedPlaylists = spotifyPlaylists
                .Select(playlist => new SpotifyPlaylistModel
                {
                    Id = playlist.Id,
                    Name = playlist.Name,
                    Href = playlist.ExternalUrls["spotify"],
                    Images = playlist.Images.Select(image => new SpotifyImageModel
                    {
                        Height = image.Height,
                        Width = image.Width,
                        Url = image.Url,
                    })
                });

            //Act
            var result = await _spotifyService.GetUserPlaylists(SPOTIFY_USERNAME);

            // Assert
            result.Should().BeEquivalentTo(expectedPlaylists);
        }

        [Test]
        public void SpotifyService_GetUserPlaylists_Throws_Exception_If_No_Spotify_Client_Configured()
        {
            //Arrange
            SpotifyClient? client = null;
            MockSpotifyClientService
                .Setup(mock => mock.CreateClient(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(client);

            //Act
            _spotifyService = new SpotifyService(MockSpotifyClientService.Object, MockUserService.Object);

            var ex = Assert.ThrowsAsync<SpotifyArgumentException>(async () => await _spotifyService.GetUserDetails(SPOTIFY_USERNAME));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No Spotify Client configured"));
        }

        [Test]
        public async Task SpotifyService_GetUserPlaylist_Returns_Data()
        {
            //Arrange
            var spotifyPlaylist = Fixture.Build<FullPlaylist>()
                .With(x => x.ExternalUrls, new Dictionary<string, string>() { { "spotify", "spotify_url" } })
                .Without(x => x.Tracks)
                .Create();

            var mockPlaylists = new Mock<IPlaylistsClient>();
            mockPlaylists
                .Setup(mock => mock.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(spotifyPlaylist);

            MockSpotifyClient
                .Setup(mock => mock.Playlists)
                .Returns(mockPlaylists.Object);

            var expectedPlaylist = new SpotifyPlaylistModel
            {
                Id = spotifyPlaylist.Id,
                Name = spotifyPlaylist.Name,
                Href = spotifyPlaylist.ExternalUrls["spotify"],
                Images = spotifyPlaylist.Images.Select(image => new SpotifyImageModel
                {
                    Height = image.Height,
                    Width = image.Width,
                    Url = image.Url,
                })
            };

            //Act
            var result = await _spotifyService.GetUserPlaylist(SPOTIFY_USERNAME, "id");

            // Assert
            result.Should().BeEquivalentTo(expectedPlaylist);
        }

        [Test]
        public async Task SpotifyService_GetUserPlaylist_Returns_Null()
        {
            //Arrange
            var mockPlaylists = new Mock<IPlaylistsClient>();
            mockPlaylists
                .Setup(mock => mock.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new APIException());

            MockSpotifyClient
                .Setup(mock => mock.Playlists)
                .Returns(mockPlaylists.Object);

            //Act
            var result = await _spotifyService.GetUserPlaylist(SPOTIFY_USERNAME, "id");

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void SpotifyService_GetUserPlaylist_Throws_Exception_If_No_Spotify_Client_Configured()
        {
            //Arrange
            SpotifyClient? client = null;
            MockSpotifyClientService
                .Setup(mock => mock.CreateClient(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(client);

            //Act
            _spotifyService = new SpotifyService(MockSpotifyClientService.Object, MockUserService.Object);

            var ex = Assert.ThrowsAsync<SpotifyArgumentException>(async () => await _spotifyService.GetUserPlaylist(SPOTIFY_USERNAME, "id"));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No Spotify Client configured"));
        }

        [Test]
        public async Task SpotifyService_DeletePlaylistTracks_Returns()
        {
            //Arrange
            var snapshotResponse = new SnapshotResponse
            {
                SnapshotId = "snaphot_id"
            };
            var mockPlaylists = new Mock<IPlaylistsClient>();
            mockPlaylists
                .Setup(mock => mock.RemoveItems(It.IsAny<string>(), It.IsAny<PlaylistRemoveItemsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(snapshotResponse);

            MockSpotifyClient
                .Setup(mock => mock.Playlists)
                .Returns(mockPlaylists.Object);

            var trackIds = Fixture.Build<string>().CreateMany(50).ToList();

            //Act
            var result = await _spotifyService.DeletePlaylistTracks(SPOTIFY_USERNAME, "id", trackIds);

            // Assert
            MockSpotifyClient.Verify(mock => mock.Playlists.RemoveItems(
                It.IsAny<string>(),
                It.Is<PlaylistRemoveItemsRequest>(request => request.Tracks.Count == trackIds.Count),
                It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(snapshotResponse);
        }

        [Test]
        public async Task SpotifyService_DeletePlaylistTracks_Only_Deletes_100_Tracks_Returns()
        {
            //Arrange
            var snapshotResponse = new SnapshotResponse
            {
                SnapshotId = "snaphot_id"
            };
            var mockPlaylists = new Mock<IPlaylistsClient>();
            mockPlaylists
                .Setup(mock => mock.RemoveItems(It.IsAny<string>(), It.IsAny<PlaylistRemoveItemsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(snapshotResponse);

            MockSpotifyClient
                .Setup(mock => mock.Playlists)
                .Returns(mockPlaylists.Object);

            var trackIds = Fixture.Build<string>().CreateMany(150).ToList();

            //Act
            var result = await _spotifyService.DeletePlaylistTracks(SPOTIFY_USERNAME, "id", trackIds);

            // Assert
            MockSpotifyClient.Verify(mock => mock.Playlists.RemoveItems(
                It.IsAny<string>(),
                It.Is<PlaylistRemoveItemsRequest>(request => request.Tracks.Count == 100),
                It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(snapshotResponse);
        }

        [Test]
        public void SpotifyService_DeletePlaylistTracks_Throws_Exception_If_No_Spotify_Client_Configured()
        {
            //Arrange
            SpotifyClient? client = null;
            MockSpotifyClientService
                .Setup(mock => mock.CreateClient(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(client);

            //Act
            _spotifyService = new SpotifyService(MockSpotifyClientService.Object, MockUserService.Object);

            var ex = Assert.ThrowsAsync<SpotifyArgumentException>(async () => await _spotifyService.DeletePlaylistTracks(SPOTIFY_USERNAME, "id", new List<string>()));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No Spotify Client configured"));
        }

        [Test]
        public async Task SpotifyService_GetUserPlaylistTracks_Returns_Data()
        {
            //Arrange
            var mockTracks = Fixture.Build<PlaylistTrack<IPlayableItem>>()
                .With(x => x.Track, Fixture.Build<FullTrack>()
                    .With(x => x.ExternalUrls, new Dictionary<string, string>() { { "spotify", "spotify_url" } })
                    .Create())
                .CreateMany()
                .ToList();

            var mockPlaylists = new Mock<IPlaylistsClient>();
            mockPlaylists
                .Setup(mock => mock.GetItems(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Paging<PlaylistTrack<IPlayableItem>>());

            MockSpotifyClient
                .Setup(mock => mock.Playlists)
                .Returns(mockPlaylists.Object);

            MockSpotifyClient
                .Setup(mock => mock.PaginateAll(It.IsAny<IPaginatable<PlaylistTrack<IPlayableItem>>>(), It.IsAny<IPaginator>()))
                .ReturnsAsync(mockTracks);

            var expectedTracks = mockTracks
                .Select(track =>
                {
                    var item = (FullTrack)track.Track;
                    item.Album.ExternalUrls.TryGetValue("spotify", out string? albumHref);
                    return new SpotifyTrackModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Artists = item.Artists.Select(artist =>
                        {
                            artist.ExternalUrls.TryGetValue("spotify", out string? artistHref);
                            return new SpotifyArtistModel
                            {
                                Name = artist.Name,
                                Id = artist.Id,
                                Href = artistHref,
                            };
                        }),
                        Album = new SpotifyAlbumModel
                        {
                            Id = item.Album.Id,
                            Name = item.Album.Name,
                            Href = albumHref,
                            Images = item.Album.Images.Select(image => new SpotifyImageModel
                            {
                                Height = image.Height,
                                Width = image.Width,
                                Url = image.Url,
                            })
                        },
                        Duration = item.DurationMs,
                        IsLocal = item.IsLocal,
                    };
                });

            //Act
            var result = await _spotifyService.GetUserPlaylistTracks(SPOTIFY_USERNAME, "id");

            // Assert
            result.Should().BeEquivalentTo(expectedTracks);
        }

        [Test]
        public void SpotifyService_GetUserPlaylistTracks_Throws_Exception_If_No_Spotify_Client_Configured()
        {
            //Arrange
            SpotifyClient? client = null;
            MockSpotifyClientService
                .Setup(mock => mock.CreateClient(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(client);

            //Act
            _spotifyService = new SpotifyService(MockSpotifyClientService.Object, MockUserService.Object);

            var ex = Assert.ThrowsAsync<SpotifyArgumentException>(async () => await _spotifyService.GetUserPlaylistTracks(SPOTIFY_USERNAME, "id"));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No Spotify Client configured"));
        }

        [Test]
        public async Task SpotifyService_GetCurrentPlayback_Returns_Data_For_Current_Context_Playlist()
        {
            //Arrange
            var albumHref = "albumHref";
            var artistHref = "artistHref";
            var context = Fixture.Build<Context>()
                .With(x => x.Type, "playlist")
                .Create();

            var album = Fixture.Build<SimpleAlbum>()
                .With(x => x.ExternalUrls, new Dictionary<string, string>() { { "spotify", albumHref } })
                .Create();

            var artists = Fixture.Build<SimpleArtist>()
                .With(x => x.ExternalUrls, new Dictionary<string, string>() { { "spotify", artistHref } })
                .CreateMany()
                .ToList();

            var playingItem = Fixture.Build<FullTrack>()
                .With(x => x.Album, album)
                .With(x => x.Artists, artists)
                .Create();

            var spotifyCurrentPlayback = Fixture.Build<CurrentlyPlayingContext>()
                .With(x => x.IsPlaying, true)
                .With(x => x.CurrentlyPlayingType, "track")
                .With(x => x.Context, context)
                .With(x => x.Item, playingItem)
                .Create();

            var mockPlayer = new Mock<IPlayerClient>();
            mockPlayer
                .Setup(mock => mock.GetCurrentPlayback(It.IsAny<CancellationToken>()))
                .ReturnsAsync(spotifyCurrentPlayback);

            MockSpotifyClient
                .Setup(mock => mock.Player)
                .Returns(mockPlayer.Object);

            var expectedPlayback = new SpotifyPlayingState
            {
                IsPlaying = true,
                Track = new SpotifyCurrentlyPlayingTrackModel
                {
                    Id = playingItem.Id,
                    PlaylistId = spotifyCurrentPlayback.Context.Uri.Split(":").LastOrDefault(),
                    ListeningOn = spotifyCurrentPlayback.Device.Name,
                    Name = playingItem.Name,
                    Artists = playingItem.Artists.Select(artist => new SpotifyArtistModel
                    {
                        Name = artist.Name,
                        Id = artist.Id,
                        Href = artistHref,
                    }),
                    Album = new SpotifyAlbumModel
                    {
                        Id = playingItem.Album.Id,
                        Name = playingItem.Album.Name,
                        Href = albumHref,
                        Images = playingItem.Album.Images.Select(image => new SpotifyImageModel
                        {
                            Height = image.Height,
                            Width = image.Width,
                            Url = image.Url,
                        })
                    },
                    Duration = playingItem.DurationMs,
                    Progress = spotifyCurrentPlayback.ProgressMs,
                    IsLocal = playingItem.IsLocal,
                }
            };

            //Act
            var result = await _spotifyService.GetCurrentPlayback(SPOTIFY_USERNAME);

            // Assert
            result.Should().BeEquivalentTo(expectedPlayback);
        }

        [Test]
        public async Task SpotifyService_GetCurrentPlayback_Returns_Data_For_Current_Context_Album()
        {
            //Arrange
            var context = Fixture.Build<Context>()
                .With(x => x.Type, "album")
                .Create();

            var playingItem = Fixture.Build<FullTrack>()
                .Create();

            var spotifyCurrentPlayback = Fixture.Build<CurrentlyPlayingContext>()
                .With(x => x.IsPlaying, true)
                .With(x => x.CurrentlyPlayingType, "track")
                .With(x => x.Context, context)
                .With(x => x.Item, playingItem)
                .Create();

            var mockPlayer = new Mock<IPlayerClient>();
            mockPlayer
                .Setup(mock => mock.GetCurrentPlayback(It.IsAny<CancellationToken>()))
                .ReturnsAsync(spotifyCurrentPlayback);

            MockSpotifyClient
                .Setup(mock => mock.Player)
                .Returns(mockPlayer.Object);

            var expectedPlayback = new SpotifyPlayingState
            {
                IsPlaying = false,
                Track = null
            };

            //Act
            var result = await _spotifyService.GetCurrentPlayback(SPOTIFY_USERNAME);

            // Assert
            result.Should().BeEquivalentTo(expectedPlayback);
        }

        [Test]
        public async Task SpotifyService_GetCurrentPlayback_Returns_Data_For_Currently_Playing_Type_Podcast()
        {
            //Arrange
            var context = Fixture.Build<Context>()
                .With(x => x.Type, "playlist")
                .Create();

            var playingItem = Fixture.Build<FullTrack>()
                .Create();

            var spotifyCurrentPlayback = Fixture.Build<CurrentlyPlayingContext>()
                .With(x => x.IsPlaying, true)
                .With(x => x.CurrentlyPlayingType, "podcast")
                .With(x => x.Context, context)
                .With(x => x.Item, playingItem)
                .Create();

            var mockPlayer = new Mock<IPlayerClient>();
            mockPlayer
                .Setup(mock => mock.GetCurrentPlayback(It.IsAny<CancellationToken>()))
                .ReturnsAsync(spotifyCurrentPlayback);

            MockSpotifyClient
                .Setup(mock => mock.Player)
                .Returns(mockPlayer.Object);

            var expectedPlayback = new SpotifyPlayingState
            {
                IsPlaying = false,
                Track = null
            };

            //Act
            var result = await _spotifyService.GetCurrentPlayback(SPOTIFY_USERNAME);

            // Assert
            result.Should().BeEquivalentTo(expectedPlayback);
        }

        [Test]
        public void SpotifyService_GetCurrentPlayback_Throws_Exception_If_No_Spotify_Client_Configured()
        {
            //Arrange
            SpotifyClient? client = null;
            MockSpotifyClientService
                .Setup(mock => mock.CreateClient(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(client);

            //Act
            _spotifyService = new SpotifyService(MockSpotifyClientService.Object, MockUserService.Object);

            var ex = Assert.ThrowsAsync<SpotifyArgumentException>(async () => await _spotifyService.GetCurrentPlayback(SPOTIFY_USERNAME));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No Spotify Client configured"));
        }

        //[Test]
        //public void SpotifyService_CheckSpotifyCredentials_Doesnt_Throw_Exception()
        //{
        //    //Assert
        //    Assert.DoesNotThrow(_spotifyService.CheckSpotifyCredentials);
        //}

        //[Test]
        //public void SpotifyService_CheckSpotifyCredentials_Throws_Exception_If_No_Spotify_Client_Id_And_Secret_Configured()
        //{
        //    //Arrange
        //    SpotifyOptions = Options.Create(new SpotifyOption
        //    {
        //        ClientId = "",
        //        ClientSecret = "",
        //    });

        //    _spotifyService = new SpotifyService(SpotifyOptions, MockUserService.Object, _loggerMock.Object);

        //    //Act
        //    var ex = Assert.Throws<SpotifyArgumentException>(_spotifyService.CheckSpotifyCredentials);

        //    // Assert
        //    Assert.That(ex.Message, Is.EqualTo("No Spotify ClientId or ClientSecret configured"));
        //}

        //[Test]
        //public void SpotifyService_CheckSpotifyCredentials_Throws_Exception_If_No_Spotify_Client_Id_Configured()
        //{
        //    //Arrange
        //    SpotifyOptions = Options.Create(new SpotifyOption
        //    {
        //        ClientId = "",
        //        ClientSecret = "mockClientSecret",
        //    });

        //    _spotifyService = new SpotifyService(SpotifyOptions, MockUserService.Object, _loggerMock.Object);

        //    //Act
        //    var ex = Assert.Throws<SpotifyArgumentException>(_spotifyService.CheckSpotifyCredentials);

        //    // Assert
        //    Assert.That(ex.Message, Is.EqualTo("No Spotify ClientId configured"));
        //}

        //[Test]
        //public void SpotifyService_CheckSpotifyCredentials_Throws_Exception_If_No_Spotify_Client_Secret_Configured()
        //{
        //    //Arrange
        //    SpotifyOptions = Options.Create(new SpotifyOption
        //    {
        //        ClientId = "mockClientId",
        //        ClientSecret = "",
        //    });

        //    _spotifyService = new SpotifyService(SpotifyOptions, MockUserService.Object, _loggerMock.Object);

        //    //Act
        //    var ex = Assert.Throws<SpotifyArgumentException>(_spotifyService.CheckSpotifyCredentials);

        //    // Assert
        //    Assert.That(ex.Message, Is.EqualTo("No Spotify ClientSecret configured"));
        //}
    }
}
