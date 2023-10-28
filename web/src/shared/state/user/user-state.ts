import { atom, selector, snapshot_UNSTABLE } from "recoil";
import { refresh } from "api/auth-api";
import { UserToken } from "shared/types";
import { getItem, setItem } from "shared/services/session-storage-service";

export const setAccurateTimeout = (fn: () => void, ms: number, offset = 50) => {
  let timeout: ReturnType<typeof setTimeout>;
  let halfLife = ms / 2;

  const onTimeout = () => {
    halfLife = halfLife / 2;

    if (halfLife < offset) {
      fn();
    } else {
      timeout = setTimeout(onTimeout, halfLife);
    }
  };

  timeout = setTimeout(onTimeout, halfLife);

  const cancel = () => {
    clearTimeout(timeout);
  };

  return { cancel } as const;
};

let timer: ReturnType<typeof setAccurateTimeout> | undefined;

const isAuthenticationDataValid = (userToken: UserToken | undefined) => {
  const nowInSeconds = new Date().getTime() / 1000;

  if (userToken?.expires_in) {
    return userToken.expires_on > nowInSeconds;
  }

  return false;
};

const getTimeout = (userToken: UserToken) => {
  // What time will the current User Token expire.
  //Check 5 minutes before the expiry time
  // prettier-ignore
  return userToken.expires_on - 300000;
};

export const userState = atom<UserToken | undefined>({
  key: "userToken",
  default: undefined,
  dangerouslyAllowMutability: true,
  effects: [
    ({ setSelf, trigger }) => {
      const user = getItem("USER_TOKEN") as UserToken;

      /**
       * If the values are stored in session storage check that the
       * expiration of the access token is in the future. If
       * they are set them to the state.
       */
      if (user && isAuthenticationDataValid(user) && trigger === "get") {
        setSelf(user);
      }
    },
    ({ setSelf, onSet }) => {
      onSet((newValue) => {
        /** Set the updated values to session storage */
        setItem("USER_TOKEN", newValue);
        timer?.cancel();

        /* istanbul ignore next */
        if (!newValue) {
          return;
        }

        const onTimeout = (token: UserToken) => async () => {
          try {
            const response = await refresh(
              { refreshToken: token.refresh_token },
              {
                headers: {
                  authorization: `Bearer ${token.access_token}`,
                },
              }
            );

            const newTokenData = response.data;

            const now = new Date().getTime();
            const newUserToken: UserToken = {
              ...newTokenData,
              expires_on: newTokenData.expires_in + now,
            };

            setSelf({
              ...newUserToken,
            });

            setItem("USER_TOKEN", {
              ...newUserToken,
            });

            // Create timeouts after the initial
            timer = setAccurateTimeout(
              onTimeout(newUserToken),
              getTimeout(newUserToken)
            );
          } catch (e) {
            // Retry re-authentication if the token refresh request fails
            timer = setAccurateTimeout(
              onTimeout(token),
              new Date().getTime()
            );
          }
        };

        // Create the initial timeout
        timer = setAccurateTimeout(onTimeout(newValue), getTimeout(newValue));
      });

      return () => timer?.cancel;
    },
  ],
});

export const userData = selector({
  key: "userData",
  get({ get }) {
    return get(userState);
  },
});

export const accessTokenState = selector({
  key: "access_token",
  get: ({ get }) => {
    return get(userState)?.access_token;
  },
});

export const getAuthorization = () => {
  const currentState = snapshot_UNSTABLE();
  const accessToken = currentState.getLoadable(accessTokenState).contents;

  return {
    authorization: `Bearer ${accessToken}`,
  };
};
