import { databasePlaylists } from "../../../shared/mock-data/api";

export const getDatabasePlaylists = jest.fn(() =>
  Promise.resolve({
    data: databasePlaylists,
  })
);

export const addDatabasePlaylist = jest.fn(() => Promise.resolve());
