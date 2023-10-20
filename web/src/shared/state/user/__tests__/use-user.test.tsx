import React, { ReactNode, RefObject } from "react";
import { MutableSnapshot, RecoilRoot } from "recoil";
import { renderHook } from "@testing-library/react-hooks";

import { getItem } from "shared/services/session-storage-service";
import { userState as mockUserState } from "../../../mock-data/state";

import { useUser } from "../use-user";
import { userState } from "../user-state";

jest.mock("shared/services/session-storage-service");
jest.mock("api/auth-api");

describe("useUser", () => {
  let result: RefObject<ReturnType<typeof useUser>>;
  (getItem as jest.Mock).mockImplementation(() => undefined);

  beforeEach(() => {
    const initializeState = ({ set }: MutableSnapshot) => {
      set(userState, { ...mockUserState });
    };
    ({ result } = renderHook(() => useUser(), {
      wrapper: ({ children }: { children: ReactNode }) => (
        <RecoilRoot {...{ initializeState }}>{children}</RecoilRoot>
      ),
    }));
  });

  it("should return the user state", async () => {
    expect(result.current).toStrictEqual(mockUserState);
  });

  it("should not return the user state if none is set in session storage", () => {
    ({ result } = renderHook(() => useUser(), {
      wrapper: ({ children }: { children: ReactNode }) => (
        <RecoilRoot>{children}</RecoilRoot>
      ),
    }));
    expect(result.current).toBeUndefined();
  });
});
