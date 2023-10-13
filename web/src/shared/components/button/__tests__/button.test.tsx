import React from "react";
import { fireEvent, getByTestId, render } from "@testing-library/react";

import { PrimaryButton, SecondaryButton } from "../button";

describe("<PrimaryButton />", () => {
  let container: HTMLElement;
  let props: any;
  const mockOnClick = jest.fn();

  beforeEach(() => {
    props = {
      onClick: mockOnClick,
      id: "primary-button-id",
      "data-testid": "test-primary-button-id",
    };

    ({ container } = render(
      <PrimaryButton {...props}>Test Primary Button</PrimaryButton>
    ));
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("should render PrimaryButton component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should execute onClick when PrimaryButton clicked", () => {
    const button = getByTestId(container, "test-primary-button-id");
    fireEvent.click(button);
    expect(mockOnClick).toHaveBeenCalled();
  });
});

describe("<SecondaryButton />", () => {
  let container: HTMLElement;
  let props: any;
  const mockOnClick = jest.fn();

  beforeEach(() => {
    props = {
      onClick: mockOnClick,
      id: "secondary-button-id",
      "data-testid": "test-secondary-button-id",
    };

    ({ container } = render(
      <SecondaryButton {...props}>Test Secondary Button</SecondaryButton>
    ));
  });

  it("should render SecondaryButton component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should execute onClick when SecondaryButton clicked", () => {
    const button = getByTestId(container, "test-secondary-button-id");
    fireEvent.click(button);
    expect(mockOnClick).toHaveBeenCalled();
  });
});
