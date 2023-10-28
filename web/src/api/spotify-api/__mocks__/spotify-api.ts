import { spotifyPlaylists } from "../../../shared/mock-data/api";

export const getSpotifyPlaylists = jest.fn(() =>
  Promise.resolve({
    data: spotifyPlaylists,
  })
);
