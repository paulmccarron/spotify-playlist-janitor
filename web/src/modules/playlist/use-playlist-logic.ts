import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import { useDataApi } from "api/data-api";
import { useSpotifyApi } from "api/spotify-api";

import { DetailedPlaylist } from "./playlist-types";
import { useModal } from "shared/components/modal";
import { DEFAULT_IMAGE } from "shared/constants";

type PlaylistProps = {
  id?: string;
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
  //   const [modalSaving, setModalSaving] = useState(false);
  const [playlist, setPlaylist] = useState<DetailedPlaylist | undefined>(
    undefined
  );
  //   const [modalError, setModalError] = useState<string | undefined>(undefined);

    const { isOpen: editOpen, onOpen: onEditOpen, onClose: onEditClose } = useModal();
    const { isOpen: deleteOpen, onOpen: onDeleteOpen, onClose: onDeleteClose } = useModal();

  //   const onModalClose = useCallback(() => {
  //     setModalError(undefined);
  //     onClose();
  //   }, [setModalError, onClose]);

  const getPlaylistData = useCallback(
    async (id: string) => {
      try {
        setLoading(true);
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

  //   const onSubmit = useCallback(
  //     async (event: React.FormEvent<HTMLFormElement>) => {
  //       event.preventDefault();
  //       setModalError(undefined);
  //       const playlistId = (event.currentTarget[1] as HTMLInputElement).value;
  //       const skipThreshold = (event.currentTarget[2] as HTMLInputElement).value;
  //       const ignoreInitialSkips = (event.currentTarget[3] as HTMLInputElement)
  //         .checked;
  //       const autoDeleteTracksAfter = (event.currentTarget[4] as HTMLInputElement)
  //         .value;

  //       if (!playlistId) {
  //         setModalError("Please select a playlist");
  //         return;
  //       }
  //       if (!!autoDeleteTracksAfter && parseInt(autoDeleteTracksAfter) === 0) {
  //         setModalError("Auto-delete must be empty or greater than 0");
  //         return;
  //       }

  //       try {
  //         setModalSaving(true);

  //         const request = {
  //           id: playlistId,
  //           skipThreshold: !!skipThreshold ? parseInt(skipThreshold) : undefined,
  //           ignoreInitialSkips: ignoreInitialSkips,
  //           autoCleanupLimit: !!autoDeleteTracksAfter
  //             ? parseInt(autoDeleteTracksAfter)
  //             : undefined,
  //         };

  //         await addDatabasePlaylist(request);
  //         setModalSaving(false);
  //         onModalClose();
  //         await getPlaylistData();
  //       } catch (e: any) {
  //         setModalError(e.response?.data?.message || "Unknown error");
  //         setModalSaving(false);
  //       }
  //     },
  //     [addDatabasePlaylist, getPlaylistData, onModalClose]
  //   );

  return {
    loading,
    notFound,
    playlist,
    // modalOpen,
    // onModalOpen,
    // onModalClose,
    // onSubmit,
    // modalError,
    // modalSaving,
  } as const;
};
