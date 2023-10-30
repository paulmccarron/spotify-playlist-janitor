import { databasePlaylists } from "../../../shared/mock-data/api";

export const useDataApi = jest.fn(() => ({
  getDatabasePlaylists: jest.fn(() =>
    Promise.resolve({
      data: databasePlaylists,
    })
  ),
  getDatabasePlaylist: jest.fn((id: string) =>
    Promise.resolve({
      data: databasePlaylists.find(x => x.id === id),
    })
  ),
  addDatabasePlaylist: jest.fn(() => Promise.resolve()),
  updateDatabasePlaylist: jest.fn(() => Promise.resolve()),
  deleteDatabasePlaylist: jest.fn(() => Promise.resolve()),
}));
