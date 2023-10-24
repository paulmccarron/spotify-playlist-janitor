import { useSetRecoilState } from 'recoil';
import { userState } from './user-state';

export const useSetUser = () => useSetRecoilState(userState);
