import {
  fireEvent,
  getByTestId,
  queryByTestId,
  render,
  waitFor,
} from "@testing-library/react";

import { PlaylistTabs } from "../playlist-tabs";
import { usePlaylistTabsLogic } from "../use-playlist-tabs-logic";
import {
  playlist,
  skippedTrackTotals,
  skippedTrackHistory,
  spotifyTracks,
  imageColumn,
  albumColumn,
  artistColumn,
  dateSkippedColumn,
  durationColumn,
  getDeleteColumn,
  titleColumn,
  totalSkippedColumn,
} from "shared/mock-data/playlist";
import { usePlaylistTabsColumns } from "../use-playlist-tabs-columns";

jest.mock("../use-playlist-tabs-logic");


describe("<PlaylistTabs />", () => {
  let container: HTMLElement;
  let mockUsePlaylistTabsLogic: any;

  const {
    skippedTrackColumns,
    skippedTrackHistoryColumns,
    spotifyTrackColumns,
  } = usePlaylistTabsColumns({ onDeleteClick: (id: string): void => {} });

  beforeEach(async () => {
    mockUsePlaylistTabsLogic = {
      loadingData: [],
      loadingSkippedTracks: false,
      skippedTrackColumns,
      skippedTracks: skippedTrackTotals,
      loadingSkippedTrackHistory: false,
      skippedTrackHistoryColumns,
      skippedTrackHistory: skippedTrackHistory,
      loadingSpotifyTracks: false,
      spotifyTrackColumns,
      spotifyTracks: spotifyTracks,
      deleteTracks: [],
      deleteOpen: false,
      onDeleteClose: jest.fn(),
      deleting: false,
      onDeleteSubmit: jest.fn(),
      deleteError: undefined,
    };

    jest
      .mocked(usePlaylistTabsLogic)
      .mockImplementation(() => mockUsePlaylistTabsLogic);

    await waitFor(() => {
      ({ container } = render(
        <PlaylistTabs {...{ id: playlist.id, loading: false }} />
      ));
    });
  });

  it("should render PlaylistTabs component", () => {
    expect(container.firstChild).toMatchSnapshot();

    expect(
      container.getElementsByClassName("react-loading-skeleton").length
    ).toBe(0);
  });

  // it("should render Playlist component in loading state", async () => {
  //   mockUsePlaylistLogic = {
  //     ...mockUsePlaylistLogic,
  //     playlist: undefined,
  //     loading: true,
  //   };
  //   jest.mocked(usePlaylistLogic).mockImplementation(() => mockUsePlaylistLogic);

  //   await waitFor(() => {
  //     ({ container } = render(<Playlist {...{id: playlist.id}}/>));
  //   });

  //   expect(container.firstChild).toMatchSnapshot();

  //   expect(container.getElementsByClassName("react-loading-skeleton").length).toBe(5)
  // });

  // it("should render Home component with Not Found", async () => {
  //   mockUsePlaylistLogic = {
  //     ...mockUsePlaylistLogic,
  //     notFound: true,
  //   };
  //   jest.mocked(usePlaylistLogic).mockImplementation(() => mockUsePlaylistLogic);

  //   await waitFor(() => {
  //     ({ container } = render(<Playlist {...{id: playlist.id}}/>));
  //   });

  //   expect(container.firstChild).toMatchSnapshot();

  //   expect(getByTestId(container, "not-found")).toBeTruthy();
  // });
});
