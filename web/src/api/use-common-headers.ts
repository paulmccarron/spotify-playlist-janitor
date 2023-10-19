import { useMemo } from "react";
// import { oktaAuth } from 'shared/services/okta-service';
// import { useCrmToken } from 'shared/state/user';

export const useCommonHeaders = () => {
  //   const crmToken = useCrmToken();

  return useMemo(
    () => ({
      //   authorization: `Bearer ${oktaAuth.getAccessToken()}`,
      //   crm_authorization: crmToken?.access_token ? `Bearer ${crmToken?.access_token}` : '',
    }),
    // [crmToken?.access_token],
    []
  );
};
