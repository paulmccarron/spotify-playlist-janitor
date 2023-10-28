import {
  fireEvent,
  getByTestId,
  render,
  waitFor,
} from "@testing-library/react";

import { Home } from "../home";
import { useHomeLogic } from "../use-home-logic";
import {
  monitoredPlaylists,
  unmonitoredPlaylists,
} from "shared/mock-data/home";

jest.mock("../use-home-logic");
jest.mock("../modal");

describe("<Home />", () => {
  let container: HTMLElement;
  let mockUseHomeLogic: any;

  beforeEach(async () => {

    mockUseHomeLogic = {
      monitoredPlaylists: monitoredPlaylists,
      unmonitoredPlaylists: unmonitoredPlaylists,
      loading: false,
      loadingSkeletons: undefined,
      onPlaylistClick: jest.fn(),
      modalOpen: false,
      onModalOpen: jest.fn(),
      onModalClose: jest.fn(),
      onSubmit: jest.fn(),
      onPlaylistChange: jest.fn(),
      modalError: "",
      modalSaving: false,
      showSpotifyAuthModal: false,
    };

    jest.mocked(useHomeLogic).mockImplementation(() => mockUseHomeLogic);

    await waitFor(() => {
      ({ container } = render(<Home />));
    });
  });

  it("should render Home component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render Home component with empty page", async () => {
    mockUseHomeLogic = {
      ...mockUseHomeLogic,
      loading: true,
    };
    jest.mocked(useHomeLogic).mockImplementation(() => mockUseHomeLogic);

    await waitFor(() => {
      ({ container } = render(<Home />));
    });
    expect(container.firstChild).toMatchSnapshot();

    expect(getByTestId(container, "empty-loading")).toBeTruthy();
  });

  it("should render Home component with loading skeletons", async () => {
    mockUseHomeLogic = {
      ...mockUseHomeLogic,
      loading: true,
      loadingSkeletons: [0, 1, 2, 3, 4]
    };
    jest.mocked(useHomeLogic).mockImplementation(() => mockUseHomeLogic);

    await waitFor(() => {
      ({ container } = render(<Home />));
    });
    expect(container.firstChild).toMatchSnapshot();

    mockUseHomeLogic.loadingSkeletons.forEach((loadingSkeleton: number) => {
      expect(getByTestId(container, `playlist-item-skeleton-${loadingSkeleton}`)).toBeTruthy();
    });
  });

  it("should render Home component with no monitored playlists", async () => {
    mockUseHomeLogic = {
      ...mockUseHomeLogic,
      monitoredPlaylists: [],
    };
    jest.mocked(useHomeLogic).mockImplementation(() => mockUseHomeLogic);

    await waitFor(() => {
      ({ container } = render(<Home />));
    });
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should call onPlaylistClick function when Playlist clicked", () => {
    const playlistItem = getByTestId(container, "playlist-item-0");
    fireEvent.click(playlistItem);

    expect(mockUseHomeLogic.onPlaylistClick).toHaveBeenCalledWith(monitoredPlaylists[0].id);
  });

  it("should call onModalOpen function when Add button clicked", () => {
    const addPlaylist = getByTestId(container, "add-playlist-item");
    fireEvent.click(addPlaylist);

    expect(mockUseHomeLogic.onModalOpen).toHaveBeenCalled();
  });
});
