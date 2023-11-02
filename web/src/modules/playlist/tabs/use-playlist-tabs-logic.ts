import { useCallback, useEffect, useState } from "react";
import { useDataApi } from "api/data-api";
import { SkippedTrackHistory, SkippedTrackTotal } from "./playlist-tabs-types";

type UsePlaylistTabsLogicProps = {
    id: string;
};

export const usePlaylistTabsLogic = ({ id }: UsePlaylistTabsLogicProps) => {
    const {
        getDatabasePlaylistSkippedTracks
    } = useDataApi();

    const [loadingSkippedTracks, setLoadingSkippedTracks] = useState(false);
    const [skippedTracks, setSkippedTracks] = useState<SkippedTrackTotal[]>([]);

    const [loadingSkippedTrackHistory, setLoadingSkippedTrackHistory] = useState(false);
    const [skippedTrackHistory, setSkippedTrackHistory] = useState<SkippedTrackHistory[]>([]);

    const getSkippedTracks = useCallback(
        async (id: string) => {
            try {
                setLoadingSkippedTracks(true);
                setLoadingSkippedTrackHistory(true);
                setSkippedTracks([]);
                setSkippedTrackHistory([]);

                const skippedTrackResponse = await getDatabasePlaylistSkippedTracks(id);

                const skippedTracks: SkippedTrackTotal[] = [];
                var skippedTracksTotals: { [id: string]: SkippedTrackTotal; } = {};

                skippedTrackResponse.data.forEach(skippedTrack => {
                    if (skippedTracksTotals[skippedTrack.trackId]) {
                        skippedTracksTotals[skippedTrack.trackId].skippedTotal++
                    }
                    else {
                        skippedTracksTotals[skippedTrack.trackId] = {
                            id: skippedTrack.trackId,
                            name: skippedTrack.name,
                            duration: skippedTrack.duration,
                            skippedTotal: 1,
                            album: skippedTrack.album,
                            artists: skippedTrack.artists,
                            image: skippedTrack.album.images.reduce((prev, curr) => {
                                return prev.height < curr.height ? prev : curr;
                            })
                        }
                    }
                })

                for (let key in skippedTracksTotals) {
                    skippedTracks.push(skippedTracksTotals[key]);
                }

                const skippedTrackHistory: SkippedTrackHistory[] = skippedTrackResponse.data
                    .map(skippedTrack => ({
                        id: skippedTrack.trackId,
                        name: skippedTrack.name,
                        duration: skippedTrack.duration,
                        skippedDate: new Date(skippedTrack.skippedDate),
                        album: skippedTrack.album,
                        artists: skippedTrack.artists,
                        image: skippedTrack.album.images.reduce((prev, curr) => {
                            return prev.height < curr.height ? prev : curr;
                        })
                    }));


                setSkippedTracks(skippedTracks);
                setSkippedTrackHistory(skippedTrackHistory);

                setLoadingSkippedTracks(false);
                setLoadingSkippedTrackHistory(false);
            } catch (e: any) {
                // if (e?.response?.status === 404) {
                //   setNotFound(true);
                // }
                setLoadingSkippedTracks(false);
                setLoadingSkippedTrackHistory(false);
            }
        },
        [setLoadingSkippedTracks, setLoadingSkippedTrackHistory, getDatabasePlaylistSkippedTracks]
    );

    useEffect(() => {
      if (!!id) {
        getSkippedTracks(id);
      }
    }, [getSkippedTracks, id]);

    return {
        loadingSkippedTracks,
        skippedTracks,
        loadingSkippedTrackHistory,
        skippedTrackHistory,
    };
}