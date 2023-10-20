import { userState } from "../../../shared/mock-data/state";

const loginResponse = {
  token_type: userState.token_type,
  expires_in: userState.expires_in,
  access_token: userState.access_token,
  refresh_token: userState.refresh_token,
};

export const useAuthApi = jest.fn(() => ({
  login: Promise.resolve(loginResponse),
  register: Promise.resolve(loginResponse),
}));
