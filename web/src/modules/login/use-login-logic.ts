import { useAuthApi } from "api/auth-api";
import { useCallback, useState } from "react";
import { useNavigate } from "react-router-dom";
import { HOME, REGISTER } from "shared/constants";

export const useLoginLogic = () => {
  const [error, setError] = useState<string | undefined>();

  const navigate = useNavigate();
  const { login } = useAuthApi();

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
        const response = await login({ email, password });
        const token = response.data;

        setError(undefined);
        navigate(HOME);
      } catch (e: any) {
        if (e.response.status === 401) {
          setError("Failed to log in with email/password.");
        } else {
          setError(e.message || "Unknown error.");
        }
      }
    },
    [setError, login, navigate]
  );

  const onRegisterClick = useCallback(() => {
    navigate(REGISTER);
  }, [navigate]);

  return { onSubmit, error, onRegisterClick } as const;
};
