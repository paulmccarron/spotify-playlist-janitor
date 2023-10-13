import React, { RefObject } from "react";
import { act, renderHook } from "@testing-library/react";

import { useModal } from "../use-modal";

describe("<useModal />", () => {
  let result: RefObject<ReturnType<typeof useModal>>;

  beforeEach(() => {
    ({ result } = renderHook(() => useModal()));
  });

  it("should return isOpen === false", () => {
    expect(result.current?.isOpen).toBe(false);
  });

  it("should return isOpen === true when onOpen called", () => {
    act(() => {
      result.current?.onOpen();
    });
    expect(result.current?.isOpen).toBe(true);
  });

  it("should return isOpen === false when onClose called", () => {
    act(() => {
      result.current?.onOpen();
    });
    expect(result.current?.isOpen).toBe(true);

    act(() => {
      result.current?.onClose();
    });
    expect(result.current?.isOpen).toBe(false);
  });
});
