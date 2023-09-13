using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.DataAccess.Entities;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Services;
using System;

namespace SpotifyPlaylistJanitorAPI.Tests.Services
{
    [TestFixture]
    public class DatabaseServiceTests : TestBase
    {
        private DatabaseService _databaseService;
        private Mock<SpotifyPlaylistJanitorDatabaseContext> _dbContextMock;
        private Mock<DbSet<Playlist>> _dbSetPlaylistMock;
        private Mock<DbSet<Artist>> _dbSetArtistMock;
        private Mock<DbSet<Album>> _dbSetAlbumMock;
        private Mock<DbSet<Track>> _dbSetTrackMock;
        private Mock<DbSet<Image>> _dbSetImageMock;
        private Mock<DbSet<SkippedTrack>> _dbSetSkippedMock;

        [SetUp]
        public void Init()
        {
            _dbSetPlaylistMock = new Mock<DbSet<Playlist>>();
            _dbSetArtistMock = new Mock<DbSet<Artist>>();
            _dbSetAlbumMock = new Mock<DbSet<Album>>();
            _dbSetTrackMock = new Mock<DbSet<Track>>();
            _dbSetImageMock = new Mock<DbSet<Image>>();
            _dbSetSkippedMock = new Mock<DbSet<SkippedTrack>>();
            _dbContextMock = new Mock<SpotifyPlaylistJanitorDatabaseContext>();

            _dbSetPlaylistMock.AddIQueryables(new List<Playlist>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.Playlists)
                .Returns(_dbSetPlaylistMock.Object);

            _dbSetArtistMock.AddIQueryables(new List<Artist>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.Artists)
                .Returns(_dbSetArtistMock.Object);

            _dbSetAlbumMock.AddIQueryables(new List<Album>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.Albums)
                .Returns(_dbSetAlbumMock.Object);

            _dbSetTrackMock.AddIQueryables(new List<Track>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.Tracks)
                .Returns(_dbSetTrackMock.Object);

            _dbSetImageMock.AddIQueryables(new List<Image>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.Images)
                .Returns(_dbSetImageMock.Object);

            _dbSetSkippedMock.AddIQueryables(new List<SkippedTrack>().AsQueryable());
            _dbContextMock
                .Setup(mock => mock.SkippedTracks)
                .Returns(_dbSetSkippedMock.Object);

            _databaseService = new DatabaseService(_dbContextMock.Object);
        }

