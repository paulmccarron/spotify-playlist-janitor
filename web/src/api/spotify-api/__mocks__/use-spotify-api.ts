import { spotifyPlaylists } from "../../../shared/mock-data/api";

export const useSpotifyApi = jest.fn(() => ({
  getSpotifyPlaylists: jest.fn(() =>
    Promise.resolve({
      data: spotifyPlaylists,
    })
  ),
  getSpotifyPlaylist: jest.fn((id: string) =>
    Promise.resolve({
      data: spotifyPlaylists.find(x => x.id === id),
    })
  ),
}));


