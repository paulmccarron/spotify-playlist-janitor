import { PropsWithChildren } from 'react';
import { SkeletonTheme as ReackSkeletonTheme, SkeletonThemeProps } from 'react-loading-skeleton'
import 'react-loading-skeleton/dist/skeleton.css'

export const SkeletonTheme = ({ children, ...props }: PropsWithChildren<SkeletonThemeProps>) => {
  return <ReackSkeletonTheme {...props}>
    {children}
  </ReackSkeletonTheme>
};