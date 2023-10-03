using Microsoft.EntityFrameworkCore;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.DataAccess.Entities;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Models.Database;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;

namespace SpotifyPlaylistJanitorAPI.Services
{
    /// <summary>
    /// Service to interact with application database.
    /// </summary>
    public class DatabaseService : IDatabaseService
    {
        private readonly SpotifyPlaylistJanitorDatabaseContext _context;
        private readonly IServiceScopeFactory _scopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseService"/> class.
        /// </summary>
        /// <param name="context">Database context.</param>
        public DatabaseService(IServiceScopeFactory scopeFactory)
        {
            //_context = context;
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Returns playlists from database.
        /// </summary>
        /// <returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "DatabasePlaylistModel" />.</returns>
        public async Task<IEnumerable<DatabasePlaylistModel>> GetPlaylists()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();
            var playlists = await dbContext.Playlists
                .Select(playlistDto => new DatabasePlaylistModel
                {
                    Id = playlistDto.Id,
                    SkipThreshold = playlistDto.SkipThreshold,
                    IgnoreInitialSkips = playlistDto.IgnoreInitialSkips,
                    AutoCleanupLimit = playlistDto.AutoCleanupLimit,
                })
                .ToAsyncEnumerable()
                .ToListAsync();

            return playlists;
        }

        /// <summary>
        /// Returns playlist from database by id.
        /// </summary>
        /// <returns>Returns a <see cref = "DatabasePlaylistModel" />.</returns>
        public async Task<DatabasePlaylistModel?> GetPlaylist(string id)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();
            var playlistDto = await dbContext.Playlists
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(x => x.Id == id);

            var playlistModel = playlistDto is null ? null : new DatabasePlaylistModel
            {
                Id = playlistDto.Id,
                SkipThreshold = playlistDto.SkipThreshold,
                IgnoreInitialSkips = playlistDto.IgnoreInitialSkips,
                AutoCleanupLimit = playlistDto.AutoCleanupLimit,
            };

            return playlistModel;
        }

        /// <summary>
        /// Add playlist to database.
        /// </summary>
        /// <returns>Returns a <see cref = "DatabasePlaylistModel" />.</returns>
        public async Task<DatabasePlaylistModel> AddPlaylist(DatabasePlaylistRequest playlistRequest)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var playlistDto = new Playlist
            {
                Id = playlistRequest.Id,
                SkipThreshold = playlistRequest.SkipThreshold,
                IgnoreInitialSkips = playlistRequest.IgnoreInitialSkips,
                AutoCleanupLimit = playlistRequest.AutoCleanupLimit,
            };

            var alreadyExists = await dbContext.Playlists
                .ToAsyncEnumerable()
                .AnyAsync(playlist => playlist.Id.Equals(playlistRequest.Id));

            if (!alreadyExists)
            {
                await dbContext.AddAsync(playlistDto);
                await dbContext.SaveChangesAsync();
            }

            var playlistModel = new DatabasePlaylistModel
            {
                Id = playlistDto.Id,
                SkipThreshold = playlistDto.SkipThreshold,
                IgnoreInitialSkips = playlistDto.IgnoreInitialSkips,
                AutoCleanupLimit = playlistDto.AutoCleanupLimit,
            };

