import { render } from '@testing-library/react';

import { Register } from '../register';

jest.mock("modules/register");

describe('<Register />', () => {
  let container: HTMLElement;

  beforeEach(() => {
    ({ container } = render(<Register />));
  });

  it('should render a Register component', () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});