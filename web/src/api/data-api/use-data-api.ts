import { useCallback } from "react";
import { useCommonHeaders } from "api/use-common-headers";
import {
  getDatabasePlaylists as getDatabasePlaylistsRequest,
  getDatabasePlaylist as getDatabasePlaylistRequest,
  addDatabasePlaylist as addDatabasePlaylistRequest,
  updateDatabasePlaylist as updateDatabasePlaylistRequest,
  deleteDatabasePlaylist as deleteDatabasePlaylistRequest,
} from "./data-api";
import {
  AddDatabasePlaylistRequest,
  UpdateDatabasePlaylistRequest,
} from "./data-api-types";

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

  const getDatabasePlaylist = useCallback(
    (id: string) =>
      getDatabasePlaylistRequest(id, {
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

  const updateDatabasePlaylist = useCallback(
    (id: string, request: UpdateDatabasePlaylistRequest) =>
      updateDatabasePlaylistRequest(id, request, {
        headers: {
          ...commonHeaders,
        },
      }),
    [commonHeaders]
  );

  const deleteDatabasePlaylist = useCallback(
    (id: string) =>
      deleteDatabasePlaylistRequest(id, {
        headers: {
          ...commonHeaders,
        },
      }),

    [commonHeaders]
  );

  return {
    getDatabasePlaylists,
    getDatabasePlaylist,
    addDatabasePlaylist,
    updateDatabasePlaylist,
    deleteDatabasePlaylist,
  } as const;
};
