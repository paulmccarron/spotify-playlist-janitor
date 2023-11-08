import { RefObject } from "react";
import { act, renderHook, waitFor } from "@testing-library/react";

import { useHomeLogic } from "../use-home-logic";
import { useDataApi } from "api/data-api";
import { useSpotifyApi } from "api/spotify-api";
import { databasePlaylists, spotifyPlaylists } from "shared/mock-data/api";
import { unmonitoredPlaylists } from "shared/mock-data/home";

const mockNavigate = jest.fn();

jest.mock("api/data-api");
jest.mock("api/spotify-api");
jest.mock("react-router-dom", () => ({
  useNavigate: jest.fn(() => mockNavigate),
}));

describe("useHomeLogic", () => {
  let result: RefObject<ReturnType<typeof useHomeLogic>>;
  let mockUseDataApi: any;
  let mockUseSpotifyApi: any;
  let event: any;

  beforeEach(async () => {
    mockUseDataApi = {
      getDatabasePlaylists: jest.fn(() =>
        Promise.resolve({ data: databasePlaylists })
      ),
      addDatabasePlaylist: jest.fn(() => Promise.resolve()),
    };

    mockUseSpotifyApi = {
      getSpotifyPlaylists: jest.fn(() =>
        Promise.resolve({ data: spotifyPlaylists })
      ),
    };

    event = {
      preventDefault: jest.fn(),
      currentTarget: [
        {},
        { value: "playlistIdValue" },
        { value: "10" },
        { checked: false },
        { value: "10" },
      ],
    };

    jest.mocked(useSpotifyApi).mockImplementation(() => mockUseSpotifyApi);
    jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
    await waitFor(() => {
      ({ result } = renderHook(() => useHomeLogic()));
    });
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("should fetch playlist data on load", async () => {
    await waitFor(() => {
      expect(mockUseDataApi.getDatabasePlaylists).toHaveBeenCalledTimes(1);
      expect(mockUseSpotifyApi.getSpotifyPlaylists).toHaveBeenCalledTimes(1);
      expect(result.current?.showSpotifyAuthModal).toBe(false);
    });
  });

  it("should set monitoredPlaylists on load", async () => {
    const expectedMonitoredPlaylists = spotifyPlaylists
      .filter((spotifyPlaylist) =>
        databasePlaylists.some(
          (databasePlaylist) => databasePlaylist.id === spotifyPlaylist.id
        )
      )
      .map((monitoredPlaylist) => ({
        id: monitoredPlaylist.id,
        href: monitoredPlaylist.href,
        name: monitoredPlaylist.name,
        image: monitoredPlaylist.images[0],
      }));

    await waitFor(() => {
      expect(result.current?.monitoredPlaylists).toStrictEqual(
        expectedMonitoredPlaylists
      );
    });
  });

  it("should set unmonitoredPlaylists on load", async () => {
    const expectedUnmonitoredPlaylists = spotifyPlaylists
      .filter(
        (spotifyPlaylist) =>
          !databasePlaylists.some(
            (databasePlaylist) => databasePlaylist.id === spotifyPlaylist.id
          )
      )
      .map((monitoredPlaylist) => ({
        id: monitoredPlaylist.id,
        href: monitoredPlaylist.href,
        name: monitoredPlaylist.name,
        image: monitoredPlaylist.images[0],
      }));

    await waitFor(() => {
      expect(result.current?.unmonitoredPlaylists).toStrictEqual(
        expectedUnmonitoredPlaylists
      );
    });
  });

  it("should set showSpotifyAuthModal to true getDatabasePlaylists errors", async () => {
    mockUseDataApi = {
      ...mockUseDataApi,
      getDatabasePlaylists: jest.fn(() =>
        Promise.reject({
          response: {
            status: 500,
            data: {
              message:
                "Application has not been logged into your Spotify account.",
            },
          },
        })
      ),
    };

    jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
    await waitFor(() => {
      ({ result } = renderHook(() => useHomeLogic()));
    });

    await waitFor(() =>
      expect(result.current?.showSpotifyAuthModal).toBe(true)
    );
  });

  it("should set modalError when onSubmit called when playlist select is empty", () => {
    event.currentTarget = [
      {},
      { value: "" },
      { value: "10" },
      { checked: false },
      { value: "10" },
    ];

    act(() => {
      result.current?.onSubmit(event);
    });

    expect(result.current?.modalError).toBe("Please select a playlist");
  });

  it("should set modalError when onSubmit called when autoDeleteTracksAfter is 0", () => {
    event.currentTarget = [
      {},
      { value: "playlistIdValue" },
      { value: "10" },
      { checked: false },
      { value: "0" },
    ];

    act(() => {
      result.current?.onSubmit(event);
    });

    expect(result.current?.modalError).toBe(
      "Auto-delete must be empty or greater than 0"
    );
  });

  [
    {
      event: {
        preventDefault: jest.fn(),
        currentTarget: [
          {},
          { value: "playlistIdValue" },
          { value: "10" },
          { checked: false },
          { value: "10" },
        ],
      },
      request: {
        id: "playlistIdValue",
        skipThreshold: 10,
        ignoreInitialSkips: false,
        autoCleanupLimit: 10,
      },
    },
    {
      event: {
        preventDefault: jest.fn(),
        currentTarget: [
          {},
          { value: "playlistIdValue" },
          { value: "" },
          { checked: true },
          { value: "" },
        ],
      },
      request: {
        id: "playlistIdValue",
        skipThreshold: undefined,
        ignoreInitialSkips: true,
        autoCleanupLimit: undefined,
      },
    },
  ].forEach((setup) => {
    it("should add playlist with request data", async () => {
      act(() => {
        //@ts-ignore
        result.current?.onSubmit(setup.event);
      });

      await waitFor(() => {
        expect(mockUseDataApi.addDatabasePlaylist).toHaveBeenNthCalledWith(
          1,
          setup.request
        );
      });
    });
  });

  it("should set fetch playlist data again when onSubmit succeeds", async () => {
    act(() => {
      result.current?.onSubmit(event);
    });

    await waitFor(() => {
      expect(mockUseDataApi.addDatabasePlaylist).toHaveBeenCalledTimes(1);
      expect(mockUseDataApi.getDatabasePlaylists).toHaveBeenCalledTimes(2);
      expect(mockUseSpotifyApi.getSpotifyPlaylists).toHaveBeenCalledTimes(2);
    });
  });

  [
    {
      e: {
        response: {
          status: 500,
          data: {
            message: "Can't add.",
          },
        },
        expectedMessage: "Can't add.",
      },
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
    it(`should set modalError: ${setup.expectedMessage} when onSubmit fails`, async () => {
      mockUseDataApi = {
        ...mockUseDataApi,
        addDatabasePlaylist: jest.fn(() => Promise.reject(setup.e)),
      };

      jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
      await waitFor(() => {
        ({ result } = renderHook(() => useHomeLogic()));
      });

      act(() => {
        result.current?.onSubmit(event);
      });

      await waitFor(() => {
        expect(mockUseDataApi.addDatabasePlaylist).toHaveBeenCalledTimes(1);

        expect(result.current?.modalError).toBe(setup.expectedMessage);
      });
    });
  }, []);

  it("should clear modalError when onPlaylistChange called", () => {
    event.currentTarget = [
      {},
      { value: "" },
      { value: "10" },
      { checked: false },
      { value: "10" },
    ];

    act(() => {
      result.current?.onSubmit(event);
    });

    expect(result.current?.modalError).toBe("Please select a playlist");

    act(() => {
      result.current?.onPlaylistChange({
        label: unmonitoredPlaylists[0].name,
        value: unmonitoredPlaylists[0].id,
      });
    });

    expect(result.current?.modalError).toBe(undefined);
  });

  it("should call naviagte when onPlaylistClick called", () => {
    result.current?.onPlaylistClick("testId");
    expect(mockNavigate).toHaveBeenCalledWith("/playlist/testId");
  });
});
