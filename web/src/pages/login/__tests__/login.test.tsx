import React from 'react';
import { render } from '@testing-library/react';

import { Login } from '../login';

jest.mock("modules/login");

describe('<Login />', () => {
  let container: HTMLElement;

  beforeEach(() => {
    ({ container } = render(<Login />));
  });

  it('should render a Login component', () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});