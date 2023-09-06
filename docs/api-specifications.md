## API Specifications

### Spotify

`GET /spotify/user`
###### Returns current users details

Responses: 
- `200 OK`
```json
{
  "id": "spotifyUserId",
  "displayName": "Spotify User Display",
  "email": "spotifyUserEmailsAddress@email.com",
  "href": "https://api.spotify.com/v1/users/spotifyUserId"
}
```

- `500 Internal Server Error`
```json
{
  "message": "Application has not been logged into your Spotify account."
}
```
---

`GET /spotify/playlists`
###### Returns current users playlists

Responses: 
- `200 OK`
```json
[
  {
    "id": "playlist_id_1",
    "name": "Playlist 1 Name",
    "href": "https://open.spotify.com/playlist/playlist_id_1",
    "images": [
      {
        "height": 200,
        "width": 200,
        "url": "https://play-lh.googleusercontent.com/cShys-AmJ93dB0SV8kE6Fl5eSaf4-qMMZdwEDKI5VEmKAXfzOqbiaeAsqqrEBCTdIEs"
      }
    ]
  },
  {
    "id": "playlist_id_2",
    "name": "Playlist 2 Name",
    "href": "https://open.spotify.com/playlist/playlist_id_2",
    "images": [
      {
        "height": 200,
        "width": 200,
        "url": "https://play-lh.googleusercontent.com/cShys-AmJ93dB0SV8kE6Fl5eSaf4-qMMZdwEDKI5VEmKAXfzOqbiaeAsqqrEBCTdIEs"
      }
    ]
  }
]
```

- `500 Internal Server Error`
```json
{
  "message": "Application has not been logged into your Spotify account."
}
```
---

`DELETE /spotify/playlists/{id}/tracks?trackIds[]={track_id_1}&trackIds[]={track_id_2}&trackIds[]={track_id_3}`
###### Removes tracks from Spotify playlist using track Ids

Responses: 
- `204  No Content`

- `500 Internal Server Error`
```json
{
  "message": "Application has not been logged into your Spotify account."
}
```
---

### Database

`GET /data/playlists`
###### Returns playlists being monitored by the current user

Responses: 
- `200 OK`
```json
[
  {
    "id": "playlist_id_1",
    "name": "Playlist 1 Name",
    "href": "https://open.spotify.com/playlist/playlist_id_1",
    "images": [
      {
        "height": 200,
        "width": 200,
        "url": "https://play-lh.googleusercontent.com/cShys-AmJ93dB0SV8kE6Fl5eSaf4-qMMZdwEDKI5VEmKAXfzOqbiaeAsqqrEBCTdIEs"
      }
    ]
  },
  {
    "id": "playlist_id_2",
    "name": "Playlist 2 Name",
    "href": "https://open.spotify.com/playlist/playlist_id_2",
    "images": [
      {
        "height": 200,
        "width": 200,
        "url": "https://play-lh.googleusercontent.com/cShys-AmJ93dB0SV8kE6Fl5eSaf4-qMMZdwEDKI5VEmKAXfzOqbiaeAsqqrEBCTdIEs"
      }
    ]
  }
]
```
---

`POST /data/playlists`
###### Adds playlist to the database to be monitored for the current user

Responses: 
- `201 Created`
```json
{
  "id": "playlist_id",
  "name": "Playlist Name",
  "href": "https://open.spotify.com/playlist/playlist_id"
}
```

- `400 Bad Request`
```json
{
  "message": "Playlist with id: ID already exists"
}
```
---

`GET /data/playlists/{id}`
###### Returns playlist being monitored by the current user by Id

Responses: 
- `200 OK`
```json
{
  "id": "playlist_id",
  "name": "Playlist Name",
  "href": "https://open.spotify.com/playlist/playlist_id"
}
```

- `404 Not Found`
```json
{
  "message": "Could not find playlist with id: ID"
}
```
---

`DELETE /data/playlists/{id}`
###### Deletes playlist being monitored by the current user by Id from the databse

Responses: 
- `204 No Content`

- `404 Not Found`
```json
{
  "message": "Could not find playlist with id: ID"
}
```
---

`GET /data/playlists/{id}/skipped`
###### Returns tracks that have been skipped in the playlist being monitored

Responses: 
- `200 OK`
```json
[
  {
    "id": "track_id_1",
    "name": "Track 1 Name",
    "href": "https://open.spotify.com/track/track_id_1",
    "length": 180000,
    "artist":{
      "id": "artist_id_1",
      "name": "Artist 1 Name",
      "href": "https://open.spotify.com/artist/artist_id_2",
    },
    "album":{
      "id": "album_id_1",
      "name": "Album 1 Name",
      "href": "https://open.spotify.com/album/album_id_1",
    },
    "images": [
      {
        "height": 200,
        "width": 200,
        "url": "https://play-lh.googleusercontent.com/cShys-AmJ93dB0SV8kE6Fl5eSaf4-qMMZdwEDKI5VEmKAXfzOqbiaeAsqqrEBCTdIEs"
      }
    ]
  },
  {
    "id": "track_id_2",
    "name": "Track 2 Name",
    "href": "https://open.spotify.com/track/track_id_2",
    "length": 180000,
    "artist":{
      "id": "artist_id_2",
      "name": "Artist 2 Name",
      "href": "https://open.spotify.com/artist/artist_id_2",
    },
    "album":{
      "id": "album_id_2",
      "name": "Album 2 Name",
      "href": "https://open.spotify.com/album/album_id_2",
    },
    "images": [
      {
        "height": 200,
        "width": 200,
        "url": "https://play-lh.googleusercontent.com/cShys-AmJ93dB0SV8kE6Fl5eSaf4-qMMZdwEDKI5VEmKAXfzOqbiaeAsqqrEBCTdIEs"
      }
    ]
  }
]
```

- `404 Not Found`
```json
{
  "message": "Could not find playlist with id: ID"
}
```
---