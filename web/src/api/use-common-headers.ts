import { useMemo } from "react";
import { useAccessToken } from "shared/state/user";

export const useCommonHeaders = () => {
  const accessToken = useAccessToken();

  return useMemo(
    () => ({
      authorization: accessToken ? `Bearer ${accessToken}` : "",
    }),
    [accessToken]
  );
};
