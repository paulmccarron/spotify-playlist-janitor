export type LoginRequest = {
  email: string;
  password: string;
};

export type LoginResponse = {
  token_type: string;
  expires_in: number;
  access_token: string;
  refresh_token: string;
};

export type RefreshRequest = {
  refreshToken: string;
};
