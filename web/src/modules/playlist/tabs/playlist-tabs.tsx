import { useMemo } from "react";

import { Tab, TabList, TabPanel, Tabs } from "shared/components/tabs";
import { Table } from "shared/components/table";
import { Modal } from "shared/components/modal";

import { usePlaylistTabsLogic } from "./use-playlist-tabs-logic";
import { DeleteTrackModalView } from "./delete-track-modal";

type PlaylistTabsProps = {
    id: string;
    loading?: boolean;
};

export const PlaylistTabs = ({ id, loading }: PlaylistTabsProps) => {
    const {
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
    } = usePlaylistTabsLogic({ id });

    const tabs = useMemo(
        () => (
            <Tabs>
                <TabList>
                    <Tab data-testid="skipped-tracks-tab">Skipped Tracks</Tab>
                    <Tab data-testid="skipped-track-history-tab">
                        Skipped Track History
                    </Tab>
                    <Tab data-testid="tracks-tab">Tracks</Tab>
                </TabList>

                <TabPanel>
                    {(loading || loadingSkippedTracks) && (
                        <Table data={loadingData} columns={skippedTrackColumns} loading />
                    )}
                    {!loading && !loadingSkippedTracks && skippedTracks.length > 0 && (
                        <Table data={skippedTracks} columns={skippedTrackColumns} />
                    )}
                    {!loading && !loadingSkippedTracks && skippedTracks.length === 0 && (
                        <>No skipped tracks found.</>
                    )}
                </TabPanel>
                <TabPanel>
                    {(loading || loadingSkippedTrackHistory) && (
                        <Table data={loadingData} columns={skippedTrackHistoryColumns} loading />
                    )}
                    {!loading && !loadingSkippedTrackHistory && skippedTrackHistory.length > 0 && (
                        <Table data={skippedTrackHistory} columns={skippedTrackHistoryColumns} />
                    )}
                    {!loading && !loadingSkippedTrackHistory && skippedTrackHistory.length === 0 && (
                        <>No skipped tracks found.</>
                    )}
                </TabPanel>
                <TabPanel>
                    {(loading || loadingSpotifyTracks) && (
                        <Table data={loadingData} columns={spotifyTrackColumns} loading />
                    )}
                    {!loading && !loadingSpotifyTracks && spotifyTracks.length > 0 && (
                        <Table data={spotifyTracks} columns={spotifyTrackColumns} />
                    )}
                    {!loading && !loadingSpotifyTracks && spotifyTracks.length === 0 && (
                        <>No Spotify tracks found.</>
                    )}
                </TabPanel>
            </Tabs>
        ),
        [
            loading,
            loadingSkippedTracks,
            skippedTrackColumns,
            skippedTracks,
            loadingSkippedTrackHistory,
            skippedTrackHistoryColumns,
            skippedTrackHistory,
            loadingSpotifyTracks,
            spotifyTrackColumns,
            spotifyTracks,
        ]
    );

    return (
        <>
            {tabs}
            <Modal
                {...{
                    isOpen: deleteOpen,
                    onClose: onDeleteClose,
                    label: "Delete Playlist Modal",
                }}
            >
                <DeleteTrackModalView
                    {...{
                        onSubmit: onDeleteSubmit,
                        deleteTracks,
                        modalSaving: deleting,
                        modalError: deleteError,
                        onModalClose: onDeleteClose,
                    }}
                />
            </Modal>
        </>
    );
};
