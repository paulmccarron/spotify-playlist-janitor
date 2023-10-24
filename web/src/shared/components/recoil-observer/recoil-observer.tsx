import { useEffect } from 'react';
import { RecoilState, useRecoilValue } from 'recoil';

export type RecoilObserverProps<T> = {
  node: RecoilState<T>;
  onChange(value: any): void;
};

export const RecoilObserver = <T,>({ node, onChange }: RecoilObserverProps<T>) => {
  const value = useRecoilValue(node);
  useEffect(() => onChange(value), [onChange, value]);
  return null;
};
