import { FormEvent, ReactNode, RefObject } from "react";
import { RecoilRoot } from "recoil";
import { act, renderHook, waitFor } from "@testing-library/react";

import { useAppHeaderLogic } from "../use-app-header-logic";

const mockNavigate = jest.fn();
const mockSetUser = jest.fn();

jest.mock("shared/state/user", () => ({
  useUser: jest.fn(() => ({test: ""})),
  useSetUser: jest.fn(() => mockSetUser),
}));
jest.mock("react-router-dom", () => ({
  useNavigate: jest.fn(() => mockNavigate),
}));

describe("useAppHeaderLogic", () => {
  let result: RefObject<ReturnType<typeof useAppHeaderLogic>>;

  beforeEach(() => {
    ({ result } = renderHook(() => useAppHeaderLogic(), {
      wrapper: ({ children }: { children: ReactNode }) => (
        <RecoilRoot>{children}</RecoilRoot>
      ),
    }));
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("should navigate to HOME when onHomeClick called", async () => {
    act(() => {
      result.current?.onHomeClick();
    });

    await waitFor(() => expect(mockNavigate).toHaveBeenCalledWith("/"));
  });

  it("should clear state and navigate to LOGIN when onHomeClick called", async () => {
    act(() => {
      result.current?.onSignOutClick();
    });

    await waitFor(() => expect(mockSetUser).toHaveBeenCalledWith(undefined));
    await waitFor(() => expect(mockNavigate).toHaveBeenCalledWith("/login"));
  });
});
