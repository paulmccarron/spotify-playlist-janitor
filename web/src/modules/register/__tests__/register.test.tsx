import { fireEvent, getByTestId, render } from "@testing-library/react";

import { Register } from "../register";
import { useRegisterLogic } from "../use-register-logic";

jest.mock("../use-register-logic");

describe("<Register />", () => {
  let container: HTMLElement;

  const mockUseRegisterLogic = {
    disabled: false,
    onSubmit: jest.fn(e => e.preventDefault()),
    error: "",
  };

  beforeEach(() => {
    jest.mocked(useRegisterLogic).mockImplementation(() => mockUseRegisterLogic);

    ({ container } = render(<Register />));
  });

  it("should render Register component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });

  it("should render Register component with error", () => {
    const mockUseRegisterLogic2 = {
      ...mockUseRegisterLogic,
      error: "Test Error",
    };
    jest.mocked(useRegisterLogic).mockImplementation(() => mockUseRegisterLogic2);

    ({ container } = render(<Register />));

    expect(container.firstChild).toMatchSnapshot();

    const item1 = getByTestId(container, "submit-error");
    expect(item1.innerHTML).toContain("Test Error");
  });

  it("should render Register component with disabled elements", () => {
    const mockUseRegisterLogic2 = {
      ...mockUseRegisterLogic,
      disabled: true,
    };
    jest.mocked(useRegisterLogic).mockImplementation(() => mockUseRegisterLogic2);

    ({ container } = render(<Register />));

    expect(container.firstChild).toMatchSnapshot();

    const emailInput = getByTestId(container, "email-input");
    expect(emailInput).toHaveAttribute("disabled");

    const passwordInput = getByTestId(container, "password-input");
    expect(passwordInput).toHaveAttribute("disabled");

    const passwordInputConfirm = getByTestId(container, "password-confirm-input");
    expect(passwordInputConfirm).toHaveAttribute("disabled");

    const registerButton = getByTestId(container, "register-button");
    expect(registerButton).toHaveAttribute("disabled");
  });

  it("should call onSubmit function when Register button clicked", () => {
    const registerButton = getByTestId(container, "register-button");
    fireEvent.click(registerButton);

    expect(mockUseRegisterLogic.onSubmit).toHaveBeenCalled();
  });
});
