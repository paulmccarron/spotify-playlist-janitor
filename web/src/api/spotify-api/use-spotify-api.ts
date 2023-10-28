import { useCallback } from "react";
import { useCommonHeaders } from "api/use-common-headers";
import { getSpotifyPlaylists as getSpotifyPlaylistsRequest } from "./spotify-api";

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

  return {
    getSpotifyPlaylists,
  } as const;
};
