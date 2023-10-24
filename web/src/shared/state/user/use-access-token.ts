import { useRecoilValue } from 'recoil';
import { accessTokenState } from './user-state';

export const useAccessToken = () => useRecoilValue(accessTokenState);
