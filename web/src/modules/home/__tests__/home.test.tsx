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

  const mockUseHomeLogic = {
    monitoredPlaylists: monitoredPlaylists,
    unmonitoredPlaylists: unmonitoredPlaylists,
    loading: false,
    modalOpen: false,
    onModalOpen: jest.fn(),
    onModalClose: jest.fn(),
    onSubmit: jest.fn(),
    onPlaylistChange: jest.fn(),
    modalError: "",
    modalSaving: false,
    showSpotifyAuthModal: false,
  };

  beforeEach(async () => {
    jest.mocked(useHomeLogic).mockImplementation(() => mockUseHomeLogic);

    await waitFor(() => {
      ({ container } = render(<Home />));
    });
  });

  it("should render Home component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render Home component with no monitored playlists", async () => {
    const mockUseHomeLogic2 = {
      ...mockUseHomeLogic,
      monitoredPlaylists: [],
    };
    jest.mocked(useHomeLogic).mockImplementation(() => mockUseHomeLogic2);

    await waitFor(() => {
      ({ container } = render(<Home />));
    });
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should call onModalOpen function when Add button clicked", () => {
    const addPlaylist = getByTestId(container, "add-playlist-item");
    fireEvent.click(addPlaylist);

    expect(mockUseHomeLogic.onModalOpen).toHaveBeenCalled();
  });
});
