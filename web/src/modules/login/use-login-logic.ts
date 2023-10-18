import { useCallback, useState } from "react";
import { useNavigate } from "react-router-dom";
import { HOME, REGISTER } from "shared/constants";

export const useLoginLogic = () => {
  const navigate = useNavigate();
  const [error, setError] = useState<string | undefined>();

  const onSubmit = useCallback(
    (event: React.FormEvent<HTMLFormElement>) => {
      event.preventDefault();

      if (!(event.currentTarget[0] as HTMLInputElement).value) {
        setError("Please enter email");
        return;
      }

      if (!(event.currentTarget[1] as HTMLInputElement).value) {
        setError("Please enter password");
        return;
      }

      setError(undefined);

      navigate(HOME);
    },
    [setError, navigate]
  );

  const onRegisterClick = useCallback(() => {
    navigate(REGISTER);
  }, [navigate]);

  return { onSubmit, error, onRegisterClick } as const;
};
