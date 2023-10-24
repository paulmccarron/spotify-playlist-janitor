import { useCallback, useEffect, useMemo, useState } from "react";

import { useDataApi } from "api/data-api";
import { useSpotifyApi } from "api/spotify-api";

import { Playlist } from "./home-types";

export const useHomeLogic = () => {
  const { getDatabasePlaylists } = useDataApi();
  const { getSpotifyPlaylists } = useSpotifyApi();

  const [loading, setLoading] = useState(false);
  const [monitoredPlaylists, setMonitoredPlaylists] = useState<Playlist[]>([]);
  const [unmonitoredPlaylists, setUnmonitoredPlaylists] = useState<Playlist[]>(
    []
  );

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
          image: monitoredPlaylist.images.filter(
            (image) => image.height >= 100 && image.height <= 300
          )[0],
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
          image: unmonitoredPlaylist.images.filter(
            (image) => image.height <= 100
          )[0],
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

  return { monitoredPlaylists, unmonitoredPlaylists, loading } as const;
};
