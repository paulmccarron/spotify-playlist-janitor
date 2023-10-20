import { userState } from "../../../mock-data/state"

export const useUser = jest.fn(() => ({ ...userState }));
