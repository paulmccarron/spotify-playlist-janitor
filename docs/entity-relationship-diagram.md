# Entity-Relationship Diagram

Current interation of the Entity-Relationship Diagram for the Spotify Playlist Janitor project, check `git` history for previous versions.

``` mermaid
%%{init: {'theme':'neutral'}}%%
erDiagram
    SPOTIFY_PLAYLIST ||--|| SKIPPED_TRACK : ""
    SKIPPED_TRACK }o--|| SPOTIFY_TRACK : ""
    SPOTIFY_TRACK ||--o{ SPOTIFY_ARTIST : ""
    SPOTIFY_TRACK ||--|| SPOTIFY_ALBUM : ""
    SPOTIFY_ARTIST }o--|| SPOTIFY_ALBUM : ""

    SPOTIFY_PLAYLIST {
        VARCHAR id PK
	    VARCHAR name
        VARCHAR href
    }
    SKIPPED_TRACK {
        VARCHAR id PK
        INT skipped_time
        VARCHAR playlist_id FK
        VARCHAR track_id FK
    }
    SPOTIFY_TRACK {
        VARCHAR id PK
        VARCHAR artist_id FK
        VARCHAR album_id FK
        VARCHAR name
        VARCHAR length
        VARCHAR href
    }
    SPOTIFY_ARTIST {
        VARCHAR id PK
        VARCHAR name
        VARCHAR href
    }
    SPOTIFY_ALBUM {
        VARCHAR id PK
        VARCHAR name
        VARCHAR href
    }
```