import { RefObject } from "react";
import { renderHook } from "@testing-library/react";

import { get, post } from "api/api";
import { AddDatabasePlaylistRequest } from "../data-api-types";
import { useDataApi } from "../use-data-api";

jest.mock("api/api");
jest.mock("shared/state/user");

describe("useDataApi", () => {
  let result: RefObject<ReturnType<typeof useDataApi>>;

  beforeEach(() => {
    ({ result } = renderHook(() => useDataApi()));
  });

  it("should call the getDatabasePlaylistsRequest function when the getDatabasePlaylists function is called", () => {
    result.current?.getDatabasePlaylists();

    expect(get).toHaveBeenCalledWith("/data/playlists", {
      headers: {
        authorization: expect.any(String),
      },
    });
  });

  it("should call the addDatabasePlaylistRequest function when the addDatabasePlaylist function is called", () => {
    const data: AddDatabasePlaylistRequest = {
      id: "id",
      ignoreInitialSkips: true,
      autoCleanupLimit: 56,
      skipThreshold: 54,
    };
    result.current?.addDatabasePlaylist(data);

    expect(post).toHaveBeenCalledWith(
      "/data/playlists",
      {
        id: "id",
        ignoreInitialSkips: true,
        autoCleanupLimit: 56,
        skipThreshold: 54,
      },
      {
        headers: {
          authorization: expect.any(String),
        },
      }
    );
  });
});
