import {
  act,
  fireEvent,
  getByTestId,
  getByText,
  render,
  waitFor,
} from "@testing-library/react";

import { Tooltip } from "..";

describe("<Tooltip />", () => {
  let container: HTMLElement;
  let props: any;

  beforeEach(() => {
    props = {
      content: "Tooltip Content",
      dataTooltipId: "test-tooltip",
      children: <div data-testid="tooltip-child-test-id">Tooltip Child</div>,
    };
    ({ container } = render(<Tooltip {...props} />));
  });

  it("should render Tooltip component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});
