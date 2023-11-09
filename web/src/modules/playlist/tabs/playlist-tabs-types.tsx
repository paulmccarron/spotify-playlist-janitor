import { Image } from "shared/types";

export type SkippedTrackArtist = {
  id: string;
  name: string;
  href: string;
};

export type SkippedTrackAlbum = {
  id: string;
  name: string;
  href: string;
};

export type Track = {
  id: string;
  name: string;
  duration: number;
  artists: SkippedTrackArtist[];
  album: SkippedTrackAlbum;
  image: Image;
};

export type SkippedTrackTotal = Track & {
  skippedTotal: number;
};

export type SkippedTrackHistory = Track & {
  skippedDate: Date;
};

export type DeleteTrack = {
  id: string;
  name: string;
};
