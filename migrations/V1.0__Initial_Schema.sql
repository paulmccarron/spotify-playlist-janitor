CREATE TABLE playlists (
    id text PRIMARY KEY not null
);

CREATE TABLE artists (
    id text PRIMARY KEY not null,
    name text not null,
    href text not null
);

CREATE TABLE albums (
    id text PRIMARY KEY not null,
    name text not null,
    href text not null
);

CREATE TABLE tracks (
    id text PRIMARY KEY not null,
    name text not null,
    length int not null,
    album_id text not null,
    FOREIGN KEY (album_id) REFERENCES albums(id)
);

CREATE TABLE images (
    id SERIAL PRIMARY KEY not null,
    url text not null,
    height int not null,
    width int not null
);

CREATE TABLE artists_tracks (
  artist_id text NOT NULL,
  track_id text NOT NULL,
  
  PRIMARY KEY (artist_id, track_id),
  FOREIGN KEY (artist_id) REFERENCES artists(id),
  FOREIGN KEY (track_id) REFERENCES tracks(id)
);

CREATE TABLE artists_albums (
  artist_id text NOT NULL,
  album_id text NOT NULL,
  
  PRIMARY KEY (artist_id, album_id),
  FOREIGN KEY (artist_id) REFERENCES artists(id),
  FOREIGN KEY (album_id) REFERENCES albums(id)
);

CREATE TABLE albums_images (
  album_id text NOT NULL,
  image_id int NOT NULL,
  
  PRIMARY KEY (album_id, image_id),
  FOREIGN KEY (album_id) REFERENCES albums(id),
  FOREIGN KEY (image_id) REFERENCES images(id)
);


CREATE TABLE skipped_tracks (
    id SERIAL PRIMARY KEY not null,
    skipped_date timestamp DEFAULT CURRENT_TIMESTAMP,
    playlist_id text not null,
    track_id text not null,

    FOREIGN KEY (playlist_id) REFERENCES playlists(id),
    FOREIGN KEY (track_id) REFERENCES tracks(id)
);