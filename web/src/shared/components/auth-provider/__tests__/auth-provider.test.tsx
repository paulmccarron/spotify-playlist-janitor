import React from "react";
import { render } from "@testing-library/react";

import { isAuthDataValid } from "shared/utils/authentication-valid";
import { AuthProvider } from "../auth-provider";

jest.mock("shared/state/user");
jest.mock("shared/utils/authentication-valid");

jest.mock("react-router-dom", () => ({
  ...jest.requireActual("react-router-dom"),
  Navigate: jest.fn(() => <div>Redirect Page</div>),
}));

describe("AuthProvider", () => {
  let container: HTMLElement;

  beforeEach(() => {
    jest.mocked(isAuthDataValid).mockImplementation(() => true);

    ({ container } = render(
      <AuthProvider>
        <div>Child Page</div>
      </AuthProvider>
    ));
  });

  it("should render a the children props if auth data is valid", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render a redirect component if auth data is invalid", () => {
    jest.mocked(isAuthDataValid).mockImplementation(() => false);

    ({ container } = render(
      <AuthProvider>
        <div>Child Page</div>
      </AuthProvider>
    ));

    expect(container.firstChild).toMatchSnapshot();
  });
});
