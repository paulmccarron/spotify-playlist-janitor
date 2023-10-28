import { spotifyPlaylists } from "../../../shared/mock-data/api";

export const useSpotifyApi = jest.fn(() => ({
  getSpotifyPlaylists: jest.fn(
    Promise.resolve({
      data: spotifyPlaylists,
    })
  ),
}));
