import { fireEvent, getByTestId, render } from "@testing-library/react";

import { AddPlaylistModalView } from "../edit-playlist-modal";
import { unmonitoredPlaylists } from "shared/mock-data/home";

describe("<AddPlaylistModalView />", () => {
  let container: HTMLElement;
  let props: any = {
    onSubmit: jest.fn(),
    unmonitoredPlaylists,
    onPlaylistChange: jest.fn(),
    modalSaving: false,
    modalError: undefined,
    onModalClose: jest.fn(),
  };

  beforeEach(async () => {
    ({ container } = render(<AddPlaylistModalView {...props} />));
  });

  it("should render AddPlaylistModalView component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render AddPlaylistModalView component with an error", () => {
    props = {
      ...props,
      modalError: "MODAL_ERROR",
    };

    ({ container } = render(<AddPlaylistModalView {...props} />));

    expect(container.firstChild).toMatchSnapshot();
  });

  it("should call onSubmit function when Confirm button clicked", () => {
    const confirm = getByTestId(container, "confirm-modal-button");
    fireEvent.click(confirm);

    expect(props.onSubmit).toHaveBeenCalled();
  });

  it("should call onModalClose function when Cancel button clicked", () => {
    const cancel = getByTestId(container, "cancel-modal-button");
    fireEvent.click(cancel);

    expect(props.onModalClose).toHaveBeenCalled();
  });
});
