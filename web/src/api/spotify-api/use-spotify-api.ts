import { useCallback } from "react";
import { useCommonHeaders } from "api/use-common-headers";
import {
  getSpotifyPlaylists as getSpotifyPlaylistsRequest,
  getSpotifyPlaylist as getSpotifyPlaylistRequest,
  getSpotifyPlaylistTracks as getSpotifyPlaylistTracksRequest,
  deleteSpotifyPlaylistTracks as deleteSpotifyPlaylistTracksRequest,
} from "./spotify-api";

export const useSpotifyApi = () => {
  const commonHeaders = useCommonHeaders();

  const getSpotifyPlaylists = useCallback(
    () =>
      getSpotifyPlaylistsRequest({
        headers: {
          ...commonHeaders,
        },
      }),
    [commonHeaders]
  );

  const getSpotifyPlaylist = useCallback(
    (id: string) =>
      getSpotifyPlaylistRequest(id, {
        headers: {
          ...commonHeaders,
        },
      }),
    [commonHeaders]
  );

  const getSpotifyPlaylistTracks = useCallback(
    (id: string) =>
      getSpotifyPlaylistTracksRequest(id, {
        headers: {
          ...commonHeaders,
        },
      }),
    [commonHeaders]
  );

  const deleteSpotifyPlaylistTracks = useCallback(
    (playlistId: string, trackIds: string[]) =>
      deleteSpotifyPlaylistTracksRequest(playlistId, trackIds, {
        headers: {
          ...commonHeaders,
        },
      }),
    [commonHeaders]
  );

  return {
    getSpotifyPlaylists,
    getSpotifyPlaylist,
    getSpotifyPlaylistTracks,
    deleteSpotifyPlaylistTracks,
  } as const;
};
