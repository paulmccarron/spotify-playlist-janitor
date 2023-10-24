import { memo, PropsWithChildren } from "react";
import { Navigate } from "react-router-dom";

import { useUser } from "shared/state/user";

import { isAuthDataValid } from "shared/utils/authentication-valid";

type AuthProviderProps = {
  shouldBeAuthorised: boolean;
  redirectPath: string;
};

export const AuthProvider = memo(
  ({
    shouldBeAuthorised,
    redirectPath,
    children,
  }: PropsWithChildren<AuthProviderProps>) => {
    const user = useUser();
    const loggedIn = isAuthDataValid(user?.access_token, user?.expires_on);

    if (loggedIn === shouldBeAuthorised) {
      return <>{children}</>;
    }

    return (
      <Navigate
        data-testid={"redirect-to-" + redirectPath.replace("/", "")}
        to={redirectPath}
      />
    );
  }
);

AuthProvider.displayName = "AuthProvider";
