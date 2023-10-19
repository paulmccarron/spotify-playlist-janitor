import { AxiosRequestConfig } from "axios";
import { post } from "api/api";
import { LoginRequest, LoginResponse, RefreshRequest } from "./auth-api-types";

export const login = (body: LoginRequest, config: AxiosRequestConfig) =>
  post<LoginResponse>("/login", body, config);

export const refresh = (body: RefreshRequest, config: AxiosRequestConfig) =>
  post<LoginResponse>("/refresh", body, config);

export const register = (body: LoginRequest, config: AxiosRequestConfig) =>
  post<LoginResponse>("/register", body, config);
