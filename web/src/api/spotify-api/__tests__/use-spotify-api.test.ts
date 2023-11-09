import { RefObject } from "react";
import { renderHook } from "@testing-library/react";

import { deleteRequest, get } from "api/api";
import { useSpotifyApi } from "../use-spotify-api";

jest.mock("api/api");
jest.mock("shared/state/user");

describe("useSpotifyApi", () => {
  let result: RefObject<ReturnType<typeof useSpotifyApi>>;

  beforeEach(() => {
    ({ result } = renderHook(() => useSpotifyApi()));
  });

  afterEach(() => {
    jest.clearAllMocks();
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

  it("should call the getSpotifyPlaylistTracksRequest function when the getSpotifyPlaylistTracks function is called", () => {
    result.current?.getSpotifyPlaylistTracks("testId");

    expect(get).toHaveBeenCalledWith("/spotify/playlists/testId/tracks", {
      headers: {
        authorization: expect.any(String),
      },
    });
  });

  it("should call the deleteSpotifyPlaylistTracksRequest function when the getSpotifyPlaylist function is called", () => {
    result.current?.deleteSpotifyPlaylistTracks("testId", ["1", "2"]);

    expect(deleteRequest).toHaveBeenCalledWith("/spotify/playlists/testId/tracks?trackIds[]=1&trackIds[]=2", {
      headers: {
        authorization: expect.any(String),
      },
    });
  });
});
