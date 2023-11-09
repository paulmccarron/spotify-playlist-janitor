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

  const skippedTracksTable = useMemo(
    () => (
      <>
        {(loading || loadingSkippedTracks) && (
          <Table
            data-testid="skipped-tracks-table-loading"
            data={loadingData}
            columns={skippedTrackColumns}
            loading
          />
        )}
        {!loading && !loadingSkippedTracks && skippedTracks.length > 0 && (
          <Table
            data-testid="skipped-tracks-table"
            data={skippedTracks}
            columns={skippedTrackColumns}
          />
        )}
        {!loading && !loadingSkippedTracks && skippedTracks.length === 0 && (
          <div data-testid="skipped-tracks-not-found">
            No skipped tracks found.
          </div>
        )}
      </>
    ),
    [
      loading,
      loadingSkippedTracks,
      loadingData,
      skippedTrackColumns,
      skippedTracks,
    ]
  );

  const skippedTrackHistoryTable = useMemo(
    () => (
      <>
        {(loading || loadingSkippedTrackHistory) && (
          <Table
            data-testid="skipped-track-history-table-loading"
            data={loadingData}
            columns={skippedTrackHistoryColumns}
            loading
          />
        )}
        {!loading &&
          !loadingSkippedTrackHistory &&
          skippedTrackHistory.length > 0 && (
            <Table
              data-testid="skipped-track-history-table"
              data={skippedTrackHistory}
              columns={skippedTrackHistoryColumns}
            />
          )}
        {!loading &&
          !loadingSkippedTrackHistory &&
          skippedTrackHistory.length === 0 && (
            <div data-testid="skipped-track-history-not-found">
              No skipped tracks found.
            </div>
          )}
      </>
    ),
    [
      loading,
      loadingSkippedTrackHistory,
      loadingData,
      skippedTrackHistoryColumns,
      skippedTrackHistory,
    ]
  );

  const spotifyTracksTable = useMemo(
    () => (
      <>
        {(loading || loadingSpotifyTracks) && (
          <Table
            data-testid="tracks-table-loading"
            data={loadingData}
            columns={spotifyTrackColumns}
            loading
          />
        )}
        {!loading && !loadingSpotifyTracks && spotifyTracks.length > 0 && (
          <Table
            data-testid="tracks-table"
            data={spotifyTracks}
            columns={spotifyTrackColumns}
          />
        )}
        {!loading && !loadingSpotifyTracks && spotifyTracks.length === 0 && (
          <div data-testid="tracks-not-found">No Spotify tracks found.</div>
        )}
      </>
    ),
    [
      loading,
      loadingSpotifyTracks,
      loadingData,
      spotifyTrackColumns,
      spotifyTracks,
    ]
  );

  return (
    <>
      <Tabs>
        <TabList>
          <Tab data-testid="skipped-tracks-tab">Skipped Tracks</Tab>
          <Tab data-testid="skipped-track-history-tab">
            Skipped Track History
          </Tab>
          <Tab data-testid="tracks-tab">Tracks</Tab>
        </TabList>

        <TabPanel data-testid="skipped-tracks-tab-content">
          {skippedTracksTable}
        </TabPanel>
        <TabPanel data-testid="skipped-track-history-tab-content">
          {skippedTrackHistoryTable}
        </TabPanel>
        <TabPanel data-testid="tracks-tab-content">
          {spotifyTracksTable}
        </TabPanel>
      </Tabs>
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
