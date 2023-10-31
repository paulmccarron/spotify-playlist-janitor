import { fireEvent, getByTestId, render } from "@testing-library/react";

import { DeletePlaylistModalView } from "../delete-playlist-modal";
import { playlist } from "shared/mock-data/playlist";

describe("<DeletePlaylistModalView />", () => {
  let container: HTMLElement;
  let props: any = {
    onSubmit: jest.fn(),
    playlist,
    modalSaving: false,
    modalError: undefined,
    onModalClose: jest.fn(),
  };

  beforeEach(async () => {
    ({ container } = render(<DeletePlaylistModalView {...props} />));
  });

  it("should render DeletePlaylistModalView component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render DeletePlaylistModalView component with an error", () => {
    props = {
      ...props,
      modalError: "MODAL_ERROR",
    };

    ({ container } = render(<DeletePlaylistModalView {...props} />));

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
