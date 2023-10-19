import { memo, PropsWithChildren } from "react";
import { Navigate } from "react-router-dom";

import { LOGIN } from "shared/constants";
import { useUser } from "shared/state/user";

import { isAuthDataValid } from "shared/utils/authentication-valid";

export const AuthProvider = memo(({ children }: PropsWithChildren) => {
  const user = useUser();
  const loggedIn = isAuthDataValid(user?.access_token, user?.expires_on);

  if (loggedIn) {
    return <>{children}</>;
  }

  return <Navigate to={LOGIN} />;
});

AuthProvider.displayName = "AuthProvider";
