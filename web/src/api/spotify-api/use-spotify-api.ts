import { useCallback } from "react";
import { useCommonHeaders } from "api/use-common-headers";
import {
  getSpotifyPlaylists as getSpotifyPlaylistsRequest,
  getSpotifyPlaylist as getSpotifyPlaylistRequest,
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

  return {
    getSpotifyPlaylists,
    getSpotifyPlaylist,
  } as const;
};
