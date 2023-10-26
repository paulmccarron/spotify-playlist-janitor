import { render } from "@testing-library/react";

import { SpotifyAuthModalView } from "../spotify-auth-modal";

describe("<SpotifyAuthModalView />", () => {
  let container: HTMLElement;
  beforeEach(async () => {
    ({ container } = render(<SpotifyAuthModalView />));
  });

  it("should render SpotifyAuthModalView component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});
