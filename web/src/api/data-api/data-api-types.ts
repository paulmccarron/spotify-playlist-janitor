type DatabasePlaylist = {
  id: string;
  skipThreshold?: number;
  ignoreInitialSkips: boolean;
  autoCleanupLimit?: number;
};

export type DatabasePlaylistResponse = DatabasePlaylist;

export type AddDatabasePlaylistRequest = DatabasePlaylist;

export type UpdateDatabasePlaylistRequest = Omit<DatabasePlaylist, "id">;

type Image = {
  height: number;
  width: number;
  url: string;
}

type SkippedTrackArtist = {
  id: string;
  name: string;
  href: string;
}

type SkippedTrackAlbum = {
  id: string;
  name: string;
  href: string;
  images: Image[];
}

type SkippedTrack = {
  playlistId: string;
  trackId: string;
  duration: number;
  name: string;
  skippedDate: Date;
  artists: SkippedTrackArtist[];
  album: SkippedTrackAlbum;
}

export type SkippedTrackResponse = SkippedTrack[];