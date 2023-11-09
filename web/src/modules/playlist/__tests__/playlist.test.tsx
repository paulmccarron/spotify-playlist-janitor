import {
  fireEvent,
  getByTestId,
  queryByTestId,
  render,
  waitFor,
} from "@testing-library/react";

import { Playlist } from "../playlist";
import { usePlaylistLogic } from "../use-playlist-logic";
import {
  playlist
} from "shared/mock-data/playlist";

jest.mock("../use-playlist-logic");
jest.mock("../modal");
jest.mock("../tabs");

describe("<Playlist />", () => {
  let container: HTMLElement;
  let mockUsePlaylistLogic: any;

  beforeEach(async () => {

    mockUsePlaylistLogic = {
      playlist,
      loading: false,
      notFound: false,
      editOpen: false,
      onEditOpen: jest.fn(),
      onEditClose: jest.fn(),
      onEditSubmit: jest.fn(),
      editError: "",
      editSaving: false,
      showSpotifyAuthModal: false,
    };

    jest.mocked(usePlaylistLogic).mockImplementation(() => mockUsePlaylistLogic);

    await waitFor(() => {
      ({ container } = render(<Playlist {...{id: playlist.id}}/>));
    });
  });

  it("should render Playlist component", () => {
    expect(container.firstChild).toMatchSnapshot();

    expect(container.getElementsByClassName("react-loading-skeleton").length).toBe(0)
  });

  it("should render Playlist component in loading state", async () => {
    mockUsePlaylistLogic = {
      ...mockUsePlaylistLogic,
      playlist: undefined,
      loading: true,
    };
    jest.mocked(usePlaylistLogic).mockImplementation(() => mockUsePlaylistLogic);

    await waitFor(() => {
      ({ container } = render(<Playlist {...{id: playlist.id}}/>));
    });

    expect(container.firstChild).toMatchSnapshot();

    expect(container.getElementsByClassName("react-loading-skeleton").length).toBe(5)
  });

  it("should render Home component with Not Found", async () => {
    mockUsePlaylistLogic = {
      ...mockUsePlaylistLogic,
      notFound: true,
    };
    jest.mocked(usePlaylistLogic).mockImplementation(() => mockUsePlaylistLogic);

    await waitFor(() => {
      ({ container } = render(<Playlist {...{id: playlist.id}}/>));
    });

    expect(container.firstChild).toMatchSnapshot();

    expect(getByTestId(container, "not-found")).toBeTruthy();
  });
});