            return playlistModel;
        }

        /// <summary>
        /// Add playlist to database.
        /// </summary>
        /// <returns>Returns a <see cref = "DatabasePlaylistModel" />.</returns>
        public async Task<DatabasePlaylistModel?> UpdatePlaylist(string id, DatabasePlaylistUpdateRequest playlistUpdateRequest)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            DatabasePlaylistModel? playlistModel = null;
            var playlistDto = await dbContext.Playlists
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (playlistDto is not null)
            {
                playlistDto.SkipThreshold = playlistUpdateRequest.SkipThreshold;
                playlistDto.IgnoreInitialSkips = playlistUpdateRequest.IgnoreInitialSkips;
                playlistDto.AutoCleanupLimit = playlistUpdateRequest.AutoCleanupLimit;

                await dbContext.SaveChangesAsync();

                playlistModel = new DatabasePlaylistModel
                {
                    Id = id,
                    SkipThreshold = playlistUpdateRequest.SkipThreshold,
                    IgnoreInitialSkips = playlistUpdateRequest.IgnoreInitialSkips,
                    AutoCleanupLimit = playlistUpdateRequest.AutoCleanupLimit,
                };
            }

            return playlistModel;
        }

        /// <summary>
        /// Deletes playlist from database.
        /// </summary>
        public async Task DeletePlaylist(string id)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            dbContext.SkippedTracks.RemoveRange(dbContext.SkippedTracks.Where(track => track.PlaylistId == id));
            dbContext.Playlists.RemoveRange(dbContext.Playlists.Where(playlist => playlist.Id == id));
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Add artist to database.
        /// </summary>
        public async Task AddArtist(DatabaseArtistModel artistRequest)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var artistDto = new Artist
            {
                Id = artistRequest.Id,
                Name = artistRequest.Name,
                Href = artistRequest.Href,
            };

            var alreadyExists = await dbContext.Artists
                .ToAsyncEnumerable()
                .AnyAsync(artist => artist.Id.Equals(artistRequest.Id));

            if (!alreadyExists)
            {
                await dbContext.AddAsync(artistDto);
                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add album to database.
        /// </summary>
        public async Task AddAlbum(DatabaseAlbumRequest albumRequest)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var albumDto = new Album
            {
                Id = albumRequest.Id,
                Name = albumRequest.Name,
                Href = albumRequest.Href,
            };

            var alreadyExists = await dbContext.Albums
                .ToAsyncEnumerable()
                .AnyAsync(album => album.Id.Equals(albumDto.Id));

            if (!alreadyExists)
            {
                await dbContext.AddAsync(albumDto);
                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add image to database.
        /// </summary>
        public async Task<DatabaseImageModel> AddImage(int height, int width, string url)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var imageRequest = new Image
            {
                Height = height,
                Width = width,
                Url = url
            };

            var imageDto = await dbContext.Images
                .ToAsyncEnumerable()
                .FirstOrDefaultAsync(image => image.Height == height && image.Width == width && image.Url == url);

            if (imageDto is null)
            {
                await dbContext.AddAsync(imageRequest);
                await dbContext.SaveChangesAsync();

                imageDto = await dbContext.Images
                .ToAsyncEnumerable()
                .FirstOrDefaultAsync(image => image.Height == height && image.Width == width && image.Url == url);
            }

            return new DatabaseImageModel
            {
                Id = imageDto?.Id ?? 0,
                Height = imageDto?.Height ?? 0,
                Width = imageDto?.Width ?? 0,
                Url = imageDto?.Url
            };
        }

        /// <summary>
        /// Add album to database.
        /// </summary>
        public async Task AddTrack(DatabaseTrackModel trackRequest)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var trackDto = new Track
            {
                Id = trackRequest.Id,
                Name = trackRequest.Name,
                Length = trackRequest.Length,
                AlbumId = trackRequest.AlbumId,
            };

            var alreadyExists = await dbContext.Tracks
                .ToAsyncEnumerable()
                .AnyAsync(track => track.Id.Equals(trackRequest.Id));

            if (!alreadyExists)
            {
                await dbContext.AddAsync(trackDto);
                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add artist to album relationship to database.
        /// </summary>
        public async Task AddArtistToTrack(string artistId, string trackId)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var trackDto = await dbContext.Tracks
                .Include(track => track.Artists)
                .ToAsyncEnumerable()
                .FirstAsync(track => track.Id.Equals(trackId));

            var trackHasArtist = trackDto.Artists.Any(artist => artist.Id == artistId);

            if (!trackHasArtist)
            {
                var artistDto = await dbContext.Artists
                .ToAsyncEnumerable()
                .FirstAsync(artist => artist.Id.Equals(artistId));

                trackDto.Artists.Add(artistDto);

                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add artist to album relationship to database.
        /// </summary>
        public async Task AddArtistToAlbum(string artistId, string albumId)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var albumDto = await dbContext.Albums
                .Include(album => album.Artists)
                .ToAsyncEnumerable()
                .FirstAsync(album => album.Id.Equals(albumId));

            var albumHasArtist = albumDto.Artists.Any(artist => artist.Id == artistId);

            if (!albumHasArtist)
            {
                var artistDto = await dbContext.Artists
                .ToAsyncEnumerable()
                .FirstAsync(artist => artist.Id.Equals(artistId));

                albumDto.Artists.Add(artistDto);

                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add image to album relationship to database.
        /// </summary>
        public async Task AddImageToAlbum(int imageId, string albumId)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var albumDto = await dbContext.Albums
                .Include(album => album.Images)
                .ToAsyncEnumerable()
                .FirstAsync(album => album.Id.Equals(albumId));

            var albumHasArtist = albumDto.Images.Any(image => image.Id == imageId);

            if (!albumHasArtist)
            {
                var imageDto = await dbContext.Images
                .ToAsyncEnumerable()
                .FirstAsync(image => image.Id.Equals(imageId));

                albumDto.Images.Add(imageDto);

                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add skipped playlist to database.
        /// </summary>
        public async Task AddSkippedTrack(DatabaseSkippedTrackRequest skippedTrackRequest)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var skippedTrackDto = new SkippedTrack
            {
                PlaylistId = skippedTrackRequest.PlaylistId,
                TrackId = skippedTrackRequest.TrackId,
                SkippedDate = skippedTrackRequest.SkippedDate,
            };

            await dbContext.AddAsync(skippedTrackDto);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Remove skipped tracks from database.
        /// </summary>
        public async Task DeleteSkippedTracks(string playlistId, IEnumerable<string> trackIds)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            dbContext.SkippedTracks.RemoveRange(
                dbContext.SkippedTracks
                    .Where(track => track.PlaylistId == playlistId && trackIds.Contains(track.TrackId))
            );
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get skipped tracks for monitored plauylist from database.
        /// </summary>
        ///<returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "DatabaseSkippedTrackResponse" />.</returns>
        public async Task<IEnumerable<DatabaseSkippedTrackResponse>> GetPlaylistSkippedTracks(string playlistId)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var skippedTracks = await dbContext.SkippedTracks
                .Where(skipped => skipped.PlaylistId == playlistId)
                .Include(skipped => skipped.Track)
                .ThenInclude(track => track.Artists)
                .ThenInclude(track => track.Albums)
                .ThenInclude(album => album.Images)
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
                })
                .ToAsyncEnumerable()
                .ToListAsync();

            return skippedTracks;
        }

        /// <summary>
        /// Returns users from database.
        /// </summary>
        ///<returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "UserDataModel" />.</returns>
        public async Task<IEnumerable<UserDataModel>> GetUsers()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var userModels = await dbContext.Users
                .Select(userDto => new UserDataModel
                {
                    Id = userDto.Id,
                    Username = userDto.Username,
                    PasswordHash = userDto.PasswordHash,
                    IsAdmin = userDto.IsAdmin,
                    RefreshToken = userDto.RefreshToken,
                    RefreshTokenExpiry = userDto.RefreshTokenExpiry,
                })
                .ToAsyncEnumerable()
                .ToListAsync();

            return userModels;
        }

        /// <summary>
        /// Returns user from database.
        /// </summary>
        ///<returns>Returns a <see cref = "UserDataModel" />.</returns>
        public async Task<UserDataModel?> GetUser(string username)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var userDto = await dbContext.Users
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(x => x.Username.ToLowerInvariant() == username.ToLowerInvariant());

            var userModel = userDto is null ? null : new UserDataModel
            {
                Id = userDto.Id,
                Username = userDto.Username,
                PasswordHash = userDto.PasswordHash,
                IsAdmin = userDto.IsAdmin,
                RefreshToken = userDto.RefreshToken,
                RefreshTokenExpiry = userDto.RefreshTokenExpiry,
            };

            return userModel;
        }

        /// <summary>
        /// Adds user to database.
        /// </summary>
        public async Task AddUser(string username, string passwordHash)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var userDtos = await dbContext.Users
                .ToAsyncEnumerable()
                .ToListAsync();

            var userDto = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                IsAdmin = userDtos.Count() == 0,
            };

            await dbContext.AddAsync(userDto);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates user refresh token in database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="refreshToken"></param>
        /// <param name="refreshTokenExpiry"></param>
        /// <returns></returns>
        public async Task UpdateUserRefreshToken(string username, string? refreshToken, DateTime? refreshTokenExpiry)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var userDto = await dbContext.Users
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(x => x.Username.ToLowerInvariant() == username.ToLowerInvariant());

            if(userDto is not null)
            {
                userDto.RefreshToken = refreshToken;
                userDto.RefreshTokenExpiry = refreshTokenExpiry;

                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Store user spotify client token in database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="encodedSpotifyToken"></param>
        public async Task AddUserEncodedSpotifyToken(string username, string encodedSpotifyToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var userSpotifyTokenDto = await dbContext.UsersSpotifyTokens
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(x => x.Username.ToLowerInvariant() == username.ToLowerInvariant());

            if (userSpotifyTokenDto is not null)
            {
                userSpotifyTokenDto.EncodedSpotifyToken = encodedSpotifyToken;
            }
            else
            {
                userSpotifyTokenDto = new UsersSpotifyToken
                {
                    Username = username,
                    EncodedSpotifyToken = encodedSpotifyToken,
                };

                await dbContext.AddAsync(userSpotifyTokenDto);
            }

            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieve user spotify client token from database.
        /// </summary>
        /// <param name="username"></param>
        public async Task<UserEncodedSpotifyTokenModel?> GetUserEncodedSpotifyToken(string username)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SpotifyPlaylistJanitorDatabaseContext>();

            var userSpotifyTokenDto = await dbContext.UsersSpotifyTokens
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(x => x.Username.ToLowerInvariant() == username.ToLowerInvariant());

            var userSpotifyTokenModel = userSpotifyTokenDto is null ? null : new UserEncodedSpotifyTokenModel
            {
                Username = userSpotifyTokenDto.Username,
                EncodedSpotifyToken = userSpotifyTokenDto.EncodedSpotifyToken,
            };

            return userSpotifyTokenModel;
        }
    }
}
