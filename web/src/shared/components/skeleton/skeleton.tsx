import { default as ReactSkeleton, SkeletonProps } from 'react-loading-skeleton'
import 'react-loading-skeleton/dist/skeleton.css'

export const Skeleton = ({ ...props }: SkeletonProps) => {
    return <ReactSkeleton {...props} />;
};