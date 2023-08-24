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
    artist_id text not null,
    album_id text not null,

    CONSTRAINT fk__artist_id FOREIGN KEY (artist_id)
        REFERENCES artist(id),

    CONSTRAINT fk__album_id FOREIGN KEY (album_id)
        REFERENCES album(id)
);

CREATE TABLE skipped_track (
    id int PRIMARY KEY not null,
    playlist_id text not null,
    track_id text not null,

    CONSTRAINT fk__playlist_id FOREIGN KEY (playlist_id)
        REFERENCES playlist(id),

    CONSTRAINT fk__track_id FOREIGN KEY (track_id)
        REFERENCES track(id)
);