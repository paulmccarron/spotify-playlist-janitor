import {
  fireEvent,
  getByLabelText,
  getByText,
  render,
  waitFor,
} from "@testing-library/react";
import { selectOptions } from "shared/mock-data/select-options";

import { Select } from "..";

describe("<Select />", () => {
  let container: HTMLElement;
  let props: any;
  const mockOnChange = jest.fn();
  const label = "Select Label";
  const KEY_DOWN = 40;

  beforeEach(() => {
    props = {
      value: undefined,
      label: label,
      placeholder: "Select option...",
      options: selectOptions,
      onChange: mockOnChange,
    };
    ({ container } = render(<Select {...props} />));
  });

  it("should render Select component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should expand Select component when clicked", async () => {
    fireEvent.focus(getByLabelText(container, label));
    fireEvent.keyDown(getByLabelText(container, label), {
      keyCode: KEY_DOWN,
    });

    const option1 = await waitFor(() => getByText(container, selectOptions[0].label));
    const option2 = await waitFor(() => getByText(container, selectOptions[1].label));
    const option3 = await waitFor(() => getByText(container, selectOptions[2].label));

    expect(option1.className).toContain("select__option");
    expect(option2.className).toContain("select__option");
    expect(option3.className).toContain("select__option");
  });
});
