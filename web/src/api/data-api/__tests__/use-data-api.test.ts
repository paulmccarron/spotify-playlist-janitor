import { RefObject } from "react";
import { renderHook } from "@testing-library/react";

import { deleteRequest, get, post, put } from "api/api";
import { AddDatabasePlaylistRequest, UpdateDatabasePlaylistRequest } from "../data-api-types";
import { useDataApi } from "../use-data-api";

jest.mock("api/api");
jest.mock("shared/state/user");

describe("useDataApi", () => {
  let result: RefObject<ReturnType<typeof useDataApi>>;

  beforeEach(() => {
    ({ result } = renderHook(() => useDataApi()));
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("should call the getDatabasePlaylistsRequest function when the getDatabasePlaylists function is called", () => {
    result.current?.getDatabasePlaylists();

    expect(get).toHaveBeenCalledWith("/data/playlists", {
      headers: {
        authorization: expect.any(String),
      },
    });
  });

  it("should call the getDatabasePlaylistRequest function when the getDatabasePlaylist function is called", () => {
    result.current?.getDatabasePlaylist("testId");

    expect(get).toHaveBeenCalledWith("/data/playlists/testId", {
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

  it("should call the updateDatabasePlaylistRequest function when the updateDatabasePlaylist function is called", () => {
    const data: UpdateDatabasePlaylistRequest = {
      ignoreInitialSkips: true,
      autoCleanupLimit: 56,
      skipThreshold: 54,
    };
    result.current?.updateDatabasePlaylist("testId", data);

    expect(put).toHaveBeenCalledWith(
      "/data/playlists/testId",
      {
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

  it("should call the deleteDatabasePlaylistRequest function when the deleteDatabasePlaylist function is called", () => {
    result.current?.deleteDatabasePlaylist("testId");

    expect(deleteRequest).toHaveBeenCalledWith("/data/playlists/testId", {
      headers: {
        authorization: expect.any(String),
      },
    });
  });

  it("should call the getDatabasePlaylistSkippedTracksRequest function when the getDatabasePlaylistSkippedTracks function is called", () => {
    result.current?.getDatabasePlaylistSkippedTracks("testId");

    expect(get).toHaveBeenCalledWith("/data/playlists/testId/skipped", {
      headers: {
        authorization: expect.any(String),
      },
    });
  });
});
