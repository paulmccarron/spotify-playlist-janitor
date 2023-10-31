import { Playlist } from "shared/types";

export type DetailedPlaylist = Playlist & {
  skipThreshold?: number;
  ignoreInitialSkips: boolean;
  autoCleanupLimit?: number;
};
