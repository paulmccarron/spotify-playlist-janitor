import React from "react";
import { getByTestId, render } from "@testing-library/react";

import { isAuthDataValid } from "shared/utils/authentication-valid";
import { AuthProvider } from "../auth-provider";

jest.mock("shared/state/user");
jest.mock("shared/utils/authentication-valid");

jest.mock("react-router-dom", () => ({
  ...jest.requireActual("react-router-dom"),
  Navigate: jest.fn(() => <div {...{ "data-testid": "redirect-page" }}>Redirect Page</div>),
}));

describe("AuthProvider", () => {
  let container: HTMLElement;

  beforeEach(() => {
    jest.mocked(isAuthDataValid).mockImplementation(() => true);

    ({ container } = render(
      <AuthProvider shouldBeAuthorised={true} redirectPath="/test">
        <div {...{ "data-testid": "child-component" }}>Child Page</div>
      </AuthProvider>
    ));
  });

  it("should render the children props if auth data is valid and shouldBeAuthorised === true", () => {
    expect(container.firstChild).toMatchSnapshot();
    
    const child = getByTestId(container, "child-component");
    expect(child.innerHTML).toContain("Child Page");
  });

  it("should render a redirect component if auth data is invalid and shouldBeAuthorised === true", () => {
    jest.mocked(isAuthDataValid).mockImplementation(() => false);

    ({ container } = render(
      <AuthProvider shouldBeAuthorised={true} redirectPath="/test">
        <div {...{ "data-testid": "child-component" }}>Child Page</div>
      </AuthProvider>
    ));

    expect(container.firstChild).toMatchSnapshot();

    const redirect = getByTestId(container, "redirect-page");
    expect(redirect.innerHTML).toContain("Redirect Page");
  });

  it("should render a redirect component if auth data is valid and shouldBeAuthorised === false", () => {
    ({ container } = render(
      <AuthProvider shouldBeAuthorised={false} redirectPath="/test">
        <div {...{ "data-testid": "child-component" }}>Child Page</div>
      </AuthProvider>
    ));

    expect(container.firstChild).toMatchSnapshot();

    const redirect = getByTestId(container, "redirect-page");
    expect(redirect.innerHTML).toContain("Redirect Page");
  });

  it("should render the children props if auth data is invalid and shouldBeAuthorised === true", () => {
    jest.mocked(isAuthDataValid).mockImplementation(() => false);

    ({ container } = render(
      <AuthProvider shouldBeAuthorised={false} redirectPath="/test">
        <div {...{ "data-testid": "child-component" }}>Child Page</div>
      </AuthProvider>
    ));

    expect(container.firstChild).toMatchSnapshot();
    
    const child = getByTestId(container, "child-component");
    expect(child.innerHTML).toContain("Child Page");
  });
});
