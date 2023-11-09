import { databasePlaylists, skippedTracks } from "../../../shared/mock-data/api";

export const getDatabasePlaylists = jest.fn(() =>
  Promise.resolve({
    data: databasePlaylists,
  })
);

export const getDatabasePlaylist = jest.fn((id: string) =>
  Promise.resolve({
    data: databasePlaylists.find(x => x.id === id),
  })
);

export const addDatabasePlaylist = jest.fn(() => Promise.resolve());
export const updateDatabasePlaylist = jest.fn(() => Promise.resolve());

export const getDatabasePlaylistSkippedTracks = jest.fn((id: string) =>
  Promise.resolve({
    data: skippedTracks,
  })
);
