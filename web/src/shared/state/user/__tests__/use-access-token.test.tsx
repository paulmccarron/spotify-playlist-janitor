import React, { ReactNode, RefObject } from 'react';
import { MutableSnapshot, RecoilRoot } from 'recoil';
import { renderHook } from '@testing-library/react-hooks';
import { userState as mockUserState } from "../../../mock-data/state"
import { useAccessToken } from '../use-access-token';
import { userState } from '../user-state';

jest.mock('api/auth-api');
jest.mock('shared/services/session-storage-service');

jest.useFakeTimers();

describe('useAccessToken', () => {
  let result: RefObject<ReturnType<typeof useAccessToken>>;

  beforeEach(() => {
    const initializeState = ({ set }: MutableSnapshot) => {
      set(userState, mockUserState);
    };

    ({ result } = renderHook(() => useAccessToken(), {
      wrapper: ({ children }: { children: ReactNode }) => <RecoilRoot {...{ initializeState }}>{children}</RecoilRoot>,
    }));
  });

  it('should return the current access token', () => {
    expect(result.current).toBe(mockUserState.access_token);
  });
});
