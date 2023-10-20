import { RefObject } from 'react';
import { renderHook } from '@testing-library/react-hooks';

import { useCommonHeaders } from '../use-common-headers';

jest.mock('shared/state/user');

describe('useCommonHeaders', () => {
  let result: RefObject<ReturnType<typeof useCommonHeaders>>;

  beforeEach(() => {
    ({ result } = renderHook(() => useCommonHeaders()));
  });

  it('should return headers', () => {
    expect(result.current).toEqual({
      authorization: expect.any(String),
    });
  });
});
