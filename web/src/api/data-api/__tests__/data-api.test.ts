import { get, post } from "api/api";
import { getDatabasePlaylists, addDatabasePlaylist } from "../data-api";
import { AddDatabasePlaylistRequest } from "../data-api-types";

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
