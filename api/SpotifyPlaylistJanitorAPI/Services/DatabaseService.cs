using Microsoft.EntityFrameworkCore;
using SpotifyPlaylistJanitorAPI.DataAccess;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
//using SpotifyPlaylistJanitorAPI.DataAccess.Context;
//using SpotifyPlaylistJanitorAPI.DataAccess.Models;
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
                .Select(x => new DatabasePlaylistModel
                {
                    Id = x.Id,
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
            };

            var alreadyExists = await _context.Playlists
                .ToAsyncEnumerable()
                .AnyAsync(playlist => playlist.Id.Equals(playlistRequest.Id));

            if(!alreadyExists)
            {
                await _context.AddAsync(playlistDto);
                await _context.SaveChangesAsync();
            }

            var playlistModel = new DatabasePlaylistModel
            {
                Id = playlistDto.Id,
            };

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
        ///<returns>Returns a <see cref = "DatabaseArtistModel" />.</returns>
        public async Task<DatabaseArtistModel> AddArtist(DatabaseArtistModel artistRequest)
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

            if(!alreadyExists)
            {
                await _context.AddAsync(artistDto);
                await _context.SaveChangesAsync();
            }

            var artistModel = new DatabaseArtistModel
            {
                Id = artistDto.Id,
                Name = artistDto.Name,
                Href = artistDto.Href,
            };

            return artistModel;
        }

        /// <summary>
        /// Add album to database.
        /// </summary>
        ///<returns>Returns a <see cref = "DatabaseAlbumModel" />.</returns>
        public async Task<DatabaseAlbumModel> AddAlbum(DatabaseAlbumModel albumRequest)
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

            if(!alreadyExists)
            {
                await _context.AddAsync(albumDto);
                await _context.SaveChangesAsync();
            }

            var albumModel = new DatabaseAlbumModel
            {
                Id = albumDto.Id,
                Name = albumDto.Name,
                Href = albumDto.Href,
            };

            return albumModel;
        }

        /// <summary>
        /// Add playlist to database.
        /// </summary>
        ///<returns>Returns a <see cref = "DatabaseTrackModel" />.</returns>
        public async Task<DatabaseTrackModel> AddTrack(DatabaseTrackModel trackRequest)
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

            if(!alreadyExists)
            {
                await _context.AddAsync(trackDto);
                await _context.SaveChangesAsync();
            }

            var trackModel = new DatabaseTrackModel
            {
                Id = trackDto.Id,
                Name = trackDto.Name,
                Length = trackDto.Length,
                AlbumId = trackDto.AlbumId,
            };

            return trackModel;
        }

        /// <summary>
        /// Add skipped playlist to database.
        /// </summary>
        ///<returns>Returns a <see cref = "DatabaseSkippedTrackModel" />.</returns>
        public async Task<DatabaseSkippedTrackModel> AddSkippedTrack(DatabaseSkippedTrackModel skippedTrackRequest)
        {
            var skippedTrackDto = new SkippedTrack
            {
                PlaylistId = skippedTrackRequest.PlaylistId,
                TrackId = skippedTrackRequest.TrackId,
                SkippedDate = skippedTrackRequest.SkippedDate,
            };

            await _context.AddAsync(skippedTrackDto);
            await _context.SaveChangesAsync();

            var skippedTrackModel = new DatabaseSkippedTrackModel
            {
                PlaylistId = skippedTrackDto.PlaylistId,
                TrackId = skippedTrackDto.TrackId,
                SkippedDate = skippedTrackRequest.SkippedDate,
            };

            return skippedTrackModel;
        }

        /// <summary>
        /// Get skipped tracks for monitored plauylist from database.
        /// </summary>
        ///<returns>Returns an<see cref="IEnumerable{T}" /> of type <see cref = "DatabaseSkippedTrackModel" />.</returns>
        public async Task<IEnumerable<DatabaseSkippedTrackModel>> GetPlaylistSkippedTracks(string playlistId){
            var skippedTracks = await _context.SkippedTracks
                .Where(track => track.PlaylistId == playlistId)
                .Select(track => new DatabaseSkippedTrackModel
                {
                    PlaylistId = playlistId,
                    TrackId = track.TrackId,
                    SkippedDate = track.SkippedDate,
                })
                .ToAsyncEnumerable()
                .ToListAsync();

            return skippedTracks;
        }
    }
}
