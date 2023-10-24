import { fireEvent, getByTestId, render } from "@testing-library/react";

import { AppHeader } from "..";
import { useAppHeaderLogic } from "../use-app-header-logic";

jest.mock("../use-app-header-logic");

describe("<AppHeader />", () => {
  let container: HTMLElement;

  const mockUseAppHeaderLogic = {
    onHomeClick: jest.fn(),
    onSignOutClick: jest.fn(),
    loggedIn: true,
  };

  beforeEach(() => {
    (useAppHeaderLogic as jest.Mock).mockImplementation(
      () => mockUseAppHeaderLogic
    );
    ({ container } = render(<AppHeader />));
  });

  it("should render AppHeader component when logged in", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render AppHeader component when not logged in", () => {
    const mockUseAppHeaderLogic2 = {
      ...mockUseAppHeaderLogic,
      loggedIn: false,
    };
    (useAppHeaderLogic as jest.Mock).mockImplementation(
      () => mockUseAppHeaderLogic2
    );
    ({ container } = render(<AppHeader />));
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should call onHomeClick when Home clicked", () => {
    const home = getByTestId(container, "home");
    fireEvent.click(home);
    
    expect(mockUseAppHeaderLogic.onHomeClick).toHaveBeenCalled();
  });

  it("should call onSignOutClick when Sign Out clicked", () => {
    const signOut = getByTestId(container, "sign-out");
    fireEvent.click(signOut);
    
    expect(mockUseAppHeaderLogic.onSignOutClick).toHaveBeenCalled();
  });
});
