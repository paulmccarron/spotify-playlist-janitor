import { useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { HOME, LOGIN } from "shared/constants";
import { clearAll } from "shared/services/session-storage-service";
import { useSetUser, useUser } from "shared/state/user";
import { isAuthDataValid } from "shared/utils/authentication-valid";

export const useAppHeaderLogic = () => {
  const navigate = useNavigate();
  const user = useUser();
  const setUser = useSetUser();

  const loggedIn = isAuthDataValid(user?.access_token, user?.expires_on);

  const onHomeClick = useCallback(() => {
    navigate(HOME);
  }, [navigate]);

  const onSignOutClick = useCallback(() => {
    clearAll();
    setUser(undefined);
    navigate(LOGIN);
  }, [setUser, navigate]);

  return { onHomeClick, onSignOutClick, loggedIn } as const;
};
