import { useCallback } from "react";
import { useCommonHeaders } from "api/use-common-headers";
import { LoginRequest } from "./auth-api-types";
import { login as loginRequest, register as registerRequest } from "./auth-api";

export const useAuthApi = () => {
  const commonHeaders = useCommonHeaders();

  const login = useCallback(
    (request: LoginRequest) =>
      loginRequest(request, {
        headers: {
          ...commonHeaders,
        },
      }),
    [commonHeaders]
  );

  const register = useCallback(
    (request: LoginRequest) =>
      registerRequest(request, {
        headers: {
          ...commonHeaders,
        },
      }),
    [commonHeaders]
  );

  return {
    login,
    register,
  } as const;
};
