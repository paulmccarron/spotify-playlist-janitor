import { fireEvent, getByTestId, render } from "@testing-library/react";

import { DeleteTrackModalView } from "../delete-track-modal";
import { unmonitoredPlaylists } from "shared/mock-data/home";

describe("<DeleteTrackModalView />", () => {
  let container: HTMLElement;
  let props: any = {
    onSubmit: jest.fn(),
    deleteTracks: [{ id: "track_id", name: "Track Name" }],
    modalSaving: false,
    modalError: undefined,
    onModalClose: jest.fn(),
  };

  beforeEach(async () => {
    ({ container } = render(<DeleteTrackModalView {...props} />));
  });

  it("should render DeleteTrackModalView component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render DeleteTrackModalView component with an error", () => {
    props = {
      ...props,
      modalError: "MODAL_ERROR",
    };

    ({ container } = render(<DeleteTrackModalView {...props} />));

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
