import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import { useDataApi } from "api/data-api";
import { useSpotifyApi } from "api/spotify-api";

import { DetailedPlaylist } from "./playlist-types";
import { useModal } from "shared/components/modal";
import { DEFAULT_IMAGE } from "shared/constants";

type PlaylistProps = {
  id: string;
};

export const usePlaylistLogic = ({ id }: PlaylistProps) => {
  const {
    getDatabasePlaylist,
    updateDatabasePlaylist,
    deleteDatabasePlaylist,
  } = useDataApi();
  const { getSpotifyPlaylist } = useSpotifyApi();
  const navigate = useNavigate();

  const [notFound, setNotFound] = useState(false);
  const [loading, setLoading] = useState(false);
  const [playlist, setPlaylist] = useState<DetailedPlaylist | undefined>(
    undefined
  );
  const [editError, setEditError] = useState<string | undefined>(undefined);
  const [editSaving, setEditSaving] = useState(false);

  const {
    isOpen: editOpen,
    onOpen: onEditOpen,
    onClose: onEditModalClose,
  } = useModal();
  const {
    isOpen: deleteOpen,
    onOpen: onDeleteOpen,
    onClose: onDeleteClose,
  } = useModal();

  const onEditClose = useCallback(() => {
    setEditError(undefined);
    onEditModalClose();
  }, [setEditError, onEditModalClose]);

  const getPlaylistData = useCallback(
    async (id: string) => {
      try {
        setLoading(true);
        setPlaylist(undefined);
        const databasePlaylistsResponse = await getDatabasePlaylist(id);

        const spotifyPlaylistsResponse = await getSpotifyPlaylist(id);

        setPlaylist({
          id: databasePlaylistsResponse.data.id,
          skipThreshold: databasePlaylistsResponse.data.skipThreshold,
          ignoreInitialSkips: databasePlaylistsResponse.data.ignoreInitialSkips,
          autoCleanupLimit: databasePlaylistsResponse.data.autoCleanupLimit,
          name: spotifyPlaylistsResponse.data.name,
          href: spotifyPlaylistsResponse.data.href,
          image: spotifyPlaylistsResponse.data.images[0] ?? DEFAULT_IMAGE,
        });

        setLoading(false);
      } catch (e: any) {
        if (e?.response?.status === 404) {
          setNotFound(true);
        }
        setLoading(false);
      }
    },
    [setLoading, getDatabasePlaylist, getSpotifyPlaylist, setNotFound]
  );

  useEffect(() => {
    if (!!id) {
      getPlaylistData(id);
    }
  }, [getPlaylistData, id]);

  const onEditSubmit = useCallback(
    async (event: React.FormEvent<HTMLFormElement>) => {
      event.preventDefault();
      setEditError(undefined);
      const skipThreshold = (event.currentTarget[0] as HTMLInputElement).value;
      const ignoreInitialSkips = (event.currentTarget[1] as HTMLInputElement)
        .checked;
      const autoDeleteTracksAfter = (event.currentTarget[2] as HTMLInputElement)
        .value;

      if (!!autoDeleteTracksAfter && parseInt(autoDeleteTracksAfter) === 0) {
        setEditError("Auto-delete must be empty or greater than 0");
        return;
      }

      try {
        setEditSaving(true);

        const request = {
          skipThreshold: !!skipThreshold ? parseInt(skipThreshold) : undefined,
          ignoreInitialSkips: ignoreInitialSkips,
          autoCleanupLimit: !!autoDeleteTracksAfter
            ? parseInt(autoDeleteTracksAfter)
            : undefined,
        };

        await updateDatabasePlaylist(id, request);
        setEditSaving(false);
        onEditClose();
        await getPlaylistData(id);
      } catch (e: any) {
        setEditError(e.response?.data?.message || "Unknown error");
        setEditSaving(false);
      }
    },
    [id, updateDatabasePlaylist, getPlaylistData, onEditClose]
  );

  return {
    loading,
    notFound,
    playlist,
    editOpen,
    onEditOpen,
    onEditClose,
    onEditSubmit,
    editError,
    editSaving,
  } as const;
};