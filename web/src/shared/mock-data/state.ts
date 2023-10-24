import { UserToken } from "shared/types";

export const userState: UserToken = {
  refresh_token: "access_token",
  token_type: "Bearer",
  access_token: "access_token",
  expires_in: 4503,
  expires_on: new Date().getTime() + 1000 * 100,
};
