import { spotifyPlaylists, spotifyTracks } from "../../../shared/mock-data/api";

export const useSpotifyApi = jest.fn(() => ({
  getSpotifyPlaylists: jest.fn(() =>
    Promise.resolve({
      data: spotifyPlaylists,
    })
  ),
  getSpotifyPlaylist: jest.fn((id: string) =>
    Promise.resolve({
      data: spotifyPlaylists.find((x) => x.id === id),
    })
  ),
  getSpotifyPlaylistTracks: jest.fn((id: string) =>
    Promise.resolve({
      data: spotifyTracks,
    })
  ),
  deleteSpotifyPlaylistTracks: jest.fn((id: string) => Promise.resolve()),
}));
