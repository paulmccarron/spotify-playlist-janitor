import { useRecoilValue } from 'recoil';
import { userData } from './user-state';

export const useUser = () => useRecoilValue(userData);
