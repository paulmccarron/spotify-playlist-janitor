import { act, fireEvent, getByTestId, render } from "@testing-library/react";

import { TextInput } from "..";

describe("<TextInput />", () => {
  let container: HTMLElement;
  let props: any;
  let mockOnChange = jest.fn();

  beforeEach(() => {
    props = {
      label: "Test Input",
      placeholder: "Enter text...",
      id: "text-input-test",
      "data-testid": "text-input-test",
      value: "",
      onChange: mockOnChange,
    };
    ({ container } = render(<TextInput {...props} />));
  });

  it("should render TextInput component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render TextInput component with value", () => {
    ({ container } = render(
      <TextInput
        {...{
          ...props,
          value: "Text Value",
        }}
      />
    ));
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render TextInput component as password input", () => {
    ({ container } = render(
      <TextInput
        {...{
          ...props,
          type: "password",
        }}
      />
    ));
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render TextInput component as number input", () => {
    ({ container } = render(
      <TextInput
        {...{
          ...props,
          type: "number",
        }}
      />
    ));
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should execute onChange when text input", async () => {
    const input = getByTestId(container, "text-input-test");

    act(() => {
      fireEvent.change(input, { target: { value: "123" } });
    });

    expect(mockOnChange).toHaveBeenCalled();
  });
});
