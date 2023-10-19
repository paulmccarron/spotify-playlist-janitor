export type UserToken = {
  token_type: string;
  expires_in: number;
  expires_on: number;
  access_token: string;
  refresh_token: string;
};
