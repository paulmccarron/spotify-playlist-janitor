import { deleteRequest, get, post, put } from "api/api";
import { getDatabasePlaylists, getDatabasePlaylist, addDatabasePlaylist, updateDatabasePlaylist, deleteDatabasePlaylist, getDatabasePlaylistSkippedTracks } from "../data-api";
import { AddDatabasePlaylistRequest, UpdateDatabasePlaylistRequest } from "../data-api-types";

jest.mock("api/api");

describe("getDatabasePlaylists", () => {
  const config = { headers: { HEADER: "HEADER" } };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it(`should call the get function with the /data/playlists URL when the getDatabasePlaylists function is called`, () => {
    getDatabasePlaylists(config);
    expect(get).toHaveBeenLastCalledWith("/data/playlists", config);
  });
});

describe("getDatabasePlaylist", () => {
  const config = { headers: { HEADER: "HEADER" } };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it(`should call the get function with the /data/playlists/{id} URL when the getDatabasePlaylist function is called`, () => {
    getDatabasePlaylist("testId", config);
    expect(get).toHaveBeenLastCalledWith("/data/playlists/testId", config);
  });
});

describe("addDatabasePlaylist", () => {
  const config = { headers: { HEADER: "HEADER" } };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it(`should call the post function with the /data/playlists URL when the addDatabasePlaylist function is called`, () => {
    const body: AddDatabasePlaylistRequest = {
      id: "id",
      ignoreInitialSkips: true,
      autoCleanupLimit: 56,
      skipThreshold: 54,
    };
    addDatabasePlaylist(body, config);
    expect(post).toHaveBeenLastCalledWith("/data/playlists", body, config);
  });
});

describe("updateDatabasePlaylist", () => {
  const config = { headers: { HEADER: "HEADER" } };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it(`should call the out function with the /data/playlists URL when the updateDatabasePlaylist function is called`, () => {
    const body: UpdateDatabasePlaylistRequest = {
      ignoreInitialSkips: true,
      autoCleanupLimit: 56,
      skipThreshold: 54,
    };
    updateDatabasePlaylist("testId", body, config);
    expect(put).toHaveBeenLastCalledWith("/data/playlists/testId", body, config);
  });
});

describe("deleteDatabasePlaylist", () => {
  const config = { headers: { HEADER: "HEADER" } };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it(`should call the get function with the /data/playlists/{id} URL when the deleteDatabasePlaylist function is called`, () => {
    deleteDatabasePlaylist("testId", config);
    expect(deleteRequest).toHaveBeenLastCalledWith("/data/playlists/testId", config);
  });
});

describe("getDatabasePlaylistSkippedTracks", () => {
  const config = { headers: { HEADER: "HEADER" } };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it(`should call the get function with the /data/playlists/{id}/skipped URL when the getDatabasePlaylistSkippedTracks function is called`, () => {
    getDatabasePlaylistSkippedTracks("testId", config);
    expect(get).toHaveBeenLastCalledWith("/data/playlists/testId/skipped", config);
  });
});
