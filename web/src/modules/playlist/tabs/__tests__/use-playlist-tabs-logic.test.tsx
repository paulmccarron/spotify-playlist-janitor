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
      deleteSpotifyPlaylistTracks: jest.fn((id: string) => Promise.resolve()),
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
      expect(result.current?.spotifyTracks).toStrictEqual(expectedSpotifyTracks);
    });
  });

  // it("should set notFound to true if getDatabasePlaylist errors with 404", async () => {
  //   mockUseDataApi = {
  //     ...mockUseDataApi,
  //     getDatabasePlaylist: jest.fn(() =>
  //       Promise.reject({
  //         response: {
  //           status: 404,
  //         },
  //       })
  //     ),
  //   };

  //   jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
  //   await waitFor(() => {
  //     ({ result } = renderHook(() => usePlaylistLogic({ id })));
  //   });

  //   await waitFor(() =>
  //     expect(result.current?.notFound).toBe(true)
  //   );
  // });

  // it("should set editError when onEditSubmit called when autoDeleteTracksAfter is 0", () => {
  //   event.currentTarget = [
  //     { value: "10" },
  //     { checked: false },
  //     { value: "0" },
  //   ];

  //   act(() => {
  //     result.current?.onEditSubmit(event);
  //   });

  //   expect(result.current?.editError).toBe(
  //     "Auto-delete must be empty or greater than 0"
  //   );
  // });

  // [
  //   {
  //     event: {
  //       preventDefault: jest.fn(),
  //       currentTarget: [
  //         { value: "10" },
  //         { checked: false },
  //         { value: "10" },
  //       ],
  //     },
  //     request: {
  //       skipThreshold: 10,
  //       ignoreInitialSkips: false,
  //       autoCleanupLimit: 10,
  //     },
  //   },
  //   {
  //     event: {
  //       preventDefault: jest.fn(),
  //       currentTarget: [
  //         { value: "" },
  //         { checked: true },
  //         { value: "" },
  //       ],
  //     },
  //     request: {
  //       skipThreshold: undefined,
  //       ignoreInitialSkips: true,
  //       autoCleanupLimit: undefined,
  //     },
  //   },
  // ].forEach((setup) => {
  //   it("should edit playlist with request data", async () => {
  //     act(() => {
  //       //@ts-ignore
  //       result.current?.onEditSubmit(setup.event);
  //     });

  //     await waitFor(() => {
  //       expect(mockUseDataApi.updateDatabasePlaylist).toHaveBeenNthCalledWith(
  //         1,
  //         id,
  //         setup.request
  //       );
  //     });
  //   });
  // });

  // it("should set fetch playlist info again when onEditSubmit succeeds", async () => {
  //   act(() => {
  //     result.current?.onEditSubmit(event);
  //   });

  //   await waitFor(() => {
  //     expect(mockUseDataApi.updateDatabasePlaylist).toHaveBeenCalledTimes(1);
  //     expect(mockUseDataApi.getDatabasePlaylist).toHaveBeenCalledTimes(2);
  //     expect(mockUseSpotifyApi.getSpotifyPlaylist).toHaveBeenCalledTimes(2);
  //   });
  // });

  // [
  //   {
  //     e: {
  //       response: {
  //         status: 500,
  //         data: {
  //           message:
  //             "Can't update.",
  //         },
  //       }, message: "Can't update."
  //     }
  //   },
  //   {
  //     e: {
  //       response: {
  //         status: 500,
  //         message: "Irregular format.",
  //       }
  //     }, message: "Unknown error"
  //   }
  // ].forEach((setup) => {
  //   it("should set editError when onEditSubmit fails", async () => {
  //     mockUseDataApi = {
  //       ...mockUseDataApi,
  //       updateDatabasePlaylist: jest.fn(() =>
  //         Promise.reject(setup.e)
  //       ),
  //     };

  //     jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
  //     await waitFor(() => {
  //       ({ result } = renderHook(() => usePlaylistLogic({ id })));
  //     });

  //     act(() => {
  //       result.current?.onEditSubmit(event);
  //     });

  //     await waitFor(() => {
  //       expect(mockUseDataApi.updateDatabasePlaylist).toHaveBeenCalledTimes(1);

  //       expect(result.current?.editError).toBe(setup.message)
  //     });
  //   });
  // }, []);

  // it("should delete playlist with request data and nabivate home", async () => {
  //   act(() => {
  //     //@ts-ignore
  //     result.current?.onDeleteSubmit(event);
  //   });

  //   await waitFor(() => {
  //     expect(mockUseDataApi.deleteDatabasePlaylist).toHaveBeenNthCalledWith(
  //       1,
  //       id,
  //     );
  //     expect(mockNavigate).toHaveBeenCalledWith("/");
  //   });
  // });

  // [
  //   {
  //     e: {
  //       response: {
  //         status: 500,
  //         data: {
  //           message:
  //             "Can't update.",
  //         },
  //       }, message: "Can't update."
  //     }
  //   },
  //   {
  //     e: {
  //       response: {
  //         status: 500,
  //         message: "Irregular format.",
  //       }
  //     }, message: "Unknown error"
  //   }
  // ].forEach((setup) => {
  //   it("should set deleteError when onDeleteSubmit fails", async () => {
  //     mockUseDataApi = {
  //       ...mockUseDataApi,
  //       deleteDatabasePlaylist: jest.fn(() =>
  //         Promise.reject(setup.e)
  //       ),
  //     };

  //     jest.mocked(useDataApi).mockImplementation(() => mockUseDataApi);
  //     await waitFor(() => {
  //       ({ result } = renderHook(() => usePlaylistLogic({ id })));
  //     });

  //     act(() => {
  //       result.current?.onDeleteSubmit(event);
  //     });

  //     await waitFor(() => {
  //       expect(mockUseDataApi.deleteDatabasePlaylist).toHaveBeenCalledTimes(1);

  //       expect(result.current?.deleteError).toBe(setup.message)
  //     });
  //   });
  // }, []);
});
