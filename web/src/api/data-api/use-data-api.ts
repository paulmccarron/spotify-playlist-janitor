import { useCallback } from "react";
import { useCommonHeaders } from "api/use-common-headers";
import {
  getDatabasePlaylists as getDatabasePlaylistsRequest,
  addDatabasePlaylist as addDatabasePlaylistRequest,
} from "./data-api";
import { AddDatabasePlaylistRequest } from "./data-api-types";

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

  const addDatabasePlaylist = useCallback(
    (request: AddDatabasePlaylistRequest) =>
      addDatabasePlaylistRequest(request, {
        headers: {
          ...commonHeaders,
        },
      }),
    [commonHeaders]
  );

  return {
    getDatabasePlaylists,
    addDatabasePlaylist,
  } as const;
};
