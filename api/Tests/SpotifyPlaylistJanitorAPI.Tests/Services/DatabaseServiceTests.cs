using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.DataAccess.Entities;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Services;
using SpotifyPlaylistJanitorAPI.System;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class DatabaseServiceTests : TestBase
    {
        private DatabaseService _databaseService;
        private Mock<IServiceScopeFactory> _scopeFactoryMock;

        [SetUp]
        public void Init()
        {
            _scopeFactoryMock = new Mock<IServiceScopeFactory>();

            Mock<IServiceProvider> mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(x => x.GetService(typeof(SpotifyPlaylistJanitorDatabaseContext)))
                .Returns(MockDbContext.Object);

            Mock<IServiceScope> mockServiceScope = new Mock<IServiceScope>();
            mockServiceScope
                .Setup(mock => mock.ServiceProvider)
                .Returns(mockServiceProvider.Object);

            _scopeFactoryMock
                .Setup(mock => mock.CreateScope())
                .Returns(mockServiceScope.Object);

            _databaseService = new DatabaseService(_scopeFactoryMock.Object);
        }

        [Test]
        public async Task DatabaseService_GetPlaylists_Returns_Data()
        {
            //Arrange
            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .AsQueryable();

            MockDbSetPlaylist.AddIQueryables(dbPlaylists);

            var expectedResults = dbPlaylists
                .Select(playlistDto => new DatabasePlaylistModel
                {
                    Id = playlistDto.Id,
                    SkipThreshold = playlistDto.SkipThreshold,
                    IgnoreInitialSkips = playlistDto.IgnoreInitialSkips,
                    AutoCleanupLimit = playlistDto.AutoCleanupLimit,
                });

            //Act
            var result = await _databaseService.GetPlaylists();

            // Assert
            result.Should().BeOfType<List<DatabasePlaylistModel>>();
            result.Should().BeEquivalentTo(expectedResults);
        }

        [Test]
        public async Task DatabaseService_GetPlaylist_Returns_Data()
        {
            //Arrange
            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .AsQueryable();

            var playlistId = dbPlaylists.First().Id;

            MockDbSetPlaylist.AddIQueryables(dbPlaylists);

            var expectedResult = dbPlaylists
                .Select(playlistDto => new DatabasePlaylistModel
                {
                    Id = playlistDto.Id,
                    SkipThreshold = playlistDto.SkipThreshold,
                    IgnoreInitialSkips = playlistDto.IgnoreInitialSkips,
                    AutoCleanupLimit = playlistDto.AutoCleanupLimit,
                })
                .First();

            //Act
            var result = await _databaseService.GetPlaylist(playlistId);

            // Assert
            result.Should().BeOfType<DatabasePlaylistModel>();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task DatabaseService_GetPlaylist_Returns_Null_For_Invalid_Id()
        {
            //Arrange
            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .AsQueryable();

            var playlistId = "RANDOM_ID";

            MockDbSetPlaylist.AddIQueryables(dbPlaylists);

            //Act
            var result = await _databaseService.GetPlaylist(playlistId);

            // Assert
            result.Should().BeNull();
        }


        [Test]
        public async Task DatabaseService_AddPlaylist_Adds_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabasePlaylistRequest>().Create();

            //Act
            var result = await _databaseService.AddPlaylist(databaseRequest);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(It.IsAny<Playlist>(), It.IsAny<CancellationToken>()), Times.Once);
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_AddPlaylist_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabasePlaylistRequest>().Create();

            var dbPlaylists = Fixture.Build<Playlist>()
                .With(playlist => playlist.Id, databaseRequest.Id)
                .CreateMany()
                .AsQueryable();

            MockDbSetPlaylist.AddIQueryables(dbPlaylists);

            //Act
            var result = await _databaseService.AddPlaylist(databaseRequest);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(It.IsAny<Playlist>(), It.IsAny<CancellationToken>()), Times.Never);
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_UpdatePlaylist_Returns_Data()
        {
            //Arrange
            var updateRequest = Fixture.Build<DatabasePlaylistUpdateRequest>()
                .Create();

            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .AsQueryable();

            var playlistId = dbPlaylists.First().Id;

            MockDbSetPlaylist.AddIQueryables(dbPlaylists);

            var expectedResult = new DatabasePlaylistModel
            {
                Id = playlistId,
                SkipThreshold = updateRequest.SkipThreshold,
                IgnoreInitialSkips = updateRequest.IgnoreInitialSkips,
                AutoCleanupLimit = updateRequest.AutoCleanupLimit,
            };

            //Act
            var result = await _databaseService.UpdatePlaylist(playlistId, updateRequest);

            // Assert
            MockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeOfType<DatabasePlaylistModel>();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task DatabaseService_UpdatePlaylist_Returns_Null_For_Invalid_Id()
        {
            //Arrange
            var updateRequest = Fixture.Build<DatabasePlaylistUpdateRequest>()
                .Create();

            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .AsQueryable();

            var playlistId = "RANDOM_ID";

            MockDbSetPlaylist.AddIQueryables(dbPlaylists);

            //Act
            var result = await _databaseService.UpdatePlaylist(playlistId, updateRequest);

            // Assert
            MockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeNull();
        }

        [Test]
        public async Task DatabaseService_DeletePlaylist_Removes_Data()
        {
            //Arrange
            var id = "id";

            var dbSkippedTracks = Fixture.Build<SkippedTrack>()
                .CreateMany()
                .AsQueryable();

            MockDbSetSkipped.AddIQueryables(dbSkippedTracks);

            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .AsQueryable();

            MockDbSetPlaylist.AddIQueryables(dbPlaylists);

            //Act
            await _databaseService.DeletePlaylist(id);

            // Assert
            MockDbContext.Verify(context => context.SkippedTracks.RemoveRange(It.IsAny<IEnumerable<SkippedTrack>>()), Times.Once);
            MockDbContext.Verify(context => context.Playlists.RemoveRange(It.IsAny<IEnumerable<Playlist>>()), Times.Once);
            MockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddArtist_Adds_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseArtistModel>().Create();

            //Act
            await _databaseService.AddArtist(databaseRequest);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(It.IsAny<Artist>(), It.IsAny<CancellationToken>()), Times.Once);
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddArtist_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseArtistModel>().Create();

            var dbArtists = Fixture.Build<Artist>()
                .With(artist => artist.Id, databaseRequest.Id)
                .CreateMany()
                .AsQueryable();

            MockDbSetArtist.AddIQueryables(dbArtists);

            //Act
            await _databaseService.AddArtist(databaseRequest);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(It.IsAny<Artist>(), It.IsAny<CancellationToken>()), Times.Never);
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DatabaseService_AddAlbum_Adds_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseAlbumRequest>().Create();

            //Act
            await _databaseService.AddAlbum(databaseRequest);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(It.IsAny<Album>(), It.IsAny<CancellationToken>()), Times.Once);
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddAlbum_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseAlbumRequest>().Create();

            var dbAlbums = Fixture.Build<Album>()
                .With(album => album.Id, databaseRequest.Id)
                .CreateMany()
                .AsQueryable();

            MockDbSetAlbum.AddIQueryables(dbAlbums);

            //Act
            await _databaseService.AddAlbum(databaseRequest);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(It.IsAny<Album>(), It.IsAny<CancellationToken>()), Times.Never);
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DatabaseService_AddTrack_Adds_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseTrackModel>().Create();

            //Act
            await _databaseService.AddTrack(databaseRequest);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(It.IsAny<Track>(), It.IsAny<CancellationToken>()), Times.Once);
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddTrack_Skips_When_Exists_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseTrackModel>().Create();
            
            var dbTracks = Fixture.Build<Track>()
                .With(album => album.Id, databaseRequest.Id)
                .CreateMany()
                .AsQueryable();

            MockDbSetTrack.AddIQueryables(dbTracks);

            //Act
            await _databaseService.AddTrack(databaseRequest);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(It.IsAny<Track>(), It.IsAny<CancellationToken>()), Times.Never);
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DatabaseService_AddImage_Adds_And_Returns_Data()
        {
            //Arrange
            var height = 150;
            var width = 150;
            var url = "http://test.com";

            //Act
            await _databaseService.AddImage(height, width, url);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(It.IsAny<Image>(), It.IsAny<CancellationToken>()), Times.Once);
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddImage_Skips_Add_When_Exists_And_Returns_Data()
        {
            //Arrange
            var height = 150;
            var width = 150;
            var url = "http://test.com";

            var dbImages = Fixture.Build<Image>()
                .With(image => image.Height, height)
                .With(image => image.Width, width)
                .With(image => image.Url, url)
                .CreateMany(1)
                .AsQueryable();

            MockDbSetImage.AddIQueryables(dbImages);

            //Act
            await _databaseService.AddImage(height, width, url);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(It.IsAny<Image>(), It.IsAny<CancellationToken>()), Times.Never);
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DatabaseService_AddArtistToTrack_Adds_Relationship()
        {
            //Arrange
            var dbArtists = Fixture.Build<Artist>()
                .CreateMany()
                .AsQueryable();

            MockDbSetArtist.AddIQueryables(dbArtists);

            var dbTracks = Fixture.Build<Track>()
                .With(x => x.Artists, new List<Artist>())
                .CreateMany()
                .AsQueryable();

            MockDbSetTrack.AddIQueryables(dbTracks);

            //Act
            await _databaseService.AddArtistToTrack(dbArtists.First().Id, dbTracks.First().Id);

            // Assert
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddArtistToTrack_Skips_Add_When_Relationship_Exists()
        {
            //Arrange
            var dbArtists = Fixture.Build<Artist>()
                .CreateMany()
                .AsQueryable();

            MockDbSetArtist.AddIQueryables(dbArtists);

            var dbTracks = Fixture.Build<Track>()
                .With(x => x.Artists, new List<Artist>(dbArtists))
                .CreateMany()
                .AsQueryable();

            MockDbSetTrack.AddIQueryables(dbTracks);

            //Act
            await _databaseService.AddArtistToTrack(dbArtists.First().Id, dbTracks.First().Id);

            // Assert
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DatabaseService_AddArtistToAlbum_Adds_Relationship()
        {
            //Arrange
            var dbArtists = Fixture.Build<Artist>()
                .CreateMany()
                .AsQueryable();

            MockDbSetArtist.AddIQueryables(dbArtists);

            var dbAlbums = Fixture.Build<Album>()
                .With(x => x.Artists, new List<Artist>())
                .CreateMany()
                .AsQueryable();

            MockDbSetAlbum.AddIQueryables(dbAlbums);

            //Act
            await _databaseService.AddArtistToAlbum(dbArtists.First().Id, dbAlbums.First().Id);

            // Assert
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddArtistToAlbum_Skips_Add_When_Relationship_Exists()
        {
            //Arrange
            var dbArtists = Fixture.Build<Artist>()
                .CreateMany()
                .AsQueryable();

            MockDbSetArtist.AddIQueryables(dbArtists);

            var dbAlbums = Fixture.Build<Album>()
                .With(x => x.Artists, new List<Artist>(dbArtists))
                .CreateMany()
                .AsQueryable();

            MockDbSetAlbum.AddIQueryables(dbAlbums);

            //Act
            await _databaseService.AddArtistToAlbum(dbArtists.First().Id, dbAlbums.First().Id);

            // Assert
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DatabaseService_AddImageToAlbum_Adds_Relationship()
        {
            //Arrange
            var dbImages = Fixture.Build<Image>()
                .CreateMany()
                .AsQueryable();

            MockDbSetImage.AddIQueryables(dbImages);

            var dbAlbums = Fixture.Build<Album>()
                .With(x => x.Images, new List<Image>())
                .CreateMany()
                .AsQueryable();

            MockDbSetAlbum.AddIQueryables(dbAlbums);

            //Act
            await _databaseService.AddImageToAlbum(dbImages.First().Id, dbAlbums.First().Id);

            // Assert
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddImageToAlbum_Skips_Add_When_Relationship_Exists()
        {
            //Arrange
            var dbImages = Fixture.Build<Image>()
                .CreateMany()
                .AsQueryable();

            MockDbSetImage.AddIQueryables(dbImages);

            var dbAlbums = Fixture.Build<Album>()
                .With(x => x.Images, new List<Image>(dbImages))
                .CreateMany()
                .AsQueryable();

            MockDbSetAlbum.AddIQueryables(dbAlbums);

            //Act
            await _databaseService.AddImageToAlbum(dbImages.First().Id, dbAlbums.First().Id);

            // Assert
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DatabaseService_AddSkippedTrack_Adds_Data()
        {
            //Arrange
            var playlistId = "playlistId";
            var trackId = "trackId";
            var skippedTime = new DateTime(2023, 9, 8, 14, 0, 0, 0);

            var datatbaseRequest = new DatabaseSkippedTrackRequest
            {
                PlaylistId = playlistId,
                TrackId = trackId,
                SkippedDate = skippedTime,
            };

            var expectedResult = new DatabaseSkippedTrackRequest
            {
                PlaylistId = playlistId,
                TrackId = trackId,
                SkippedDate = skippedTime,
            };

            //Act
            await _databaseService.AddSkippedTrack(datatbaseRequest);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(
                It.Is<SkippedTrack>(skipped => skipped.SkippedDate.Equals(skippedTime)),
                It.IsAny<CancellationToken>()),
            Times.Once());
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Test]
        public async Task DatabaseService_GetPlaylistSkippedTracks_Returns_Data()
        {
            //Arrange
            var playlistId = "playlist_id";
            var track_id = "track_id";
            var albumId = "album_id";
            var dateTime = DateTime.UtcNow;

            var dbArtists = Fixture.Build<Artist>()
                .CreateMany(2)
                .ToList();

            var dbImages = Fixture.Build<Image>()
                .CreateMany(2)
                .ToList();

            var dbAlbum = Fixture.Build<Album>()
                .With(x => x.Id, albumId)
                .With(x => x.Images, dbImages)
                .Create();

            var dbTrack = Fixture.Build<Track>()
                .With(x => x.Id, track_id)
                .With(x => x.Artists, dbArtists)
                .With(x => x.Album, dbAlbum)
                .Create();

            var dbSkippedTracks = Fixture.Build<SkippedTrack>()
                .With(x => x.SkippedDate, dateTime)
                .With(x => x.PlaylistId, playlistId)
                .With(x => x.TrackId, track_id)
                .With(x => x.Track, dbTrack)
                .CreateMany(1)
                .AsQueryable();

            MockDbSetSkipped.AddIQueryables(dbSkippedTracks);

            var expectedResults = dbSkippedTracks
                .Select(skipped => new DatabaseSkippedTrackResponse
                {
                    PlaylistId = playlistId,
                    TrackId = skipped.TrackId,
                    Name = skipped.Track.Name,
                    Duration = skipped.Track.Length,
                    SkippedDate = skipped.SkippedDate,
                    Artists = skipped.Track.Artists
                    .Select(artist => new DatabaseArtistModel
                    {
                        Id = artist.Id,
                        Name = artist.Name,
                        Href = artist.Href,
                    }),
                    Album = new DatabaseAlbumResponse
                    {
                        Id = skipped.Track.Album.Id,
                        Name = skipped.Track.Album.Name,
                        Href = skipped.Track.Album.Href,
                        Images = skipped.Track.Album.Images
                        .Select(image => new DatabaseImageModel
                        {
                            Id = image.Id,
                            Height = image.Height,
                            Width = image.Width,
                            Url = image.Url,
                        })
                    }
                });

            //Act
            var result = await _databaseService.GetPlaylistSkippedTracks(playlistId);

            // Assert
            result.Should().BeOfType<List<DatabaseSkippedTrackResponse>>();
            result.Should().BeEquivalentTo(expectedResults);
        }

        [Test]
        public async Task DatabaseService_DeleteSkippedTracks_Removes_Data()
        {
            //Arrange
            var id = "id";

            var dbSkippedTracks = Fixture.Build<SkippedTrack>()
                .CreateMany()
                .AsQueryable();

            MockDbSetSkipped.AddIQueryables(dbSkippedTracks);

            var trackIds = dbSkippedTracks.Select(track => track.TrackId);

            //Act
            await _databaseService.DeleteSkippedTracks(id, trackIds);

            // Assert
            MockDbContext.Verify(context => context.SkippedTracks.RemoveRange(It.IsAny<IEnumerable<SkippedTrack>>()), Times.Once);
            MockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_GetUsers_Returns_Data()
        {
            //Arrange
            var dbUsers = Fixture.Build<User>()
                .CreateMany()
                .AsQueryable();

            MockDbSetUser.AddIQueryables(dbUsers);

            var expectedResult = dbUsers
                .Select(x => new UserDataModel
                {
                    Id = x.Id,
                    Username = x.Username,
                    PasswordHash = x.PasswordHash,
                    IsAdmin = x.IsAdmin,
                    RefreshToken = x.RefreshToken,
                    RefreshTokenExpiry = x.RefreshTokenExpiry,
                });

            //Act
            var result = await _databaseService.GetUsers();

            // Assert
            result.Should().BeOfType<List<UserDataModel>>();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task DatabaseService_GetUser_Returns_Data()
        {
            //Arrange
            var dbUsers = Fixture.Build<User>()
                .CreateMany()
                .AsQueryable();

            var userName = dbUsers.First().Username;

            MockDbSetUser.AddIQueryables(dbUsers);

            var expectedResult = dbUsers
                .Select(x => new UserDataModel
                {
                    Id = x.Id,
                    Username = x.Username,
                    PasswordHash = x.PasswordHash,
                    IsAdmin = x.IsAdmin,
                    RefreshToken = x.RefreshToken,
                    RefreshTokenExpiry = x.RefreshTokenExpiry,
                })
                .First();

            //Act
            var result = await _databaseService.GetUser(userName);

            // Assert
            result.Should().BeOfType<UserDataModel>();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task DatabaseService_GetUser_Returns_Null_For_Invalid_Id()
        {
            //Arrange
            var dbUsers = Fixture.Build<User>()
                .CreateMany()
                .AsQueryable();

            var userName = "RANDOM_ID";

            MockDbSetUser.AddIQueryables(dbUsers);

            //Act
            var result = await _databaseService.GetUser(userName);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task DatabaseService_AddUser_Adds_And_Returns_Task()
        {
            //Arrange
            var username = "username";
            var passwordHash = "paswordHash";

            //Act
            await _databaseService.AddUser(username, passwordHash);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_UpdateUserRefreshToken_Saves_And_Returns_Task()
        {
            //Arrange
            var username = "username";
            var refreshToken = "refreshToken";
            var tokenExpiry = SystemTime.Now();

            var dbUsers = Fixture.Build<User>()
                .With(x => x.Username, username)
                .CreateMany(1)
                .AsQueryable();

            MockDbSetUser.AddIQueryables(dbUsers);

            //Act
            await _databaseService.UpdateUserRefreshToken(username, refreshToken, tokenExpiry);

            // Assert
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_UpdateUserRefreshToken_Skips_Save_And_Returns_Task()
        {
            //Arrange
            var username = "username";
            var refreshToken = "refreshToken";
            var tokenExpiry = SystemTime.Now();

            var dbUsers = Fixture.Build<User>()
                .With(x => x.Username, username)
                .CreateMany()
                .AsQueryable();

            MockDbSetUser.AddIQueryables(dbUsers);

            //Act
            await _databaseService.UpdateUserRefreshToken("RANDOM_USER", refreshToken, tokenExpiry);

            // Assert
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DatabaseService_AddUserEncodedSpotifyToken_Saves_Existing_User_Token_And_Returns_Task()
        {
            //Arrange
            var username = "username";
            var spotifyToken = "spotifyToken";
            var tokenExpiry = SystemTime.Now();

            var dbUsers = Fixture.Build<UsersSpotifyToken>()
                .With(x => x.Username, username)
                .CreateMany(1)
                .AsQueryable();

            MockDbSetUserSpotifyToken.AddIQueryables(dbUsers);

            //Act
            await _databaseService.AddUserEncodedSpotifyToken(username, spotifyToken);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(It.IsAny<UsersSpotifyToken>(), It.IsAny<CancellationToken>()), Times.Never);
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddUserEncodedSpotifyToken_Adds_New_User_Token_And_Returns_Task()
        {
            //Arrange
            var username = "username";
            var spotifyToken = "spotifyToken";

            //Act
            await _databaseService.AddUserEncodedSpotifyToken(username, spotifyToken);

            // Assert
            MockDbContext.Verify(context => context.AddAsync(It.IsAny<UsersSpotifyToken>(), It.IsAny<CancellationToken>()), Times.Once);
            MockDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
