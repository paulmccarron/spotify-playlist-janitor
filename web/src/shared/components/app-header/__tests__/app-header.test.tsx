import React from "react";
import { render } from "@testing-library/react";

import { AppHeader } from "../app-header";

describe("<AppHeader />", () => {
  let container: HTMLElement;

  beforeEach(() => {
    ({ container } = render(<AppHeader />));
  });

  it("should render AppHeader component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});
