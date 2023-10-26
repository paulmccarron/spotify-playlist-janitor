import { RefObject } from "react";
import { renderHook } from "@testing-library/react";

import { post } from "api/api";
import { LoginRequest } from "../auth-api-types";
import { useAuthApi } from "../use-auth-api";

jest.mock("api/api");
jest.mock("shared/state/user");

describe("useAuthApi", () => {
  let result: RefObject<ReturnType<typeof useAuthApi>>;

  beforeEach(() => {
    ({ result } = renderHook(() => useAuthApi()));
  });

  it("should call the loginRequest function when the login function is called", () => {
    const data: LoginRequest = {
      email: "qwer",
      password: "zxcv",
    };
    result.current?.login(data);

    expect(post).toHaveBeenCalledWith(
      "/login",
      {
        email: "qwer",
        password: "zxcv",
      },
      {
        headers: {
          authorization: expect.any(String),
        },
      }
    );
  });

  it("should call the registerRequest function when the register function is called", () => {
    const data: LoginRequest = {
      email: "qwer",
      password: "zxcv",
    };
    result.current?.register(data);

    expect(post).toHaveBeenCalledWith(
      "/register",
      {
        email: "qwer",
        password: "zxcv",
      },
      {
        headers: {
          authorization: expect.any(String),
        },
      }
    );
  });
});
