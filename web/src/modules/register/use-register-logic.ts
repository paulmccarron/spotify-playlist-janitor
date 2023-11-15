import { useAuthApi } from "api/auth-api";
import { useCallback, useState } from "react";
import { useNavigate } from "react-router-dom";
import { LOGIN } from "shared/constants";

export const useRegisterLogic = () => {
  const [error, setError] = useState<string | undefined>();
  const [disabled, setDisabled] = useState(false);

  const navigate = useNavigate();
  const { register } = useAuthApi();

  const onSubmit = useCallback(
    async (event: React.FormEvent<HTMLFormElement>) => {
      event.preventDefault();

      const email = (event.currentTarget[0] as HTMLInputElement).value;
      const spotifyEmail = (event.currentTarget[1] as HTMLInputElement).value;
      const password = (event.currentTarget[2] as HTMLInputElement).value;
      const confirmPassword = (event.currentTarget[3] as HTMLInputElement)
        .value;

      if (!email) {
        setError("Please enter email");
        return;
      }

      if (!password) {
        setError("Please enter password");
        return;
      }

      if (!confirmPassword) {
        setError("Please confirm password");
        return;
      }

      if (password !== confirmPassword) {
        setError("Passwords do not match");
        return;
      }

      try {
        setDisabled(true);
        await register({
          email,
          spotifyEmail: !!spotifyEmail ? spotifyEmail : email,
          password,
        });

        setError(undefined);
        navigate(LOGIN);
      } catch (e: any) {
        setError(e.response?.data?.message || "Unknown error");
      } finally {
        setDisabled(false);
      }
    },
    [setError, register, navigate]
  );

  return { disabled, onSubmit, error } as const;
};
