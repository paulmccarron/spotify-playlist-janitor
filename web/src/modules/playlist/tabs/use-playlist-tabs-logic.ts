import { useCallback, useEffect, useState } from "react";
import { useDataApi } from "api/data-api";
import { useSpotifyApi } from "api/spotify-api";
import {
  DeleteTrack,
  SkippedTrackHistory,
  SkippedTrackTotal,
  Track,
} from "./playlist-tabs-types";
import { useModal } from "shared/components/modal";
import { usePlaylistTabsColumns } from "./use-playlist-tabs-columns";
import { Image } from "shared/types";
import { DEFAULT_ALBUM_IMAGE } from "shared/constants";

const loadingData: any[] = [{}, {}, {}, {}, {}, {}, {}, {}, {}];

type UsePlaylistTabsLogicProps = {
  id: string;
};

export const usePlaylistTabsLogic = ({ id }: UsePlaylistTabsLogicProps) => {
  const { getDatabasePlaylistSkippedTracks } = useDataApi();
  const { getSpotifyPlaylistTracks, deleteSpotifyPlaylistTracks } =
    useSpotifyApi();

  const [loadingSkippedTracks, setLoadingSkippedTracks] = useState(false);
  const [skippedTracks, setSkippedTracks] = useState<SkippedTrackTotal[]>([]);

  const [loadingSkippedTrackHistory, setLoadingSkippedTrackHistory] =
    useState(false);
  const [skippedTrackHistory, setSkippedTrackHistory] = useState<
    SkippedTrackHistory[]
  >([]);

  const [loadingSpotifyTracks, setLoadingSpotifyTracks] = useState(false);
  const [spotifyTracks, setSpotifyTracks] = useState<Track[]>([]);

  const { isOpen: deleteOpen, onOpen, onClose } = useModal();

  const [deleting, setDeleting] = useState(false);
  const [deleteError, setDeleteError] = useState<string | undefined>(undefined);
  const [deleteTracks, setDeleteTracks] = useState<DeleteTrack[]>([]);

  const onDeleteOpen = useCallback(
    (trackId: string) => {
      const skippedTrack = skippedTracks.find((track) => track.id === trackId);
      if (skippedTrack) {
        setDeleteTracks([{ id: skippedTrack?.id, name: skippedTrack?.name }]);
        onOpen();
      }
      const spotifyTrack = spotifyTracks.find((track) => track.id === trackId);
      if (spotifyTrack) {
        setDeleteTracks([{ id: spotifyTrack?.id, name: spotifyTrack?.name }]);
        onOpen();
      }
    },
    [skippedTracks, spotifyTracks, setDeleteTracks, onOpen]
  );

  const onDeleteClose = useCallback(() => {
    setDeleteTracks([]);
    setDeleteError(undefined);
    onClose();
  }, [setDeleteTracks, onClose]);

  const {
    skippedTrackColumns,
    skippedTrackHistoryColumns,
    spotifyTrackColumns,
  } = usePlaylistTabsColumns({ onDeleteClick: onDeleteOpen });

  const getSkippedTracks = useCallback(
    async (id: string) => {
      try {
        setLoadingSkippedTracks(true);
        setLoadingSkippedTrackHistory(true);
        setSkippedTracks([]);
        setSkippedTrackHistory([]);

        const skippedTrackResponse = await getDatabasePlaylistSkippedTracks(id);

        const skippedTracks: SkippedTrackTotal[] = [];
        var skippedTracksTotals: { [id: string]: SkippedTrackTotal } = {};

        skippedTrackResponse.data.forEach((skippedTrack) => {
          if (skippedTracksTotals[skippedTrack.trackId]) {
            skippedTracksTotals[skippedTrack.trackId].skippedTotal++;
          } else {
            skippedTracksTotals[skippedTrack.trackId] = {
              id: skippedTrack.trackId,
              name: skippedTrack.name,
              duration: skippedTrack.duration,
              skippedTotal: 1,
              album: skippedTrack.album,
              artists: skippedTrack.artists,
              image: skippedTrack.album.images.reduce((prev, curr) => {
                return prev.height < curr.height ? prev : curr;
              }),
            };
          }
        });

        for (let key in skippedTracksTotals) {
          skippedTracks.push(skippedTracksTotals[key]);
        }

        const skippedTrackHistory: SkippedTrackHistory[] =
          skippedTrackResponse.data.map((skippedTrack) => ({
            id: skippedTrack.trackId,
            name: skippedTrack.name,
            duration: skippedTrack.duration,
            skippedDate: new Date(skippedTrack.skippedDate),
            album: skippedTrack.album,
            artists: skippedTrack.artists,
            image: skippedTrack.album.images.reduce((prev, curr) => {
              return prev.height < curr.height ? prev : curr;
            }),
          }));

        setSkippedTracks(skippedTracks);
        setSkippedTrackHistory(skippedTrackHistory);

        setLoadingSkippedTracks(false);
        setLoadingSkippedTrackHistory(false);
      } catch (e: any) {
        // if (e?.response?.status === 404) {
        //   setNotFound(true);
        // }

        setSkippedTracks([]);
        setSkippedTrackHistory([]);

        setLoadingSkippedTracks(false);
        setLoadingSkippedTrackHistory(false);
      }
    },
    [
      setLoadingSkippedTracks,
      setLoadingSkippedTrackHistory,
      getDatabasePlaylistSkippedTracks,
    ]
  );

  const getSpotifyTracks = useCallback(
    async (id: string) => {
      try {
        setLoadingSpotifyTracks(true);
        setSpotifyTracks([]);

        const spotifyTrackResponse = await getSpotifyPlaylistTracks(id);

        const spotifyTracks: Track[] = spotifyTrackResponse.data.map(
          (spotifytrack) => {
            let image: Image = {
              height: 40,
              width: 40,
              url: DEFAULT_ALBUM_IMAGE,
            };

            if (spotifytrack.album.images.length > 0) {
              image = spotifytrack.album.images.reduce((prev, curr) => {
                return prev.height < curr.height ? prev : curr;
              });
            }

            return {
              id: spotifytrack.id,
              name: spotifytrack.name,
              duration: spotifytrack.duration,
              album: spotifytrack.album,
              artists: spotifytrack.artists,
              image,
            };
          }
        );

        setSpotifyTracks(spotifyTracks);

        setLoadingSpotifyTracks(false);
      } catch (e: any) {
        // if (e?.response?.status === 404) {
        //   setNotFound(true);
        // }
        setSpotifyTracks([]);
        setLoadingSpotifyTracks(false);
      }
    },
    [setLoadingSpotifyTracks, getSpotifyPlaylistTracks]
  );

  useEffect(() => {
    if (!!id) {
      getSkippedTracks(id);
      getSpotifyTracks(id);
    }
  }, [id, getSkippedTracks, getSpotifyTracks]);

  const onDeleteSubmit = useCallback(
    async (event: React.FormEvent<HTMLFormElement>) => {
      try {
        event.preventDefault();
        setDeleting(true);
        setDeleteError(undefined);
        const trackIds = deleteTracks.map((deleteTrack) => deleteTrack.id);
        await deleteSpotifyPlaylistTracks(id, trackIds);

        setDeleting(false);
        onDeleteClose();

        await getSkippedTracks(id);
        await getSpotifyTracks(id);
      } catch (e: any) {
        setDeleteError(e.response?.data?.message || "Unknown error");
        setDeleting(false);
      }
    },
    [
      id,
      setDeleting,
      setDeleteError,
      deleteTracks,
      deleteSpotifyPlaylistTracks,
      onDeleteClose,
      getSkippedTracks,
      getSpotifyTracks,
    ]
  );

  return {
    loadingData,
    loadingSkippedTracks,
    skippedTrackColumns,
    skippedTracks,
    loadingSkippedTrackHistory,
    skippedTrackHistoryColumns,
    skippedTrackHistory,
    loadingSpotifyTracks,
    spotifyTrackColumns,
    spotifyTracks,
    deleteTracks,
    deleteOpen,
    onDeleteClose,
    deleting,
    onDeleteSubmit,
    deleteError,
  };
};
