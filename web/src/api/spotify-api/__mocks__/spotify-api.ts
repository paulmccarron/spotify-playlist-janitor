import { spotifyPlaylists } from "../../../shared/mock-data/api";

export const getSpotifyPlaylists = jest.fn(() =>
  Promise.resolve({
    data: spotifyPlaylists,
  })
);

export const getSpotifyPlaylist = jest.fn((id: string) =>
  Promise.resolve({
    data: spotifyPlaylists.find(x => x.id === id),
  })
);
