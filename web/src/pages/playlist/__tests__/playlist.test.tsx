import { render } from '@testing-library/react';

import { Playlist } from '../playlist';

jest.mock("modules/playlist");
jest.mock("react-router-dom", () => ({
  useParams: jest.fn(() => ({id: "testPlaylistId"})),
}));

describe('<Playlist />', () => {
  let container: HTMLElement;

  beforeEach(() => {
    ({ container } = render(<Playlist />));
  });

  it('should render a Playlist component', () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});