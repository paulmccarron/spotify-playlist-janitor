using Microsoft.EntityFrameworkCore;
using SpotifyPlaylistJanitorAPI.DataAccess.Models;
using System.Diagnostics.CodeAnalysis;

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

    public virtual DbSet<SkippedTrack> SkippedTracks { get; set; }

    public virtual DbSet<SpotifyAlbum> SpotifyAlbums { get; set; }

    public virtual DbSet<SpotifyArtist> SpotifyArtists { get; set; }

    public virtual DbSet<SpotifyPlaylist> SpotifyPlaylists { get; set; }

    public virtual DbSet<SpotifyTrack> SpotifyTracks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SkippedTrack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("skipped_track_pkey");

            entity.ToTable("skipped_track");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SkippedDate).HasColumnName("skipped_date");
            entity.Property(e => e.SpotifyPlaylistId).HasColumnName("spotify_playlist_id");
            entity.Property(e => e.SpotifyTrackId).HasColumnName("spotify_track_id");

            entity.HasOne(d => d.SpotifyPlaylist).WithMany(p => p.SkippedTracks)
                .HasForeignKey(d => d.SpotifyPlaylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__spotify_playlist_id");

            entity.HasOne(d => d.SpotifyTrack).WithMany(p => p.SkippedTracks)
                .HasForeignKey(d => d.SpotifyTrackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__track_id");
        });

        modelBuilder.Entity<SpotifyAlbum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spotify_album_pkey");

            entity.ToTable("spotify_album");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Href).HasColumnName("href");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<SpotifyArtist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spotify_artist_pkey");

            entity.ToTable("spotify_artist");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Href).HasColumnName("href");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<SpotifyPlaylist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spotify_playlist_pkey");

            entity.ToTable("spotify_playlist");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Href).HasColumnName("href");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<SpotifyTrack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("spotify_track_pkey");

            entity.ToTable("spotify_track");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Length).HasColumnName("length");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.SpotifyAlbumId).HasColumnName("spotify_album_id");
            entity.Property(e => e.SpotifyArtistId).HasColumnName("spotify_artist_id");

            entity.HasOne(d => d.SpotifyAlbum).WithMany(p => p.SpotifyTracks)
                .HasForeignKey(d => d.SpotifyAlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__spotify_album_id");

            entity.HasOne(d => d.SpotifyArtist).WithMany(p => p.SpotifyTracks)
                .HasForeignKey(d => d.SpotifyArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__spotify_artist_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