        [Test]
        public async Task DatabaseService_GetPlaylists_Returns_Data()
        {
            //Arrange
            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .AsQueryable();

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            _dbContextMock
                .Setup(mock => mock.Playlists)
                .Returns(_dbSetPlaylistMock.Object);

            var expectedResults = dbPlaylists
                .Select(x => new DatabasePlaylistModel
                {
                    Id = x.Id,
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

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            var expectedResult = dbPlaylists
                .Select(x => new DatabasePlaylistModel
                {
                    Id = x.Id,
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

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            _dbContextMock
                .Setup(mock => mock.Playlists)
                .Returns(_dbSetPlaylistMock.Object);

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
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Playlist>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            //Act
            var result = await _databaseService.AddPlaylist(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Playlist>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeEquivalentTo(databaseRequest);
        }

        [Test]
        public async Task DatabaseService_DeletePlaylist_Removes_Data()
        {
            //Arrange
            var id = "id";

            var dbSkippedTracks = Fixture.Build<SkippedTrack>()
                .CreateMany()
                .AsQueryable();

            _dbSetSkippedMock.AddIQueryables(dbSkippedTracks);

            var dbPlaylists = Fixture.Build<Playlist>()
                .CreateMany()
                .AsQueryable();

            _dbSetPlaylistMock.AddIQueryables(dbPlaylists);

            //Act
            await _databaseService.DeletePlaylist(id);

            // Assert
            _dbContextMock.Verify(context => context.SkippedTracks.RemoveRange(It.IsAny<IEnumerable<SkippedTrack>>()), Times.Once);
            _dbContextMock.Verify(context => context.Playlists.RemoveRange(It.IsAny<IEnumerable<Playlist>>()), Times.Once);
            _dbContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddArtist_Adds_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseArtistModel>().Create();

            //Act
            await _databaseService.AddArtist(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Artist>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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

            _dbSetArtistMock.AddIQueryables(dbArtists);

            //Act
            await _databaseService.AddArtist(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Artist>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DatabaseService_AddAlbum_Adds_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseAlbumRequest>().Create();

            //Act
            await _databaseService.AddAlbum(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Album>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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

            _dbSetAlbumMock.AddIQueryables(dbAlbums);

            //Act
            await _databaseService.AddAlbum(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Album>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DatabaseService_AddTrack_Adds_And_Returns_Data()
        {
            //Arrange
            var databaseRequest = Fixture.Build<DatabaseTrackModel>().Create();

            //Act
            await _databaseService.AddTrack(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Track>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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

            _dbSetTrackMock.AddIQueryables(dbTracks);

            //Act
            await _databaseService.AddTrack(databaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Track>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
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
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Image>(), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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

            _dbSetImageMock.AddIQueryables(dbImages);

            //Act
            await _databaseService.AddImage(height, width, url);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(It.IsAny<Image>(), It.IsAny<CancellationToken>()), Times.Never);
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DatabaseService_AddArtistToTrack_Adds_Relationship()
        {
            //Arrange
            var dbArtists = Fixture.Build<Artist>()
                .CreateMany()
                .AsQueryable();

            _dbSetArtistMock.AddIQueryables(dbArtists);

            var dbTracks = Fixture.Build<Track>()
                .With(x => x.Artists, new List<Artist>())
                .CreateMany()
                .AsQueryable();

            _dbSetTrackMock.AddIQueryables(dbTracks);

            //Act
            await _databaseService.AddArtistToTrack(dbArtists.First().Id, dbTracks.First().Id);

            // Assert
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddArtistToTrack_Skips_Add_When_Relationship_Exists()
        {
            //Arrange
            var dbArtists = Fixture.Build<Artist>()
                .CreateMany()
                .AsQueryable();

            _dbSetArtistMock.AddIQueryables(dbArtists);

            var dbTracks = Fixture.Build<Track>()
                .With(x => x.Artists, new List<Artist>(dbArtists))
                .CreateMany()
                .AsQueryable();

            _dbSetTrackMock.AddIQueryables(dbTracks);

            //Act
            await _databaseService.AddArtistToTrack(dbArtists.First().Id, dbTracks.First().Id);

            // Assert
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DatabaseService_AddArtistToAlbum_Adds_Relationship()
        {
            //Arrange
            var dbArtists = Fixture.Build<Artist>()
                .CreateMany()
                .AsQueryable();

            _dbSetArtistMock.AddIQueryables(dbArtists);

            var dbAlbums = Fixture.Build<Album>()
                .With(x => x.Artists, new List<Artist>())
                .CreateMany()
                .AsQueryable();

            _dbSetAlbumMock.AddIQueryables(dbAlbums);

            //Act
            await _databaseService.AddArtistToAlbum(dbArtists.First().Id, dbAlbums.First().Id);

            // Assert
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddArtistToAlbum_Skips_Add_When_Relationship_Exists()
        {
            //Arrange
            var dbArtists = Fixture.Build<Artist>()
                .CreateMany()
                .AsQueryable();

            _dbSetArtistMock.AddIQueryables(dbArtists);

            var dbAlbums = Fixture.Build<Album>()
                .With(x => x.Artists, new List<Artist>(dbArtists))
                .CreateMany()
                .AsQueryable();

            _dbSetAlbumMock.AddIQueryables(dbAlbums);

            //Act
            await _databaseService.AddArtistToAlbum(dbArtists.First().Id, dbAlbums.First().Id);

            // Assert
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DatabaseService_AddImageToAlbum_Adds_Relationship()
        {
            //Arrange
            var dbImages = Fixture.Build<Image>()
                .CreateMany()
                .AsQueryable();

            _dbSetImageMock.AddIQueryables(dbImages);

            var dbAlbums = Fixture.Build<Album>()
                .With(x => x.Images, new List<Image>())
                .CreateMany()
                .AsQueryable();

            _dbSetAlbumMock.AddIQueryables(dbAlbums);

            //Act
            await _databaseService.AddImageToAlbum(dbImages.First().Id, dbAlbums.First().Id);

            // Assert
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DatabaseService_AddImageToAlbum_Skips_Add_When_Relationship_Exists()
        {
            //Arrange
            var dbImages = Fixture.Build<Image>()
                .CreateMany()
                .AsQueryable();

            _dbSetImageMock.AddIQueryables(dbImages);

            var dbAlbums = Fixture.Build<Album>()
                .With(x => x.Images, new List<Image>(dbImages))
                .CreateMany()
                .AsQueryable();

            _dbSetAlbumMock.AddIQueryables(dbAlbums);

            //Act
            await _databaseService.AddImageToAlbum(dbImages.First().Id, dbAlbums.First().Id);

            // Assert
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
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

            _dbContextMock
                .Setup(mock => mock.SkippedTracks)
                .Returns(_dbSetSkippedMock.Object);

            var expectedResult = new DatabaseSkippedTrackRequest
            {
                PlaylistId = playlistId,
                TrackId = trackId,
                SkippedDate = skippedTime,
            };

            //Act
            await _databaseService.AddSkippedTrack(datatbaseRequest);

            // Assert
            _dbContextMock.Verify(context => context.AddAsync(
                It.Is<SkippedTrack>(skipped => skipped.SkippedDate.Equals(skippedTime)),
                It.IsAny<CancellationToken>()),
            Times.Once());
            _dbContextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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

            _dbSetSkippedMock.AddIQueryables(dbSkippedTracks);

            var dbTracks = Fixture.Build<Track>()
                .With(x => x.Id, track_id)
                .CreateMany(1)
                .AsQueryable();

            _dbContextMock
                .Setup(mock => mock.Playlists)
                .Returns(_dbSetPlaylistMock.Object);

            var expectedResults = dbSkippedTracks
                .Select(skipped => new DatabaseSkippedTrackResponse
                {
                    PlaylistId = playlistId,
                    TrackId = skipped.TrackId,
                    Name = skipped.Track.Name,
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

            _dbSetSkippedMock.AddIQueryables(dbSkippedTracks);

            var trackIds = dbSkippedTracks.Select(track => track.TrackId);

            //Act
            await _databaseService.DeleteSkippedTracks(id, trackIds);

            // Assert
            _dbContextMock.Verify(context => context.SkippedTracks.RemoveRange(It.IsAny<IEnumerable<SkippedTrack>>()), Times.Once);
            _dbContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
