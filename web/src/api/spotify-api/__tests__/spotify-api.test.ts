import { get } from "api/api";
import { getSpotifyPlaylist, getSpotifyPlaylists } from "../spotify-api";

jest.mock("api/api");

describe("getSpotifyPlaylists", () => {
  const config = { headers: { HEADER: "HEADER" } };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it(`should call the get function with the /spotify/playlists URL when the getSpotifyPlaylists function is called`, () => {
    getSpotifyPlaylists(config);
    expect(get).toHaveBeenLastCalledWith("/spotify/playlists", config);
  });

  it(`should call the get function with the /spotify/playlists/{id} URL when the getSpotifyPlaylist function is called`, () => {
    getSpotifyPlaylist("testId", config);
    expect(get).toHaveBeenLastCalledWith("/spotify/playlists/testId", config);
  });
});
