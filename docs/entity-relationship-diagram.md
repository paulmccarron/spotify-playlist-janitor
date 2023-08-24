# spotify-playlist-janitor Entity-Relationship Diagram

Current interation of the Entity-Relationship Diagram for the Spotify Playlist Janitor project, check `git` history for previous versions.

## Entity-Relationship Diagram
``` mermaid
%%{init: {'theme':'neutral'}}%%
erDiagram
    PLAYLIST ||--|| SKIPPED_TRACK : ""
    SKIPPED_TRACK ||--|| TRACK : ""
    TRACK ||--o{ ARTIST : ""
    TRACK ||--|| ALBUM : ""
    ARTIST }o--|| ALBUM : ""

    PLAYLIST {
        VARCHAR id PK
	    VARCHAR name
        VARCHAR href
    }
    SKIPPED_TRACK {
        VARCHAR id PK
        VARCHAR playlist_id FK
        VARCHAR track_id FK
    }
    TRACK {
        VARCHAR id PK
        VARCHAR artist_id FK
        VARCHAR album_id FK
        VARCHAR name
        VARCHAR length
        VARCHAR href
    }
    ARTIST {
        VARCHAR id PK
        VARCHAR name
        VARCHAR href
    }
    ALBUM {
        VARCHAR id PK
        VARCHAR name
        VARCHAR href
    }
```