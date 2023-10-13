import React from "react";
import {
  fireEvent,
  getByTestId,
  getByText,
  render,
} from "@testing-library/react";

import { Tabs, TabList, Tab, TabPanel } from "../tabs";

describe("<Tabs />", () => {
  let container: HTMLElement;

  beforeEach(() => {
    ({ container } = render(
      <Tabs>
        <TabList>
          <Tab data-testid="tab-1">Tab 1</Tab>
          <Tab data-testid="tab-2">Tab 2</Tab>
          <Tab data-testid="tab-3">Longer Tab Title 3</Tab>
        </TabList>

        <TabPanel>
          <div data-testid="tab-1-content">Tab 1 content</div>
        </TabPanel>
        <TabPanel>
          <div data-testid="tab-2-content">Tab 2 content</div>
        </TabPanel>
        <TabPanel>
          <div data-testid="tab-3-content">Tab 3 content</div>
        </TabPanel>
      </Tabs>
    ));
  });

  it("should render Tabs component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should default to first tab", () => {
    const item1 = getByTestId(container, "tab-1-content");

    expect(item1.innerHTML).toContain("Tab 1 content");
  });

  it("should change Tab when clicked", () => {
    const tab = getByTestId(container, "tab-2");
    fireEvent.click(tab);

    const item1 = getByTestId(container, "tab-2-content");

    expect(item1.innerHTML).toContain("Tab 2 content");
  });
});
