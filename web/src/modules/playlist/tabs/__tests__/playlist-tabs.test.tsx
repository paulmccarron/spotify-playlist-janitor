import {
  fireEvent,
  getByTestId,
  render,
  renderHook,
  waitFor,
} from "@testing-library/react";

import { PlaylistTabs } from "../playlist-tabs";
import { usePlaylistTabsLogic } from "../use-playlist-tabs-logic";
import {
  playlist,
  skippedTrackTotals,
  skippedTrackHistory,
  spotifyTracks,
} from "shared/mock-data/playlist";
import { usePlaylistTabsColumns } from "../use-playlist-tabs-columns";
import { RefObject } from "react";

jest.mock("../use-playlist-tabs-logic");

describe("<PlaylistTabs />", () => {
  let columns: RefObject<ReturnType<typeof usePlaylistTabsColumns>>;
  let container: HTMLElement;
  let mockUsePlaylistTabsLogic: any;
  const mockOnDeleteClick = jest.fn();

  ({ result: columns } = renderHook(() =>
    usePlaylistTabsColumns({ onDeleteClick: mockOnDeleteClick })
  ));

  beforeEach(async () => {
    mockUsePlaylistTabsLogic = {
      loadingData: [],
      loadingSkippedTracks: false,
      skippedTrackColumns: columns.current?.skippedTrackColumns,
      skippedTracks: skippedTrackTotals,
      loadingSkippedTrackHistory: false,
      skippedTrackHistoryColumns: columns.current?.skippedTrackHistoryColumns,
      skippedTrackHistory: skippedTrackHistory,
      loadingSpotifyTracks: false,
      spotifyTrackColumns: columns.current?.spotifyTrackColumns,
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
  });

  it("should render PlaylistTabs component in loading state", async () => {
    await waitFor(() => {
      ({ container } = render(
        <PlaylistTabs {...{ id: playlist.id, loading: true }} />
      ));
    });

    expect(container.firstChild).toMatchSnapshot();
  });

  it("should display Skipped Tracks tab default", () => {
    const skippedTracksTab = getByTestId(container, "skipped-tracks-tab");
    const skippedTracksContent = getByTestId(
      container,
      "skipped-tracks-tab-content"
    );

    expect(skippedTracksTab.className).toContain("selected");
    expect(skippedTracksContent.className).toContain("selected");
    expect(getByTestId(container, "skipped-tracks-table")).not.toBeNull();
  });

  it("should display Skipped Tracks in loading state", async () => {
    mockUsePlaylistTabsLogic = {
      ...mockUsePlaylistTabsLogic,
      loadingSkippedTracks: true,
    };

    jest
      .mocked(usePlaylistTabsLogic)
      .mockImplementation(() => mockUsePlaylistTabsLogic);

    await waitFor(() => {
      ({ container } = render(
        <PlaylistTabs {...{ id: playlist.id, loading: false }} />
      ));
    });
    expect(
      getByTestId(container, "skipped-tracks-table-loading")
    ).not.toBeNull();
  });

  it("should display Skipped Tracks not found state", async () => {
    mockUsePlaylistTabsLogic = {
      ...mockUsePlaylistTabsLogic,
      skippedTracks: [],
    };

    jest
      .mocked(usePlaylistTabsLogic)
      .mockImplementation(() => mockUsePlaylistTabsLogic);

    await waitFor(() => {
      ({ container } = render(
        <PlaylistTabs {...{ id: playlist.id, loading: false }} />
      ));
    });
    expect(getByTestId(container, "skipped-tracks-not-found")).not.toBeNull();
  });

  it("should display Skipped Track History tab when clicked", () => {
    const skippedTrackHistoryTab = getByTestId(
      container,
      "skipped-track-history-tab"
    );
    const skippedTrackHistoryContent = getByTestId(
      container,
      "skipped-track-history-tab-content"
    );

    fireEvent.click(skippedTrackHistoryTab);

    expect(skippedTrackHistoryTab.className).toContain("selected");
    expect(skippedTrackHistoryContent.className).toContain("selected");
    expect(
      getByTestId(container, "skipped-track-history-table")
    ).not.toBeNull();
  });

  it("should display Skipped Track History in loading state", async () => {
    mockUsePlaylistTabsLogic = {
      ...mockUsePlaylistTabsLogic,
      loadingSkippedTrackHistory: true,
    };

    jest
      .mocked(usePlaylistTabsLogic)
      .mockImplementation(() => mockUsePlaylistTabsLogic);

    await waitFor(() => {
      ({ container } = render(
        <PlaylistTabs {...{ id: playlist.id, loading: false }} />
      ));
    });
    const skippedTrackHistoryTab = getByTestId(
      container,
      "skipped-track-history-tab"
    );

    fireEvent.click(skippedTrackHistoryTab);

    expect(
      getByTestId(container, "skipped-track-history-table-loading")
    ).not.toBeNull();
  });

  it("should display Skipped Track History not found state", async () => {
    mockUsePlaylistTabsLogic = {
      ...mockUsePlaylistTabsLogic,
      skippedTrackHistory: [],
    };

    jest
      .mocked(usePlaylistTabsLogic)
      .mockImplementation(() => mockUsePlaylistTabsLogic);

    await waitFor(() => {
      ({ container } = render(
        <PlaylistTabs {...{ id: playlist.id, loading: false }} />
      ));
    });
    const skippedTrackHistoryTab = getByTestId(
      container,
      "skipped-track-history-tab"
    );

    fireEvent.click(skippedTrackHistoryTab);

    expect(
      getByTestId(container, "skipped-track-history-not-found")
    ).not.toBeNull();
  });

  it("should display Tracks tab when clicked", () => {
    const tracksTab = getByTestId(container, "tracks-tab");
    const tracksContent = getByTestId(container, "tracks-tab-content");

    fireEvent.click(tracksTab);

    expect(tracksTab.className).toContain("selected");
    expect(tracksContent.className).toContain("selected");
    expect(getByTestId(container, "tracks-table")).not.toBeNull();
  });

  it("should display Tracks in loading state", async () => {
    mockUsePlaylistTabsLogic = {
      ...mockUsePlaylistTabsLogic,
      loadingSpotifyTracks: true,
    };

    jest
      .mocked(usePlaylistTabsLogic)
      .mockImplementation(() => mockUsePlaylistTabsLogic);

    await waitFor(() => {
      ({ container } = render(
        <PlaylistTabs {...{ id: playlist.id, loading: false }} />
      ));
    });
    const tracksTab = getByTestId(container, "tracks-tab");

    fireEvent.click(tracksTab);

    expect(getByTestId(container, "tracks-table-loading")).not.toBeNull();
  });

  it("should display Tracks not found state", async () => {
    mockUsePlaylistTabsLogic = {
      ...mockUsePlaylistTabsLogic,
      spotifyTracks: [],
    };

    jest
      .mocked(usePlaylistTabsLogic)
      .mockImplementation(() => mockUsePlaylistTabsLogic);

    await waitFor(() => {
      ({ container } = render(
        <PlaylistTabs {...{ id: playlist.id, loading: false }} />
      ));
    });
    const tracksTab = getByTestId(container, "tracks-tab");

    fireEvent.click(tracksTab);

    expect(getByTestId(container, "tracks-not-found")).not.toBeNull();
  });
});
