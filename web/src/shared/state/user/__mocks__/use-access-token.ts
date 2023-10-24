
import { userState } from "../../../mock-data/state"

export const useAccessToken = jest.fn(() => userState.access_token);
