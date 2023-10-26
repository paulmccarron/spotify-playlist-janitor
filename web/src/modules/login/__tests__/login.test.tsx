import { fireEvent, getByTestId, render } from "@testing-library/react";

import { Login } from "../login";
import { useLoginLogic } from "../use-login-logic";

jest.mock("../use-login-logic");

describe("<Login />", () => {
  let container: HTMLElement;

  const mockUseLoginLogic = {
    disabled: false,
    onSubmit: jest.fn(e => e.preventDefault()),
    error: "",
    onRegisterClick: jest.fn(),
  };

  beforeEach(() => {
    jest.mocked(useLoginLogic).mockImplementation(() => mockUseLoginLogic);

    ({ container } = render(<Login />));
  });

  it("should render Login component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render Login component with error", () => {
    const mockUseLoginLogic2 = {
      ...mockUseLoginLogic,
      error: "Test Error",
    };
    jest.mocked(useLoginLogic).mockImplementation(() => mockUseLoginLogic2);

    ({ container } = render(<Login />));

    expect(container.firstChild).toMatchSnapshot();

    const item1 = getByTestId(container, "submit-error");
    expect(item1.innerHTML).toContain("Test Error");
  });

  it("should render Login component with disabled elements", () => {
    const mockUseLoginLogic2 = {
      ...mockUseLoginLogic,
      disabled: true,
    };
    jest.mocked(useLoginLogic).mockImplementation(() => mockUseLoginLogic2);

    ({ container } = render(<Login />));

    expect(container.firstChild).toMatchSnapshot();

    const emailInput = getByTestId(container, "email-input");
    expect(emailInput).toHaveAttribute("disabled");

    const passwordInput = getByTestId(container, "password-input");
    expect(passwordInput).toHaveAttribute("disabled");

    const loginButton = getByTestId(container, "login-button");
    expect(loginButton).toHaveAttribute("disabled");

    const registerButton = getByTestId(container, "register-button");
    expect(registerButton).toHaveAttribute("disabled");
  });

  it("should call onSubmit function when Login button clicked", () => {
    const loginButton = getByTestId(container, "login-button");
    fireEvent.click(loginButton);

    expect(mockUseLoginLogic.onSubmit).toHaveBeenCalled();
  });

  it("should call onRegisterClick function when Register button clicked", () => {
    const registerButton = getByTestId(container, "register-button");
    fireEvent.click(registerButton);
    
    expect(mockUseLoginLogic.onRegisterClick).toHaveBeenCalled();
  });
});
