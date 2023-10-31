import { fireEvent, getByTestId, render } from "@testing-library/react";

import { EditPlaylistModalView } from "../edit-playlist-modal";
import { playlist } from "shared/mock-data/playlist";

describe("<EditPlaylistModalView />", () => {
  let container: HTMLElement;
  let props: any = {
    onSubmit: jest.fn(),
    playlist,
    modalSaving: false,
    modalError: undefined,
    onModalClose: jest.fn(),
  };

  beforeEach(async () => {
    ({ container } = render(<EditPlaylistModalView {...props} />));
  });

  it("should render EditPlaylistModalView component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render EditPlaylistModalView component with an error", () => {
    props = {
      ...props,
      modalError: "MODAL_ERROR",
    };

    ({ container } = render(<EditPlaylistModalView {...props} />));

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
