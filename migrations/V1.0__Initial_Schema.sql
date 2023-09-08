CREATE TABLE spotify_playlist (
    id text PRIMARY KEY not null,
    name text not null,
    href text not null
);

CREATE TABLE spotify_artist (
    id text PRIMARY KEY not null,
    name text not null,
    href text not null
);

CREATE TABLE spotify_album (
    id text PRIMARY KEY not null,
    name text not null,
    href text not null
);

CREATE TABLE spotify_track (
    id text PRIMARY KEY not null,
    name text not null,
    length int not null,
    spotify_artist_id text not null,
    spotify_album_id text not null,

    CONSTRAINT fk__spotify_artist_id FOREIGN KEY (spotify_artist_id)
        REFERENCES spotify_artist(id),

    CONSTRAINT fk__spotify_album_id FOREIGN KEY (spotify_album_id)
        REFERENCES spotify_album(id)
);

CREATE TABLE skipped_track (
    id SERIAL PRIMARY KEY not null,
    skipped_date bigint not null,
    spotify_playlist_id text not null,
    spotify_track_id text not null,

    CONSTRAINT fk__spotify_playlist_id FOREIGN KEY (spotify_playlist_id)
        REFERENCES spotify_playlist(id),

    CONSTRAINT fk__track_id FOREIGN KEY (spotify_track_id)
        REFERENCES spotify_track(id)
);