import { Tab, TabList, TabPanel, Tabs } from "shared/components/tabs";

type PlaylistTabsProps = {
    id: string;
};

export const PlaylistTabs = ({ id }: PlaylistTabsProps) => {

    return <Tabs>
        <TabList>
            <Tab data-testid="skipped-tracks-tab">Skipped Tracks</Tab>
            <Tab data-testid="skipped-track-history-tab">Skipped Track History</Tab>
            <Tab data-testid="tracks-tab">Tracks</Tab>
        </TabList>

        <TabPanel>
            <>Skipped Tracks</>
        </TabPanel>
        <TabPanel>
            <>Skipped Track History</>
        </TabPanel>
        <TabPanel>
            <>Tracks</>
        </TabPanel>
    </Tabs>
}