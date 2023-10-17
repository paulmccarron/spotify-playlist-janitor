import React from 'react';
import { render } from '@testing-library/react';

import { Home } from '../home';

describe('<Home />', () => {
  let container: HTMLElement;

  beforeEach(() => {
    ({ container } = render(<Home />));
  });

  beforeAll(() => {
    process.env.LOCAL_ENVIRONMENT = 'https://localhost:5001';
  });

  it('should render a Home component', () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});