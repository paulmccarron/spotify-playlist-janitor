import { Playlist } from "modules/home/home-types";
import { databasePlaylists, spotifyPlaylists } from "shared/mock-data/api";

export const monitoredPlaylists: Playlist[] = spotifyPlaylists
  .filter((spotifyPlaylist) =>
    databasePlaylists.some(
      (databasePlaylist) => databasePlaylist.id === spotifyPlaylist.id
    )
  )
  .map((monitoredPlaylist) => ({
    id: monitoredPlaylist.id,
    href: monitoredPlaylist.href,
    name: monitoredPlaylist.name,
    image: monitoredPlaylist.images[0],
  }));

export const unmonitoredPlaylists: Playlist[] = spotifyPlaylists
  .filter(
    (spotifyPlaylist) =>
      !databasePlaylists.some(
        (databasePlaylist) => databasePlaylist.id === spotifyPlaylist.id
      )
  )
  .map((monitoredPlaylist) => ({
    id: monitoredPlaylist.id,
    href: monitoredPlaylist.href,
    name: monitoredPlaylist.name,
    image: monitoredPlaylist.images[0],
  }));
