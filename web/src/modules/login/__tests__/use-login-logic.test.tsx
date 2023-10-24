import { FormEvent, ReactNode, RefObject } from "react";
import { RecoilRoot } from "recoil";
import { act, renderHook, waitFor } from "@testing-library/react";

import { useLoginLogic } from "../use-login-logic";
import { useAuthApi } from "api/auth-api";
import { auth } from "shared/mock-data/api";

const mockNavigate = jest.fn();
const mockSetUser = jest.fn();

jest.mock("api/auth-api");
jest.mock("shared/state/user", () => ({
  useSetUser: jest.fn(() => mockSetUser),
}));
jest.mock("react-router-dom", () => ({
  useNavigate: jest.fn(() => mockNavigate),
}));

describe("useLoginLogic", () => {
  let result: RefObject<ReturnType<typeof useLoginLogic>>;
  let mockUseAuthApi: any;
  let event: FormEvent<HTMLFormElement>;

  beforeEach(() => {
    mockUseAuthApi = {
      login: jest.fn(() => Promise.resolve({ data: auth.login })),
    };

    event = {
      //@ts-ignore
      preventDefault: jest.fn(),
      //@ts-ignore
      currentTarget: [{ value: "emailValue" }, { value: "passwordValue" }],
    };

    (useAuthApi as jest.Mock).mockImplementation(() => mockUseAuthApi);
    ({ result } = renderHook(() => useLoginLogic(), {
      wrapper: ({ children }: { children: ReactNode }) => (
        <RecoilRoot>{children}</RecoilRoot>
      ),
    }));
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("should set error when onSubmit called and email input is empty", () => {
    //@ts-ignore
    event.currentTarget = [{ value: "" }, { value: "passwordValue" }];

    act(() => {
      result.current?.onSubmit(event);
    });

    expect(result.current?.error).toBe("Please enter email");
  });

  it("should set error when onSubmit called and password input is empty", () => {
    //@ts-ignore
    event.currentTarget = [{ value: "emailValue" }, { value: "" }];

    act(() => {
      result.current?.onSubmit(event);
    });

    expect(result.current?.error).toBe("Please enter password");
  });

  it("should set User state and navigate when onSubmit called login succeeds", async () => {
    act(() => {
      result.current?.onSubmit(event);
    });

    await waitFor(() => expect(mockSetUser).toHaveBeenCalled());
    await waitFor(() => expect(mockNavigate).toHaveBeenCalledWith("/"));
  });

  [
    {
      response: { status: 401 },
      expectedMessage: "Failed to log in with email/password",
    },
    {
      response: { data: { message: "Data Message" } },
      expectedMessage: "Data Message",
    },
    {
      response: { format: "unknown" },
      expectedMessage: "Unknown error",
    },
  ].forEach((setup) => {
    it("should set error when onSubmit called login fails", async () => {
      mockUseAuthApi = {
        login: jest.fn(() => Promise.reject({ response: setup.response })),
      };

      (useAuthApi as jest.Mock).mockImplementation(() => mockUseAuthApi);
      ({ result } = renderHook(() => useLoginLogic(), {
        wrapper: ({ children }: { children: ReactNode }) => (
          <RecoilRoot>{children}</RecoilRoot>
        ),
      }));

      act(() => {
        result.current?.onSubmit(event);
      });

      await waitFor(() =>
        expect(result.current?.error).toBe(setup.expectedMessage)
      );
    });
  });

  it("should call naviagte when onRegisterClick called", () => {
    result.current?.onRegisterClick();
    expect(mockNavigate).toHaveBeenCalledWith("/register");
  });
});
