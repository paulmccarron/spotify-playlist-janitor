import { RefObject } from "react";
import { act, renderHook, waitFor } from "@testing-library/react";

import { usePlaylistTabsLogic } from "../use-playlist-tabs-logic";
import { useDataApi } from "api/data-api";
import { useSpotifyApi } from "api/spotify-api";
import {
  databasePlaylists,
  skippedTracks,
  spotifyTracks,
} from "shared/mock-data/api";

const mockNavigate = jest.fn();

jest.mock("api/data-api");
jest.mock("api/spotify-api");
jest.mock("react-router-dom", () => ({
  useNavigate: jest.fn(() => mockNavigate),
}));

describe("usePlaylistTabsLogic", () => {
  let result: RefObject<ReturnType<typeof usePlaylistTabsLogic>>;
  const id = databasePlaylists[0].id;
  let mockUseDataApi: any;
  let mockUseSpotifyApi: any;
  let event: any;

  beforeEach(async () => {
    mockUseDataApi = {
      getDatabasePlaylistSkippedTracks: jest.fn(() =>
        Promise.resolve({
          data: skippedTracks,
        })
      ),
    };

    mockUseSpotifyApi = {
      getSpotifyPlaylistTracks: jest.fn((id: string) =>
        Promise.resolve({
          data: spotifyTracks,
        })
      ),
      deleteSpotifyPlaylistTracks: jest.fn(
        (playlistId: string, trackIds: string[]) => Promise.resolve()
      ),
    };

    event = {
      preventDefault: jest.fn(),
    };

    jest.mocked(useSpotifyApi).mockImplementation(() => mockUseSpotifyApi);
    jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
    await waitFor(() => {
      ({ result } = renderHook(() => usePlaylistTabsLogic({ id })));
    });
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("should fetch playlist data on load", async () => {
    await waitFor(() => {
      expect(
        mockUseDataApi.getDatabasePlaylistSkippedTracks
      ).toHaveBeenNthCalledWith(1, id);
      expect(
        mockUseSpotifyApi.getSpotifyPlaylistTracks
      ).toHaveBeenNthCalledWith(1, id);
    });
  });

  it("should set skipped tracks on load", async () => {
    const expectedSkippedTracks = [
      {
        id: skippedTracks[0].trackId,
        name: skippedTracks[0].name,
        duration: skippedTracks[0].duration,
        skippedTotal: 3,
        album: skippedTracks[0].album,
        artists: skippedTracks[0].artists,
        image: skippedTracks[0].album.images.reduce((prev, curr) => {
          return prev.height < curr.height ? prev : curr;
        }),
      },
      {
        id: skippedTracks[3].trackId,
        name: skippedTracks[3].name,
        duration: skippedTracks[3].duration,
        skippedTotal: 2,
        album: skippedTracks[3].album,
        artists: skippedTracks[3].artists,
        image: skippedTracks[3].album.images.reduce((prev, curr) => {
          return prev.height < curr.height ? prev : curr;
        }),
      },
    ];

    await waitFor(() => {
      expect(result.current?.skippedTracks).toStrictEqual(
        expectedSkippedTracks
      );
    });
  });

  it("should set skipped tracks as [] if api errors", async () => {
    mockUseDataApi = {
      getDatabasePlaylistSkippedTracks: jest.fn(() => Promise.reject()),
    };

    jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
    await waitFor(() => {
      ({ result } = renderHook(() => usePlaylistTabsLogic({ id })));
    });

    await waitFor(() => {
      expect(result.current?.skippedTracks).toStrictEqual([]);
      expect(result.current?.skippedTrackHistory).toStrictEqual([]);
    });
  });

  it("should set skipped track history on load", async () => {
    const expectedSkippedTrackHistory = skippedTracks.map((skippedTrack) => ({
      id: skippedTrack.trackId,
      name: skippedTrack.name,
      duration: skippedTrack.duration,
      skippedDate: new Date(skippedTrack.skippedDate),
      album: skippedTrack.album,
      artists: skippedTrack.artists,
      image: skippedTrack.album.images.reduce((prev, curr) => {
        return prev.height < curr.height ? prev : curr;
      }),
    }));

    await waitFor(() => {
      expect(result.current?.skippedTrackHistory).toStrictEqual(
        expectedSkippedTrackHistory
      );
    });
  });

  it("should set skipped tracks and skipped track history as [] if api errors", async () => {
    mockUseDataApi = {
      getDatabasePlaylistSkippedTracks: jest.fn(() => Promise.reject()),
    };

    jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
    await waitFor(() => {
      ({ result } = renderHook(() => usePlaylistTabsLogic({ id })));
    });

    await waitFor(() => {
      expect(result.current?.skippedTracks).toStrictEqual([]);
      expect(result.current?.skippedTrackHistory).toStrictEqual([]);
    });
  });

  it("should set spotify tracks on load", async () => {
    const expectedSpotifyTracks = spotifyTracks.map((spotifyTrack) => {
      return {
        ...spotifyTrack,
        image: spotifyTrack.album.images.reduce((prev, curr) => {
          return prev.height < curr.height ? prev : curr;
        }),
      };
    });
    await waitFor(() => {
      expect(result.current?.spotifyTracks).toStrictEqual(
        expectedSpotifyTracks
      );
    });
  });

  it("should set skipped tracks as [] if api errors", async () => {
    mockUseSpotifyApi = {
      getSpotifyPlaylistTracks: jest.fn(() => Promise.reject()),
    };

    jest.mocked(useSpotifyApi).mockImplementation(() => mockUseSpotifyApi);
    await waitFor(() => {
      ({ result } = renderHook(() => usePlaylistTabsLogic({ id })));
    });

    await waitFor(() => {
      expect(result.current?.spotifyTracks).toStrictEqual([]);
    });
  });

  it("should delete track when onDeleteSubmit called", async () => {
    act(() => {
      result.current?.onDeleteSubmit(event);
    });

    await waitFor(() => {
      expect(
        mockUseSpotifyApi.deleteSpotifyPlaylistTracks
      ).toHaveBeenNthCalledWith(1, id, []);
    });
  });

  it("should set fetch track info again when onDeleteSubmit succeeds", async () => {
    act(() => {
      result.current?.onDeleteSubmit(event);
    });

    await waitFor(() => {
      expect(
        mockUseSpotifyApi.deleteSpotifyPlaylistTracks
      ).toHaveBeenCalledTimes(1);
      expect(
        mockUseDataApi.getDatabasePlaylistSkippedTracks
      ).toHaveBeenCalledTimes(2);
      expect(mockUseSpotifyApi.getSpotifyPlaylistTracks).toHaveBeenCalledTimes(
        2
      );
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
    it(`should set deleteError: ${setup.expectedMessage} when onDeleteSubmit fails `, async () => {
      mockUseSpotifyApi = {
        ...mockUseSpotifyApi,
        deleteSpotifyPlaylistTracks: jest.fn(() => Promise.reject(setup.e)),
      };

      jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
      await waitFor(() => {
        ({ result } = renderHook(() => usePlaylistTabsLogic({ id })));
      });

      act(() => {
        result.current?.onDeleteSubmit(event);
      });

      await waitFor(() => {
        expect(
          mockUseSpotifyApi.deleteSpotifyPlaylistTracks
        ).toHaveBeenCalledTimes(1);

        expect(result.current?.deleteError).toBe(setup.expectedMessage);
      });
    });
  }, []);
});
