# Entity-Relationship Diagram

Current interation of the Entity-Relationship Diagram for the Spotify Playlist Janitor project, check `git` history for previous versions.

``` mermaid
%%{init: {'theme':'neutral'}}%%
erDiagram
    PLAYLISTS ||--o{ SKIPPED_TRACKS : ""
    SKIPPED_TRACKS }o--|| TRACKS : ""
    TRACKS }o--o{ ARTISTS_TRACKS : ""
    ARTISTS_TRACKS }o--o{ ARTISTS : ""
    ARTISTS }o--o{ ARTISTS_ALBUMS : ""
    ARTISTS_ALBUMS }o--o{ ALBUMS : ""
    TRACKS ||--o{ ALBUMS : ""
    ALBUMS }o--o{ ALBUMS_IMAGES : ""
    ALBUMS_IMAGES }o--|| IMAGES : ""
    USERS ||--|| USERS_SPOTIFY_TOKEN : ""

    PLAYLISTS {
        VARCHAR id PK
        INT skip_threshold
        BOOL ignore_initial_skips
        INT auto_cleanup_limit
    }

    ARTISTS {
        VARCHAR id PK
        VARCHAR name
        VARCHAR href
    }

    ALBUMS {
        VARCHAR id PK
        VARCHAR name
        VARCHAR href
    }

    TRACKS {
        VARCHAR id PK
        VARCHAR name
        INT length
        VARCHAR album_id FK
    }

    IMAGES {
        INT id PK
        VARCHAR url
        INT length
        INT length
    }

    ARTISTS_ALBUMS {
        VARCHAR artist_id_album_id PK
        VARCHAR artist_id FK
        VARCHAR album_id FK
    }

    ARTISTS_TRACKS {
        VARCHAR artist_id_track_id PK
        VARCHAR artist_id FK
        VARCHAR track_id FK
    }

    ALBUMS_IMAGES {
        VARCHAR album_id_image_id PK
        VARCHAR album_id FK
        VARCHAR image_id FK
    }
    
    SKIPPED_TRACKS {
        VARCHAR id PK
        TIMESTAMP skipped_time
        VARCHAR playlist_id FK
        VARCHAR track_id FK
    }
    
    USERS {
        VARCHAR id PK
        VARCHAR username
        VARCHAR password_hash
        BOOL is_admin
        VARCHAR refresh_token
        TIMESTAMP refresh_token_expiry
    }
    
    USERS_SPOTIFY_TOKEN {
        VARCHAR username FK
        VARCHAR spotify_token
    }
```