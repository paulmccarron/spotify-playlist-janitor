# Domain Model

Current interation of the Domain Model for the Spotify Playlist Janitor project, check `git` history for previous versions.

```mermaid
%%{init: {'theme':'neutral'}}%%
erDiagram
    USER ||--o{ PLAYLIST : ""
    PLAYLIST ||--o{ SKIPPED_TRACK : ""
```