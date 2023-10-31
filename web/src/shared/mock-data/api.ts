import { LoginResponse } from "api/auth-api/auth-api-types";
import { DatabasePlaylistResponse } from "api/data-api";
import { SpotifyPlaylistResponse } from "api/spotify-api";

type AuthData = {
  login: LoginResponse;
};

export const auth: AuthData = {
  login: {
    refresh_token: "access_token",
    token_type: "Bearer",
    access_token: "access_token",
    expires_in: 4503,
  },
};

export const databasePlaylists: DatabasePlaylistResponse[] = [
  {
    id: "1",
    skipThreshold: 10,
    ignoreInitialSkips: false,
    autoCleanupLimit: 10,
  },
  {
    id: "2",
    skipThreshold: 2,
    ignoreInitialSkips: false,
    autoCleanupLimit: 2,
  },
  {
    id: "3",
    skipThreshold: 3,
    ignoreInitialSkips: true,
    autoCleanupLimit: 3,
  },
  {
    id: "4",
    skipThreshold: 4,
    ignoreInitialSkips: true,
    autoCleanupLimit: 4,
  },
];

export const spotifyPlaylists: SpotifyPlaylistResponse[] = [
  {
    id: "1",
    name: "Playlist 1",
    href: "https://open.spotify.com/playlist/1",
    images: [
      {
        height: 640,
        width: 640,
        url: "https://mosaic.scdn.co/101",
      },
      {
        height: 300,
        width: 300,
        url: "https://mosaic.scdn.co/102",
      },
      {
        height: 60,
        width: 60,
        url: "https://mosaic.scdn.co/103",
      },
    ],
  },
  {
    id: "2",
    name: "Playlist 2",
    href: "https://open.spotify.com/playlist/2",
    images: [
      {
        height: 640,
        width: 640,
        url: "https://mosaic.scdn.co/201",
      },
      {
        height: 300,
        width: 300,
        url: "https://mosaic.scdn.co/202",
      },
      {
        height: 60,
        width: 60,
        url: "https://mosaic.scdn.co/203",
      },
    ],
  },
  {
    id: "3",
    name: "Playlist 3",
    href: "https://open.spotify.com/playlist/3",
    images: [
      {
        height: 640,
        width: 640,
        url: "https://mosaic.scdn.co/301",
      },
      {
        height: 300,
        width: 300,
        url: "https://mosaic.scdn.co/302",
      },
      {
        height: 60,
        width: 60,
        url: "https://mosaic.scdn.co/303",
      },
    ],
  },
  {
    id: "4",
    name: "Playlist 4",
    href: "https://open.spotify.com/playlist/4",
    images: [
      {
        height: 640,
        width: 640,
        url: "https://mosaic.scdn.co/401",
      },
      {
        height: 300,
        width: 300,
        url: "https://mosaic.scdn.co/402",
      },
      {
        height: 60,
        width: 60,
        url: "https://mosaic.scdn.co/403",
      },
    ],
  },
  {
    id: "5",
    name: "Playlist 5",
    href: "https://open.spotify.com/playlist/5",
    images: [
      {
        height: 640,
        width: 640,
        url: "https://mosaic.scdn.co/501",
      },
      {
        height: 300,
        width: 300,
        url: "https://mosaic.scdn.co/502",
      },
      {
        height: 60,
        width: 60,
        url: "https://mosaic.scdn.co/503",
      },
    ],
  },
  {
    id: "6",
    name: "Playlist 6",
    href: "https://open.spotify.com/playlist/6",
    images: [
      {
        height: 640,
        width: 640,
        url: "https://mosaic.scdn.co/601",
      },
      {
        height: 300,
        width: 300,
        url: "https://mosaic.scdn.co/602",
      },
      {
        height: 60,
        width: 60,
        url: "https://mosaic.scdn.co/603",
      },
    ],
  },
];
