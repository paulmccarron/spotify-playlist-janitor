import React from "react";
import { fireEvent, getByText, render } from "@testing-library/react";

import { Modal } from "../modal";

describe("<Modal />", () => {
  let baseElement: HTMLElement;
  let props: any;
  
  beforeEach(() => {
    props = {
      label: "Modal",
      isOpen: true,
      onClose: jest.fn(),
    };

    ({ baseElement } = render(
      <Modal {...props}><div>Modal Content</div></Modal>
    ));
  });

  it("should render Modal component", () => {
    expect(baseElement.firstChild).toMatchSnapshot();
  });
});
