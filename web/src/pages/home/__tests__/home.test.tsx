import { render } from '@testing-library/react';

import { Home } from '../home';

jest.mock("modules/home");

describe('<Home />', () => {
  let container: HTMLElement;

  beforeEach(() => {
    ({ container } = render(<Home />));
  });

  beforeAll(() => {
    process.env.REACT_APP_API_URL = 'https://localhost:5001';
  });

  it('should render a Home component', () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});