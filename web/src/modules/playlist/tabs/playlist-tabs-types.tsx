import { Image } from "shared/types";


export type SkippedTrackArtist = {
    id: string;
    name: string;
    href: string;
}

export type SkippedTrackAlbum = {
    id: string;
    name: string;
    href: string;
}

type SkippedTrack = {
    id: string;
    name: string;
    duration: number;
    artists: SkippedTrackArtist[];
    album: SkippedTrackAlbum;
    image: Image;
}

export type SkippedTrackTotal = SkippedTrack & {
    skippedTotal: number;
}

export type SkippedTrackHistory = SkippedTrack & {
    skippedDate: Date;
}