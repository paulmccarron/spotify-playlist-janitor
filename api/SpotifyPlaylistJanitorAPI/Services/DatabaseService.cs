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

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseService"/> class.
        /// </summary>
        /// <param name="context">Database context.</param>
        public DatabaseService(SpotifyPlaylistJanitorDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns playlists from database.
        /// </summary>
        /// <returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "DatabasePlaylistModel" />.</returns>
        public async Task<IEnumerable<DatabasePlaylistModel>> GetPlaylists()
        {
            var playlists = await _context.Playlists
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
            var playlistDto = await _context.Playlists
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
            var playlistDto = new Playlist
            {
                Id = playlistRequest.Id,
                SkipThreshold = playlistRequest.SkipThreshold,
                IgnoreInitialSkips = playlistRequest.IgnoreInitialSkips,
                AutoCleanupLimit = playlistRequest.AutoCleanupLimit,
            };

            var alreadyExists = await _context.Playlists
                .ToAsyncEnumerable()
                .AnyAsync(playlist => playlist.Id.Equals(playlistRequest.Id));

            if (!alreadyExists)
            {
                await _context.AddAsync(playlistDto);
                await _context.SaveChangesAsync();
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
            DatabasePlaylistModel? playlistModel = null;
            var playlistDto = await _context.Playlists
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (playlistDto is not null)
            {
                playlistDto.SkipThreshold = playlistUpdateRequest.SkipThreshold;
                playlistDto.IgnoreInitialSkips = playlistUpdateRequest.IgnoreInitialSkips;
                playlistDto.AutoCleanupLimit = playlistUpdateRequest.AutoCleanupLimit;

                await _context.SaveChangesAsync();

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
            _context.SkippedTracks.RemoveRange(_context.SkippedTracks.Where(track => track.PlaylistId == id));
            _context.Playlists.RemoveRange(_context.Playlists.Where(playlist => playlist.Id == id));
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Add artist to database.
        /// </summary>
        public async Task AddArtist(DatabaseArtistModel artistRequest)
        {
            var artistDto = new Artist
            {
                Id = artistRequest.Id,
                Name = artistRequest.Name,
                Href = artistRequest.Href,
            };

            var alreadyExists = await _context.Artists
                .ToAsyncEnumerable()
                .AnyAsync(artist => artist.Id.Equals(artistRequest.Id));

            if (!alreadyExists)
            {
                await _context.AddAsync(artistDto);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add album to database.
        /// </summary>
        public async Task AddAlbum(DatabaseAlbumRequest albumRequest)
        {
            var albumDto = new Album
            {
                Id = albumRequest.Id,
                Name = albumRequest.Name,
                Href = albumRequest.Href,
            };

            var alreadyExists = await _context.Albums
                .ToAsyncEnumerable()
                .AnyAsync(album => album.Id.Equals(albumDto.Id));

            if (!alreadyExists)
            {
                await _context.AddAsync(albumDto);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add image to database.
        /// </summary>
        public async Task<DatabaseImageModel> AddImage(int height, int width, string url)
        {
            var imageRequest = new Image
            {
                Height = height,
                Width = width,
                Url = url
            };

            var imageDto = await _context.Images
                .ToAsyncEnumerable()
                .FirstOrDefaultAsync(image => image.Height == height && image.Width == width && image.Url == url);

            if (imageDto is null)
            {
                await _context.AddAsync(imageRequest);
                await _context.SaveChangesAsync();

                imageDto = await _context.Images
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
            var trackDto = new Track
            {
                Id = trackRequest.Id,
                Name = trackRequest.Name,
                Length = trackRequest.Length,
                AlbumId = trackRequest.AlbumId,
            };

            var alreadyExists = await _context.Tracks
                .ToAsyncEnumerable()
                .AnyAsync(track => track.Id.Equals(trackRequest.Id));

            if (!alreadyExists)
            {
                await _context.AddAsync(trackDto);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add artist to album relationship to database.
        /// </summary>
        public async Task AddArtistToTrack(string artistId, string trackId)
        {
            var trackDto = await _context.Tracks
                .Include(track => track.Artists)
                .ToAsyncEnumerable()
                .FirstAsync(track => track.Id.Equals(trackId));

            var trackHasArtist = trackDto.Artists.Any(artist => artist.Id == artistId);

            if (!trackHasArtist)
            {
                var artistDto = await _context.Artists
                .ToAsyncEnumerable()
                .FirstAsync(artist => artist.Id.Equals(artistId));

                trackDto.Artists.Add(artistDto);

                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add artist to album relationship to database.
        /// </summary>
        public async Task AddArtistToAlbum(string artistId, string albumId)
        {
            var albumDto = await _context.Albums
                .Include(album => album.Artists)
                .ToAsyncEnumerable()
                .FirstAsync(album => album.Id.Equals(albumId));

            var albumHasArtist = albumDto.Artists.Any(artist => artist.Id == artistId);

            if (!albumHasArtist)
            {
                var artistDto = await _context.Artists
                .ToAsyncEnumerable()
                .FirstAsync(artist => artist.Id.Equals(artistId));

                albumDto.Artists.Add(artistDto);

                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add image to album relationship to database.
        /// </summary>
        public async Task AddImageToAlbum(int imageId, string albumId)
        {
            var albumDto = await _context.Albums
                .Include(album => album.Images)
                .ToAsyncEnumerable()
                .FirstAsync(album => album.Id.Equals(albumId));

            var albumHasArtist = albumDto.Images.Any(image => image.Id == imageId);

            if (!albumHasArtist)
            {
                var imageDto = await _context.Images
                .ToAsyncEnumerable()
                .FirstAsync(image => image.Id.Equals(imageId));

                albumDto.Images.Add(imageDto);

                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add skipped playlist to database.
        /// </summary>
        public async Task AddSkippedTrack(DatabaseSkippedTrackRequest skippedTrackRequest)
        {
            var skippedTrackDto = new SkippedTrack
            {
                PlaylistId = skippedTrackRequest.PlaylistId,
                TrackId = skippedTrackRequest.TrackId,
                SkippedDate = skippedTrackRequest.SkippedDate,
            };

            await _context.AddAsync(skippedTrackDto);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove skipped tracks from database.
        /// </summary>
        public async Task DeleteSkippedTracks(string playlistId, IEnumerable<string> trackIds)
        {
            _context.SkippedTracks.RemoveRange(
                _context.SkippedTracks
                    .Where(track => track.PlaylistId == playlistId && trackIds.Contains(track.TrackId))
            );
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get skipped tracks for monitored plauylist from database.
        /// </summary>
        ///<returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "DatabaseSkippedTrackResponse" />.</returns>
        public async Task<IEnumerable<DatabaseSkippedTrackResponse>> GetPlaylistSkippedTracks(string playlistId)
        {
            var skippedTracks = await _context.SkippedTracks
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
        /// Returns user from database.
        /// </summary>
        ///<returns>Returns a <see cref = "UserDataModel" />.</returns>
        public async Task<UserDataModel?> GetUser(string username)
        {
            var userDto = await _context.Users
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
            var userDtos = await _context.Users
                .ToAsyncEnumerable()
                .ToListAsync();

            var userDto = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                IsAdmin = userDtos.Count() == 0,
            };

            await _context.AddAsync(userDto);
            await _context.SaveChangesAsync();
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
            var userDto = await _context.Users
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(x => x.Username.ToLowerInvariant() == username.ToLowerInvariant());

            if(userDto is not null)
            {
                userDto.RefreshToken = refreshToken;
                userDto.RefreshTokenExpiry = refreshTokenExpiry;

                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Store user spotify client token in database.
        /// </summary>
        public async Task AddUserSpotifyToken(string username, string spotifyToken)
        {
            var userSpotifyTokenDto = await _context.UsersSpotifyTokens
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(x => x.Username.ToLowerInvariant() == username.ToLowerInvariant());

            if (userSpotifyTokenDto is not null)
            {
                userSpotifyTokenDto.SpotifyToken = spotifyToken;
            }
            else
            {
                userSpotifyTokenDto = new UsersSpotifyToken
                {
                    Username = username,
                    SpotifyToken = spotifyToken,
                };

                await _context.AddAsync(userSpotifyTokenDto);
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieve user spotify client token from database.
        /// </summary>
        public async Task<UserSpotifyTokenModel?> GetUserSpotifyToken(string username)
        {
            var userSpotifyTokenDto = await _context.UsersSpotifyTokens
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(x => x.Username.ToLowerInvariant() == username.ToLowerInvariant());

            var userSpotifyTokenModel = userSpotifyTokenDto is null ? null : new UserSpotifyTokenModel
            {
                Username = userSpotifyTokenDto.Username,
                SpotifyToken = userSpotifyTokenDto.SpotifyToken,
            };

            return userSpotifyTokenModel;
        }
    }
}
