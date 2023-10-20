import { LoginResponse } from "api/auth-api/auth-api-types";

type AuthData = {
  login: LoginResponse;
};

export const auth: AuthData = {
  login: {
    refresh_token: "access_token",
    token_type: "Bearer",
    access_token: "access_token",
    expires_in: 4503,
  },
};
