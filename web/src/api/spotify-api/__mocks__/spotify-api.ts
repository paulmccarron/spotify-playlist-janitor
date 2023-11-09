import { spotifyPlaylists, spotifyTracks } from "../../../shared/mock-data/api";

export const getSpotifyPlaylists = jest.fn(() =>
  Promise.resolve({
    data: spotifyPlaylists,
  })
);

export const getSpotifyPlaylist = jest.fn((id: string) =>
  Promise.resolve({
    data: spotifyPlaylists.find((x) => x.id === id),
  })
);

export const getSpotifyPlaylistTracks = jest.fn((id: string) =>
  Promise.resolve({
    data: spotifyTracks,
  })
);

export const deleteSpotifyPlaylistTracks = jest.fn((id: string) =>
  Promise.resolve()
);
