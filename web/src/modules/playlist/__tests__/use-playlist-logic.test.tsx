import { RefObject } from "react";
import { act, renderHook, waitFor } from "@testing-library/react";

import { usePlaylistLogic } from "../use-playlist-logic";
import { useDataApi } from "api/data-api";
import { useSpotifyApi } from "api/spotify-api";
import { databasePlaylists, spotifyPlaylists } from "shared/mock-data/api";

const mockNavigate = jest.fn();

jest.mock("api/data-api");
jest.mock("api/spotify-api");
jest.mock("react-router-dom", () => ({
  useNavigate: jest.fn(() => mockNavigate),
}));

describe("usePlaylistLogic", () => {
  let result: RefObject<ReturnType<typeof usePlaylistLogic>>;
  const id = databasePlaylists[0].id;
  let mockUseDataApi: any;
  let mockUseSpotifyApi: any;
  let event: any;

  beforeEach(async () => {
    mockUseDataApi = {
      getDatabasePlaylist: jest.fn((id: string) =>
        Promise.resolve({ data: databasePlaylists.find((x) => x.id === id) })
      ),
      updateDatabasePlaylist: jest.fn(() => Promise.resolve()),
      deleteDatabasePlaylist: jest.fn(() => Promise.resolve()),
    };

    mockUseSpotifyApi = {
      getSpotifyPlaylist: jest.fn((id: string) =>
        Promise.resolve({ data: spotifyPlaylists.find((x) => x.id === id) })
      ),
    };

    event = {
      preventDefault: jest.fn(),
      currentTarget: [{ value: "10" }, { checked: false }, { value: "10" }],
    };

    jest.mocked(useSpotifyApi).mockImplementation(() => mockUseSpotifyApi);
    jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
    await waitFor(() => {
      ({ result } = renderHook(() => usePlaylistLogic({ id })));
    });
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("should fetch playlist data on load", async () => {
    await waitFor(() => {
      expect(mockUseDataApi.getDatabasePlaylist).toHaveBeenNthCalledWith(1, id);
      expect(mockUseSpotifyApi.getSpotifyPlaylist).toHaveBeenNthCalledWith(
        1,
        id
      );
    });
  });

  it("should set playlist on load", async () => {
    const expectedPlaylist = {
      id: databasePlaylists[0].id,
      skipThreshold: databasePlaylists[0].skipThreshold,
      ignoreInitialSkips: databasePlaylists[0].ignoreInitialSkips,
      autoCleanupLimit: databasePlaylists[0].autoCleanupLimit,
      name: spotifyPlaylists[0].name,
      href: spotifyPlaylists[0].href,
      image: spotifyPlaylists[0].images[0],
    };

    await waitFor(() => {
      expect(result.current?.playlist).toStrictEqual(expectedPlaylist);
    });
  });

  it("should set notFound to true if getDatabasePlaylist errors with 404", async () => {
    mockUseDataApi = {
      ...mockUseDataApi,
      getDatabasePlaylist: jest.fn(() =>
        Promise.reject({
          response: {
            status: 404,
          },
        })
      ),
    };

    jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
    await waitFor(() => {
      ({ result } = renderHook(() => usePlaylistLogic({ id })));
    });

    await waitFor(() => expect(result.current?.notFound).toBe(true));
  });

  it("should set editError when onEditSubmit called when autoDeleteTracksAfter is 0", () => {
    event.currentTarget = [{ value: "10" }, { checked: false }, { value: "0" }];

    act(() => {
      result.current?.onEditSubmit(event);
    });

    expect(result.current?.editError).toBe(
      "Auto-delete must be empty or greater than 0"
    );
  });

  [
    {
      event: {
        preventDefault: jest.fn(),
        currentTarget: [{ value: "10" }, { checked: false }, { value: "10" }],
      },
      request: {
        skipThreshold: 10,
        ignoreInitialSkips: false,
        autoCleanupLimit: 10,
      },
    },
    {
      event: {
        preventDefault: jest.fn(),
        currentTarget: [{ value: "" }, { checked: true }, { value: "" }],
      },
      request: {
        skipThreshold: undefined,
        ignoreInitialSkips: true,
        autoCleanupLimit: undefined,
      },
    },
  ].forEach((setup) => {
    it("should edit playlist with request data", async () => {
      act(() => {
        //@ts-ignore
        result.current?.onEditSubmit(setup.event);
      });

      await waitFor(() => {
        expect(mockUseDataApi.updateDatabasePlaylist).toHaveBeenNthCalledWith(
          1,
          id,
          setup.request
        );
      });
    });
  });

  it("should set fetch playlist info again when onEditSubmit succeeds", async () => {
    act(() => {
      result.current?.onEditSubmit(event);
    });

    await waitFor(() => {
      expect(mockUseDataApi.updateDatabasePlaylist).toHaveBeenCalledTimes(1);
      expect(mockUseDataApi.getDatabasePlaylist).toHaveBeenCalledTimes(2);
      expect(mockUseSpotifyApi.getSpotifyPlaylist).toHaveBeenCalledTimes(2);
    });
  });

  [
    {
      e: {
        response: {
          status: 500,
          data: {
            message: "Can't update.",
          },
        },
      },
      expectedMessage: "Can't update.",
    },
    {
      e: {
        response: {
          status: 500,
          message: "Irregular format.",
        },
      },
      expectedMessage: "Unknown error",
    },
  ].forEach((setup) => {
    it(`should set editError: ${setup.expectedMessage} when onEditSubmit fails`, async () => {
      mockUseDataApi = {
        ...mockUseDataApi,
        updateDatabasePlaylist: jest.fn(() => Promise.reject(setup.e)),
      };

      jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
      await waitFor(() => {
        ({ result } = renderHook(() => usePlaylistLogic({ id })));
      });

      act(() => {
        result.current?.onEditSubmit(event);
      });

      await waitFor(() => {
        expect(mockUseDataApi.updateDatabasePlaylist).toHaveBeenCalledTimes(1);

        expect(result.current?.editError).toBe(setup.expectedMessage);
      });
    });
  }, []);

  it("should delete playlist with request data and nabivate home", async () => {
    act(() => {
      //@ts-ignore
      result.current?.onDeleteSubmit(event);
    });

    await waitFor(() => {
      expect(mockUseDataApi.deleteDatabasePlaylist).toHaveBeenNthCalledWith(
        1,
        id
      );
      expect(mockNavigate).toHaveBeenCalledWith("/");
    });
  });

  [
    {
      e: {
        response: {
          status: 500,
          data: {
            message: "Can't delete.",
          },
        },
      },
      expectedMessage: "Can't delete.",
    },
    {
      e: {
        response: {
          status: 500,
          message: "Irregular format.",
        },
      },
      expectedMessage: "Unknown error",
    },
  ].forEach((setup) => {
    it(`should set deleteError: ${setup.expectedMessage} when onDeleteSubmit fails`, async () => {
      mockUseDataApi = {
        ...mockUseDataApi,
        deleteDatabasePlaylist: jest.fn(() => Promise.reject(setup.e)),
      };

      jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
      await waitFor(() => {
        ({ result } = renderHook(() => usePlaylistLogic({ id })));
      });

      act(() => {
        result.current?.onDeleteSubmit(event);
      });

      await waitFor(() => {
        expect(mockUseDataApi.deleteDatabasePlaylist).toHaveBeenCalledTimes(1);

        expect(result.current?.deleteError).toBe(setup.expectedMessage);
      });
    });
  }, []);
});
