import React from "react";
import { fireEvent, getByText, render } from "@testing-library/react";

import { Menu, MenuItem } from "../menu";

describe("<Menu />", () => {
  let container: HTMLElement;
  const mockOnClick1 = jest.fn();
  const mockOnClick2 = jest.fn();

  beforeEach(() => {
    ({ container } = render(
      <Menu menuButton={<div>Menu Button</div>}>
        <MenuItem>
          <button onClick={mockOnClick1}>Item 1</button>
        </MenuItem>
        <MenuItem>
          <button onClick={mockOnClick2}>Item 2</button>
        </MenuItem>
      </Menu>
    ));
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("should render Menu component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should expand Menu component when clicked", () => {
    const menuButton = getByText(container, "Menu Button");
    fireEvent.click(menuButton);

    const item1 = getByText(container, "Item 1");
    const item2 = getByText(container, "Item 2");

    expect(item1.parentElement?.className).toContain("menu__item");
    expect(item2.parentElement?.className).toContain("menu__item");
  });

  it("should expand Menu component when clicked", () => {
    const menuButton = getByText(container, "Menu Button");
    fireEvent.click(menuButton);

    const item1 = getByText(container, "Item 1");
    const item2 = getByText(container, "Item 2");
    
    fireEvent.click(item1);
    fireEvent.click(item2);

    expect(mockOnClick1).toHaveBeenCalled();
    expect(mockOnClick2).toHaveBeenCalled();
  });
});
