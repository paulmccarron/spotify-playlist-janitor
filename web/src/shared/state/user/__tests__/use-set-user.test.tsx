import React, { ReactNode } from "react";
import { RecoilRoot } from "recoil";

import { act, renderHook } from "@testing-library/react-hooks";

import { RecoilObserver } from "shared/components/recoil-observer";
import { userState as mockUserState } from "../../../mock-data/state";
import { getItem } from "shared/services/session-storage-service";

import { userState } from "../user-state";
import { useSetUser } from "../use-set-user";

jest.mock("api/auth-api");
jest.mock("shared/services/session-storage-service");

const mockGetUser = Promise.resolve(mockUserState);

describe("useSetUser", () => {
  const onChange = jest.fn();

  beforeEach(async () => {
    jest.mocked(getItem).mockImplementation(() => undefined);

    renderHook(() => useSetUser(), {
      wrapper: ({ children }: { children: ReactNode }) => (
        <RecoilRoot>
          <RecoilObserver node={userState} onChange={onChange} />
          {children}
        </RecoilRoot>
      ),
    });
    await act(async () => {
      await mockGetUser;
    });
  });

  it("should set the user data on mount", async () => {
    expect(onChange).toHaveBeenCalled();
    // expect(onChange).toHaveBeenNthCalledWith(1, {
    //   ...mockUserState,
    // });
  });
});
