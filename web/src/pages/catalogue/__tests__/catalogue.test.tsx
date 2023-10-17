import React from 'react';
import { render } from '@testing-library/react';

import { Catalogue } from '../catalogue';

describe('<Catalogue />', () => {
  let container: HTMLElement;

  beforeEach(() => {
    ({ container } = render(<Catalogue />));
  });

  it('should render a Catalogue component', () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});