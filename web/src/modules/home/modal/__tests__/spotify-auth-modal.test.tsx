import { fireEvent, getByTestId, render } from "@testing-library/react";

import { SpotifyAuthModalView } from "../spotify-auth-modal";

describe("<SpotifyAuthModalView />", () => {
  let container: HTMLElement;

  beforeEach(async () => {
    ({ container } = render(<SpotifyAuthModalView />));
  });

  beforeAll(() => {
    process.env.REACT_APP_API_URL = 'https://localhost:5001';
  });

  it("should render SpotifyAuthModalView component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});
