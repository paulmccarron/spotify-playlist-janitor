import { post } from "api/api";
import { login, refresh, register } from "../auth-api";
import {
  LoginRequest,
  RefreshRequest,
  RegisterRequest,
} from "../auth-api-types";

jest.mock("api/api");

describe("login", () => {
  const config = { headers: { HEADER: "HEADER" } };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it(`should call the post function with the /login URL when the login function is called`, () => {
    const body: LoginRequest = {
      email: "qwer",
      password: "zxcv",
    };
    login(body, config);
    expect(post).toHaveBeenLastCalledWith("/login", body, config);
  });
});

describe("refresh", () => {
  const config = { headers: { HEADER: "HEADER" } };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it(`should call the post function with the /refresh URL when the refresh function is called`, () => {
    const body: RefreshRequest = {
      refreshToken: "qwer",
    };
    refresh(body, config);
    expect(post).toHaveBeenLastCalledWith("/refresh", body, config);
  });
});

describe("register", () => {
  const config = { headers: { HEADER: "HEADER" } };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it(`should call the post function with the /register URL when the register function is called`, () => {
    const body: RegisterRequest = {
      email: "qwer",
      spotifyEmail: "asdf",
      password: "zxcv",
    };
    register(body, config);
    expect(post).toHaveBeenLastCalledWith("/register", body, config);
  });
});
