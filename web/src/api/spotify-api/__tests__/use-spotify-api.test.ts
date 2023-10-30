import { RefObject } from "react";
import { renderHook } from "@testing-library/react";

import { get } from "api/api";
import { useSpotifyApi } from "../use-spotify-api";

jest.mock("api/api");
jest.mock("shared/state/user");

describe("useSpotifyApi", () => {
  let result: RefObject<ReturnType<typeof useSpotifyApi>>;

  beforeEach(() => {
    ({ result } = renderHook(() => useSpotifyApi()));
  });

  it("should call the getSpotifyPlaylistsRequest function when the getSpotifyPlaylists function is called", () => {
    result.current?.getSpotifyPlaylists();

    expect(get).toHaveBeenCalledWith("/spotify/playlists", {
      headers: {
        authorization: expect.any(String),
      },
    });
  });

  it("should call the getSpotifyPlaylistRequest function when the getSpotifyPlaylist function is called", () => {
    result.current?.getSpotifyPlaylist("testId");

    expect(get).toHaveBeenCalledWith("/spotify/playlists/testId", {
      headers: {
        authorization: expect.any(String),
      },
    });
  });
});
