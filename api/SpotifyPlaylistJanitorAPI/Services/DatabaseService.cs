using Microsoft.EntityFrameworkCore;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.DataAccess.Context;
using SpotifyPlaylistJanitorAPI.DataAccess.Models;
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
            var playlists = await _context.SpotifyPlaylists
                .Select(x => new DatabasePlaylistModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Href = x.Href,
                })
                .OrderBy(playlist => playlist.Name)
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
            var playlistDto = await _context.SpotifyPlaylists
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(x => x.Id == id);

            var playlistModel = playlistDto is null ? null : new DatabasePlaylistModel
            {
                Id = playlistDto.Id,
                Name = playlistDto.Name,
                Href = playlistDto.Href,
            };

            return playlistModel;
        }

        /// <summary>
        /// Add playlist to database.
        /// </summary>
        /// <returns>Returns a <see cref = "DatabasePlaylistModel" />.</returns>
        public async Task<DatabasePlaylistModel> AddPlaylist(DatabasePlaylistRequest playlistRequest)
        {
            var playlistDto = new SpotifyPlaylist
            {
                Id = playlistRequest.Id,
                Name = playlistRequest.Name,
                Href = playlistRequest.Href,
            };

            if (!_context.SpotifyPlaylists.Any(track => track.Id.Equals(playlistRequest.Id)))
            {
                await _context.AddAsync(playlistDto);
                await _context.SaveChangesAsync();
            }

            var playlistModel = new DatabasePlaylistModel
            {
                Id = playlistDto.Id,
                Name = playlistDto.Name,
                Href = playlistDto.Href,
            };

            return playlistModel;
        }

        /// <summary>
        /// Deletes playlist from database.
        /// </summary>
        public async Task DeletePlaylist(string id)
        {
            _context.SkippedTracks.RemoveRange(_context.SkippedTracks.Where(track => track.SpotifyPlaylistId == id));
            _context.SpotifyPlaylists.RemoveRange(_context.SpotifyPlaylists.Where(playlist => playlist.Id == id));
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Add artist to database.
        /// </summary>
        ///<returns>Returns a <see cref = "DatabaseArtistModel" />.</returns>
        public async Task<DatabaseArtistModel> AddArtist(DatabaseArtistModel artistRequest)
        {
            var artistDto = new SpotifyArtist
            {
                Id = artistRequest.Id,
                Name = artistRequest.Name,
                Href = artistRequest.Href,
            };

            if (!_context.SpotifyArtists.Any(artist => artist.Id.Equals(artistRequest.Id)))
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
            var albumDto = new SpotifyAlbum
            {
                Id = albumRequest.Id,
                Name = albumRequest.Name,
                Href = albumRequest.Href,
            };

            if (!_context.SpotifyAlbums.Any(album => album.Id.Equals(albumDto.Id)))
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
        /// Add track to database.
        /// </summary>
        ///<returns>Returns a <see cref = "DatabaseTrackModel" />.</returns>
        public async Task<DatabaseTrackModel> AddTrack(DatabaseTrackModel trackRequest)
        {
            var trackDto = new SpotifyTrack
            {
                Id = trackRequest.Id,
                Name = trackRequest.Name,
                Length = trackRequest.Length,
                SpotifyAlbumId = trackRequest.AlbumId,
                SpotifyArtistId = trackRequest.ArtistId,
            };

            if (!_context.SpotifyTracks.Any(track => track.Id.Equals(trackRequest.Id)))
            {
                await _context.AddAsync(trackDto);
                await _context.SaveChangesAsync();
            }

            var trackModel = new DatabaseTrackModel
            {
                Id = trackDto.Id,
                Name = trackDto.Name,
                Length = trackDto.Length,
                AlbumId = trackDto.SpotifyAlbumId,
                ArtistId = trackDto.SpotifyArtistId,
            };

            return trackModel;
        }

        /// <summary>
        /// Add skipped track to database.
        /// </summary>
        ///<returns>Returns a <see cref = "DatabaseSkippedTrackModel" />.</returns>
        public async Task<DatabaseSkippedTrackModel> AddSkippedTrack(DatabaseSkippedTrackModel skippedTrackRequest)
        {
            var playlistDto = new SkippedTrack
            {
                SpotifyPlaylistId = skippedTrackRequest.PlaylistId,
                SpotifyTrackId = skippedTrackRequest.TrackId,
                SkippedDate = skippedTrackRequest.SkippedDate.ToUnixTimeSeconds(),
            };

            await _context.AddAsync(playlistDto);
            await _context.SaveChangesAsync();

            var skippedTrackModel = new DatabaseSkippedTrackModel
            {
                PlaylistId = playlistDto.SpotifyPlaylistId,
                TrackId = playlistDto.SpotifyTrackId,
                SkippedDate = DateTimeOffset.FromUnixTimeSeconds(playlistDto.SkippedDate),
            };

            return skippedTrackModel;
        }
    }
}
