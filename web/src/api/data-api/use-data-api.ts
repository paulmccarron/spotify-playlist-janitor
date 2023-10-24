import { useCallback } from "react";
import { useCommonHeaders } from "api/use-common-headers";
import { getDatabasePlaylists as getDatabasePlaylistsRequest } from "./data-api";

export const useDataApi = () => {
  const commonHeaders = useCommonHeaders();

  const getDatabasePlaylists = useCallback(
    () =>
      getDatabasePlaylistsRequest({
        headers: {
          ...commonHeaders,
        },
      }),
    [commonHeaders]
  );

  return {
    getDatabasePlaylists,
  } as const;
};
