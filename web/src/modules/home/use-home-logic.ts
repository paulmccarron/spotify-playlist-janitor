import { useCallback, useEffect, useMemo, useState } from "react";

import { useDataApi } from "api/data-api";
import { useSpotifyApi } from "api/spotify-api";

import { Playlist } from "./home-types";
import { useModal } from "shared/components/modal";

export const useHomeLogic = () => {
  const { getDatabasePlaylists } = useDataApi();
  const { getSpotifyPlaylists } = useSpotifyApi();

  const [loading, setLoading] = useState(false);
  const [monitoredPlaylists, setMonitoredPlaylists] = useState<Playlist[] | undefined>(undefined);
  const [unmonitoredPlaylists, setUnmonitoredPlaylists] = useState<Playlist[] | undefined>(undefined);
  const { isOpen: modalOpen, onOpen: onModalOpen, onClose: onModalClose } = useModal();

  const getPlaylistData = useCallback(async () => {
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
      const playlistId = (event.currentTarget[1] as HTMLInputElement).value;
      console.log(playlistId)
    },
    []
  );

  return { monitoredPlaylists, unmonitoredPlaylists, loading, modalOpen, onModalOpen, onModalClose, onSubmit } as const;
};
