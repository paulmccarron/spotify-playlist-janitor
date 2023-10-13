import { render } from "@testing-library/react";

import { AppHeader } from "..";

describe("<AppHeader />", () => {
  let container: HTMLElement;

  beforeEach(() => {
    ({ container } = render(<AppHeader />));
  });

  it("should render AppHeader component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});
