import { Playlist, Image } from "shared/types";

export type DetailedPlaylist = Playlist & {
  skipThreshold?: number;
  ignoreInitialSkips: boolean;
  autoCleanupLimit?: number;
};
