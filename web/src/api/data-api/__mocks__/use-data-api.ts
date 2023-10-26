import { databasePlaylists } from "../../../shared/mock-data/api";

export const useDataApi = jest.fn(() => ({
  getDatabasePlaylists: jest.fn(() =>
    Promise.resolve({
      data: databasePlaylists,
    })
  ),
  addDatabasePlaylist: jest.fn(() => Promise.resolve()),
}));
