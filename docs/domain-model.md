# spotify-playlist-janitor Domain Model

Current interation of the Domain Model for the Spotify Playlist Janitor project, check `git` history for previous versions.

## Domain Model
```mermaid
%%{init: {'theme':'neutral'}}%%
erDiagram
    USER ||--o{ PLAYLIST : ""
    PLAYLIST ||--o{ SKIPPED_TRACK : ""
```