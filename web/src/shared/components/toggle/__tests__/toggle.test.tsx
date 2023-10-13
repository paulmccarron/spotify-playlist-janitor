import { fireEvent, getByTestId, render } from "@testing-library/react";

import { Toggle } from "..";

describe("<Toggle />", () => {
  let container: HTMLElement;
  let props: any;
  let mockOnChange = jest.fn();

  beforeEach(() => {
    props = {
      label: "Toggle Example",
      onChange: mockOnChange,
      checked: false,
      id: "test-toggle",
      "data-testid": "test-toggle",
    };
    ({ container } = render(<Toggle {...props} />));
  });

  it("should render Toggle component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render Toggle component toggled", () => {
    ({ container } = render(<Toggle {...{ ...props, checked: true }} />));
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should fire onChange when clicked", () => {
    const input = getByTestId(container, "test-toggle");
    fireEvent.click(input);
    expect(mockOnChange).toHaveBeenCalled();
  });
});
