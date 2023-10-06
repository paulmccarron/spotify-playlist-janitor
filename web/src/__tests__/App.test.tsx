import React from 'react';
import { render } from '@testing-library/react';
import App from '../App';

// jest.mock('react-router-dom', () => ({
//   BrowserRouter: ({ children }: any) => <div>{children}</div>,
// }));

// jest.mock('modules/header');
// jest.mock('modules/footer');
// jest.mock('modules/routes', () => ({
//   Routes: () => <div>Routes</div>,
//   Security: ({ children }: any) => <div>{children}</div>,
// }));
// jest.mock('modules/user');

// jest.mock('modules/snackbar', () => ({
//   Snackbar: ({ children }: any) => <div>{children}</div>,
// }));

describe('<App />', () => {
  it('should render the App', async () => {
    const { container } = render(<App />);
    expect(container.firstChild).toMatchSnapshot();
  });
});
