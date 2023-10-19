import { useAuthApi } from "api/auth-api";
import { useCallback, useState } from "react";
import { useNavigate } from "react-router-dom";
import { HOME, REGISTER } from "shared/constants";
import { useSetUser } from "shared/state/user";
import { UserToken } from "shared/types";

export const useLoginLogic = () => {
  const [error, setError] = useState<string | undefined>();
  const [disabled, setDisabled] = useState(false);

  const navigate = useNavigate();
  const { login } = useAuthApi();
  const setUser = useSetUser();

  const onSubmit = useCallback(
    async (event: React.FormEvent<HTMLFormElement>) => {
      event.preventDefault();

      const email = (event.currentTarget[0] as HTMLInputElement).value;
      const password = (event.currentTarget[1] as HTMLInputElement).value;

      if (!email) {
        setError("Please enter email");
        return;
      }

      if (!password) {
        setError("Please enter password");
        return;
      }

      try {
        setDisabled(true);
        const response = await login({ email, password });
        const token = response.data;

        const now = new Date().getTime();
        const userToken: UserToken = {
          ...token,
          expires_on: token.expires_in * 1000 + now,
        };

        setError(undefined);
        setUser(userToken);
        navigate(HOME);
      } catch (e: any) {
        if (e.response.status === 401) {
          setError("Failed to log in with email/password.");
        } else {
          setError(e.response?.data?.message || e.message || "Unknown error.");
        }
      } finally {
        setDisabled(false);
      }
    },
    [setError, login, setUser, navigate]
  );

  const onRegisterClick = useCallback(() => {
    navigate(REGISTER);
  }, [navigate]);

  return { disabled, onSubmit, error, onRegisterClick } as const;
};
