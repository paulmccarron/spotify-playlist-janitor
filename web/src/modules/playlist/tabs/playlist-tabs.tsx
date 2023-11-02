import { Tab, TabList, TabPanel, Tabs } from "shared/components/tabs";
import { usePlaylistTabsLogic } from "./use-playlist-tabs-logic";
import { Table } from "shared/components/table";
import { skippedTrackColumns, skippedTrackHistoryColumns } from "./playlist-tabs-columns";
import { useMemo } from "react";

type PlaylistTabsProps = {
    id: string;
};

export const PlaylistTabs = ({ id }: PlaylistTabsProps) => {

    const { skippedTracks, skippedTrackHistory, loadingSkippedTracks, loadingSkippedTrackHistory } = usePlaylistTabsLogic({ id });

    const tabs = useMemo(() => <Tabs>
        <TabList>
            <Tab data-testid="skipped-tracks-tab">Skipped Tracks</Tab>
            <Tab data-testid="skipped-track-history-tab">Skipped Track History</Tab>
            <Tab data-testid="tracks-tab">Tracks</Tab>
        </TabList>

        <TabPanel>
            {!loadingSkippedTracks &&
                <Table
                    data={skippedTracks}
                    columns={skippedTrackColumns}
                />
            }
        </TabPanel>
        <TabPanel>
            {!loadingSkippedTrackHistory &&
                <Table
                    data={skippedTrackHistory}
                    columns={skippedTrackHistoryColumns}
                />
            }
        </TabPanel>
        <TabPanel>
            <>Tracks</>
        </TabPanel>
    </Tabs>, [loadingSkippedTracks, skippedTracks, loadingSkippedTrackHistory, skippedTrackHistory])

    return tabs;
}