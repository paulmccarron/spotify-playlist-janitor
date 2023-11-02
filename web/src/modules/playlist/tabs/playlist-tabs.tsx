import { Tab, TabList, TabPanel, Tabs } from "shared/components/tabs";
import { usePlaylistTabsLogic } from "./use-playlist-tabs-logic";
import { Table } from "shared/components/table";
import { useMemo } from "react";
import { Modal } from "shared/components/modal";
import { DeleteTrackModalView } from "./delete-track-modal";

type PlaylistTabsProps = {
  id: string;
};

export const PlaylistTabs = ({ id }: PlaylistTabsProps) => {
  const {
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
          {loadingSkippedTracks && <>Loading...</>}
          {!loadingSkippedTracks && skippedTracks.length > 0 && (
            <Table data={skippedTracks} columns={skippedTrackColumns} />
          )}
          {!loadingSkippedTracks && skippedTracks.length === 0 && (
            <>No skipped tracks found.</>
          )}
        </TabPanel>
        <TabPanel>
          {loadingSkippedTrackHistory && <>Loading...</>}
          {!loadingSkippedTrackHistory && skippedTrackHistory.length > 0 && (
            <Table
              data={skippedTrackHistory}
              columns={skippedTrackHistoryColumns}
            />
          )}
          {!loadingSkippedTrackHistory && skippedTrackHistory.length === 0 && (
            <>No skipped tracks found.</>
          )}
        </TabPanel>
        <TabPanel>
          {loadingSpotifyTracks && <>Loading...</>}
          {!loadingSpotifyTracks && spotifyTracks.length > 0 && (
            <Table data={spotifyTracks} columns={spotifyTrackColumns} />
          )}
          {!loadingSpotifyTracks && spotifyTracks.length === 0 && (
            <>No Spotify tracks found.</>
          )}
        </TabPanel>
      </Tabs>
    ),
    [
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
