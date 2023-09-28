using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SpotifyPlaylistJanitorAPI.DataAccess.Entities;

namespace SpotifyPlaylistJanitorAPI.DataAccess.Context;

#pragma warning disable 1591
[ExcludeFromCodeCoverage]

public partial class SpotifyPlaylistJanitorDatabaseContext : DbContext
{
    public SpotifyPlaylistJanitorDatabaseContext()
    {
    }

    public SpotifyPlaylistJanitorDatabaseContext(DbContextOptions<SpotifyPlaylistJanitorDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<SkippedTrack> SkippedTracks { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersSpotifyToken> UsersSpotifyTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("albums_pkey");

            entity.ToTable("albums");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Href).HasColumnName("href");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasMany(d => d.Images).WithMany(p => p.Albums)
                .UsingEntity<Dictionary<string, object>>(
                    "AlbumsImage",
                    r => r.HasOne<Image>().WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("albums_images_image_id_fkey"),
                    l => l.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("albums_images_album_id_fkey"),
                    j =>
                    {
                        j.HasKey("AlbumId", "ImageId").HasName("albums_images_pkey");
                        j.ToTable("albums_images");
                        j.IndexerProperty<string>("AlbumId").HasColumnName("album_id");
                        j.IndexerProperty<int>("ImageId").HasColumnName("image_id");
                    });
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("artists_pkey");

            entity.ToTable("artists");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Href).HasColumnName("href");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasMany(d => d.Albums).WithMany(p => p.Artists)
                .UsingEntity<Dictionary<string, object>>(
                    "ArtistsAlbum",
                    r => r.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("artists_albums_album_id_fkey"),
                    l => l.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("artists_albums_artist_id_fkey"),
                    j =>
                    {
                        j.HasKey("ArtistId", "AlbumId").HasName("artists_albums_pkey");
                        j.ToTable("artists_albums");
                        j.IndexerProperty<string>("ArtistId").HasColumnName("artist_id");
                        j.IndexerProperty<string>("AlbumId").HasColumnName("album_id");
                    });

            entity.HasMany(d => d.Tracks).WithMany(p => p.Artists)
                .UsingEntity<Dictionary<string, object>>(
                    "ArtistsTrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("artists_tracks_track_id_fkey"),
                    l => l.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("artists_tracks_artist_id_fkey"),
                    j =>
                    {
                        j.HasKey("ArtistId", "TrackId").HasName("artists_tracks_pkey");
                        j.ToTable("artists_tracks");
                        j.IndexerProperty<string>("ArtistId").HasColumnName("artist_id");
                        j.IndexerProperty<string>("TrackId").HasColumnName("track_id");
                    });
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("images_pkey");

            entity.ToTable("images");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.Url).HasColumnName("url");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("playlists_pkey");

            entity.ToTable("playlists");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AutoCleanupLimit).HasColumnName("auto_cleanup_limit");
            entity.Property(e => e.IgnoreInitialSkips).HasColumnName("ignore_initial_skips");
            entity.Property(e => e.SkipThreshold).HasColumnName("skip_threshold");
        });

        modelBuilder.Entity<SkippedTrack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("skipped_tracks_pkey");

            entity.ToTable("skipped_tracks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");
            entity.Property(e => e.SkippedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("skipped_date");
            entity.Property(e => e.TrackId).HasColumnName("track_id");

            entity.HasOne(d => d.Playlist).WithMany(p => p.SkippedTracks)
                .HasForeignKey(d => d.PlaylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("skipped_tracks_playlist_id_fkey");

            entity.HasOne(d => d.Track).WithMany(p => p.SkippedTracks)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("skipped_tracks_track_id_fkey");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tracks_pkey");

            entity.ToTable("tracks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AlbumId).HasColumnName("album_id");
            entity.Property(e => e.Length).HasColumnName("length");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.Album).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tracks_album_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsAdmin).HasColumnName("is_admin");
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasColumnName("password_hash");
            entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");
            entity.Property(e => e.RefreshTokenExpiry)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("refresh_token_expiry");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasColumnName("username");
        });

        modelBuilder.Entity<UsersSpotifyToken>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("users_spotify_token_pkey");

            entity.ToTable("users_spotify_token");

            entity.Property(e => e.Username).HasColumnName("username");
            entity.Property(e => e.SpotifyToken).HasColumnName("spotify_token");

            entity.HasOne(d => d.UsernameNavigation).WithOne(p => p.UsersSpotifyToken)
                .HasPrincipalKey<User>(p => p.Username)
                .HasForeignKey<UsersSpotifyToken>(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_spotify_token_username_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
