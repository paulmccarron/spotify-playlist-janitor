import { useCallback, useEffect, useState } from "react";

import { useDataApi } from "api/data-api";
import { useSpotifyApi } from "api/spotify-api";

import { Playlist } from "./home-types";
import { useModal } from "shared/components/modal";

export const useHomeLogic = () => {
  const { getDatabasePlaylists, addDatabasePlaylist } = useDataApi();
  const { getSpotifyPlaylists } = useSpotifyApi();

  const [loading, setLoading] = useState(false);
  const [modalSaving, setModalSaving] = useState(false);
  const [showSpotifyAuthModal, setShowSpotifyAuthModal] = useState(false);
  const [monitoredPlaylists, setMonitoredPlaylists] = useState<
    Playlist[] | undefined
  >(undefined);
  const [unmonitoredPlaylists, setUnmonitoredPlaylists] = useState<
    Playlist[] | undefined
  >(undefined);
  const [modalError, setModalError] = useState<string | undefined>(undefined);

  const { isOpen: modalOpen, onOpen: onModalOpen, onClose } = useModal();

  const onModalClose = useCallback(() => {
    setModalError(undefined);
    onClose();
  }, [setModalError, onClose]);

  const getPlaylistData = useCallback(async () => {
    try {
      setLoading(true);
      const databasePlaylistsResponse = await getDatabasePlaylists();
      const spotifyPlaylistsResponse = await getSpotifyPlaylists();

      setMonitoredPlaylists(
        spotifyPlaylistsResponse.data
          .filter((spotifyPlaylist) =>
            databasePlaylistsResponse.data.some(
              (databasePlaylist) => databasePlaylist.id === spotifyPlaylist.id
            )
          )
          .map((monitoredPlaylist) => ({
            id: monitoredPlaylist.id,
            href: monitoredPlaylist.href,
            name: monitoredPlaylist.name,
            image: monitoredPlaylist.images[0],
          }))
      );

      setUnmonitoredPlaylists(
        spotifyPlaylistsResponse.data
          .filter(
            (spotifyPlaylist) =>
              !databasePlaylistsResponse.data.some(
                (databasePlaylist) => databasePlaylist.id === spotifyPlaylist.id
              )
          )
          .map((unmonitoredPlaylist) => ({
            id: unmonitoredPlaylist.id,
            href: unmonitoredPlaylist.href,
            name: unmonitoredPlaylist.name,
            image: unmonitoredPlaylist.images[0],
          }))
      );
      setLoading(false);
    } catch (e: any) {
      if (
        e?.response?.status === 500 &&
        e?.response?.data?.message ===
          "Application has not been logged into your Spotify account."
      ) {
        setShowSpotifyAuthModal(true);
      }
      setLoading(false);
    }
  }, [
    setLoading,
    getDatabasePlaylists,
    getSpotifyPlaylists,
    setMonitoredPlaylists,
    setUnmonitoredPlaylists,
  ]);

  useEffect(() => {
    getPlaylistData();
  }, [getPlaylistData]);

  const onSubmit = useCallback(
    async (event: React.FormEvent<HTMLFormElement>) => {
      event.preventDefault();
      setModalError(undefined);
      const playlistId = (event.currentTarget[1] as HTMLInputElement).value;
      const skipThreshold = (event.currentTarget[2] as HTMLInputElement).value;
      const ignoreInitialSkips = (event.currentTarget[3] as HTMLInputElement)
        .checked;
      const autoDeleteTracksAfter = (event.currentTarget[4] as HTMLInputElement)
        .value;

      if (!playlistId) {
        setModalError("Please select a playlist");
        return;
      }
      if (!!autoDeleteTracksAfter && parseInt(autoDeleteTracksAfter) === 0) {
        setModalError("Auto-delete must be empty or greater than 0");
        return;
      }

      try {
        setModalSaving(true);
        
        const request = {
          id: playlistId,
          skipThreshold: !!skipThreshold ? parseInt(skipThreshold) : undefined,
          ignoreInitialSkips: ignoreInitialSkips,
          autoCleanupLimit: !!autoDeleteTracksAfter
            ? parseInt(autoDeleteTracksAfter)
            : undefined,
        };

        await addDatabasePlaylist(request);
        setModalSaving(false);
        onModalClose();
        await getPlaylistData();
      } catch (e: any) {
        setModalError(e.response?.data?.message || "Unknown error");
        setModalSaving(false);
      }
    },
    [addDatabasePlaylist, getPlaylistData, onModalClose]
  );

  const onPlaylistChange = useCallback(
    async (newValue: { label: string; value: string }) => {
      if (newValue?.value) {
        setModalError(undefined);
      }
    },
    []
  );

  return {
    monitoredPlaylists,
    unmonitoredPlaylists,
    loading,
    modalOpen,
    onModalOpen,
    onModalClose,
    onSubmit,
    onPlaylistChange,
    modalError,
    modalSaving,
    showSpotifyAuthModal,
  } as const;
};
