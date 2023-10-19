import { AxiosRequestConfig } from "axios";
import { post } from "api/api";
import { LoginRequest, LoginResponse } from "./auth-api-types";

export const login = (body: LoginRequest, config: AxiosRequestConfig) =>
  post<LoginResponse>("/login", body, config);
