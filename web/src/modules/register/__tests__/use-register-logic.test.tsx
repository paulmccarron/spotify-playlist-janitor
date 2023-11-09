import { FormEvent, RefObject } from "react";
import { act, renderHook, waitFor } from "@testing-library/react";

import { useRegisterLogic } from "../use-register-logic";
import { useAuthApi } from "api/auth-api";

const mockNavigate = jest.fn();

jest.mock("api/auth-api");
jest.mock("react-router-dom", () => ({
  useNavigate: jest.fn(() => mockNavigate),
}));

describe("useRegisterLogic", () => {
  let result: RefObject<ReturnType<typeof useRegisterLogic>>;
  let mockUseAuthApi: any;
  let event: FormEvent<HTMLFormElement>;

  beforeEach(() => {
    mockUseAuthApi = {
      register: jest.fn(() => Promise.resolve()),
    };

    event = {
      //@ts-ignore
      preventDefault: jest.fn(),
      //@ts-ignore
      currentTarget: [
        //@ts-ignore
        { value: "emailValue" },
        //@ts-ignore
        { value: "passwordValue" },
        //@ts-ignore
        { value: "passwordValue" },
      ],
    };

    jest.mocked(useAuthApi).mockImplementation(() => mockUseAuthApi);
    ({ result } = renderHook(() => useRegisterLogic()));
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("should set error when onSubmit called and email input is empty", () => {
    //@ts-ignore
    event.currentTarget = [
      //@ts-ignore
      { value: "" },
      //@ts-ignore
      { value: "passwordValue" },
      //@ts-ignore
      { value: "passwordValue" },
    ];

    act(() => {
      result.current?.onSubmit(event);
    });

    expect(result.current?.error).toBe("Please enter email");
  });

  it("should set error when onSubmit called and password input is empty", () => {
    //@ts-ignore
    event.currentTarget = [
      //@ts-ignore
      { value: "email" },
      //@ts-ignore
      { value: "" },
      //@ts-ignore
      { value: "passwordValue" },
    ];

    act(() => {
      result.current?.onSubmit(event);
    });

    expect(result.current?.error).toBe("Please enter password");
  });

  it("should set error when onSubmit called and password confirm input is empty", () => {
    //@ts-ignore
    event.currentTarget = [
      //@ts-ignore
      { value: "email" },
      //@ts-ignore
      { value: "passwordValue" },
      //@ts-ignore
      { value: "" },
    ];

    act(() => {
      result.current?.onSubmit(event);
    });

    expect(result.current?.error).toBe("Please confirm password");
  });

  it("should set error when onSubmit called and password and password confirm inputs are different", () => {
    //@ts-ignore
    event.currentTarget = [
      //@ts-ignore
      { value: "email" },
      //@ts-ignore
      { value: "passwordValue" },
      //@ts-ignore
      { value: "passwordValueDifferent" },
    ];

    act(() => {
      result.current?.onSubmit(event);
    });

    expect(result.current?.error).toBe("Passwords do not match");
  });

  it("should set User state and navigate when onSubmit called login succeeds", async () => {
    act(() => {
      result.current?.onSubmit(event);
    });

    await waitFor(() => expect(mockNavigate).toHaveBeenCalledWith("/login"));
  });

  [
    {
      response: { data: { message: "User already exists" } },
      expectedMessage: "User already exists",
    },
    {
      response: { format: "unknown" },
      expectedMessage: "Unknown error",
    },
  ].forEach((setup) => {
    it(`should set error: ${setup.expectedMessage} when onSubmit called login fails`, async () => {
      mockUseAuthApi = {
        register: jest.fn(() => Promise.reject({ response: setup.response })),
      };

      jest.mocked(useAuthApi).mockImplementation(() => mockUseAuthApi);
      ({ result } = renderHook(() => useRegisterLogic()));

      act(() => {
        result.current?.onSubmit(event);
      });

      await waitFor(() =>
        expect(result.current?.error).toBe(setup.expectedMessage)
      );
    });
  });
});
