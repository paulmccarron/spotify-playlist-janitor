import { deleteRequest, get } from "api/api";
import { deleteSpotifyPlaylistTracks, getSpotifyPlaylist, getSpotifyPlaylistTracks, getSpotifyPlaylists } from "../spotify-api";

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

  it(`should call the get function with the /spotify/playlists/{id}/tracks URL when the getSpotifyPlaylistTracks function is called`, () => {
    getSpotifyPlaylistTracks("testId", config);
    expect(get).toHaveBeenLastCalledWith("/spotify/playlists/testId/tracks", config);
  });

  it(`should call the deleteRequest function with the /spotify/playlists/{id}/tracks URL when the deleteSpotifyPlaylistTracks function is called`, () => {
    deleteSpotifyPlaylistTracks("testId", [], config);
    expect(deleteRequest).toHaveBeenLastCalledWith("/spotify/playlists/testId/tracks", config);
  });

  it(`should call the deleteRequest function with the /spotify/playlists/{id}/tracks?trackIds URL when the deleteSpotifyPlaylistTracks function is called`, () => {
    deleteSpotifyPlaylistTracks("testId", ["1", "2"], config);
    expect(deleteRequest).toHaveBeenLastCalledWith("/spotify/playlists/testId/tracks?trackIds[]=1&trackIds[]=2", config);
  });
});
