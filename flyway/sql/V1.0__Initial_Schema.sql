CREATE TABLE playlist (
    id text PRIMARY KEY not null,
    name text not null,
    href text not null
);

CREATE TABLE artist (
    id text PRIMARY KEY not null,
    name text not null,
    href text not null
);

CREATE TABLE album (
    id text PRIMARY KEY not null,
    name text not null,
    href text not null
);

CREATE TABLE track (
    id text PRIMARY KEY not null,
    name text not null,
    length int not null,
    href text not null,
    artist_id text not null,
    album_id text not null,

    CONSTRAINT artist_id FOREIGN KEY (id)
        REFERENCES artist(id),

    CONSTRAINT album_id FOREIGN KEY (id)
        REFERENCES album(id)
);

CREATE TABLE skipped_track (
    id text PRIMARY KEY not null,
    playlist_id text not null,
    track_id text not null,

    CONSTRAINT playlist_id FOREIGN KEY (id)
        REFERENCES playlist(id),

    CONSTRAINT track_id FOREIGN KEY (id)
        REFERENCES track(id)
);