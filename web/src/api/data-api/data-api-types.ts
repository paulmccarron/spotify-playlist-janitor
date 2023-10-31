type DatabasePlaylist = {
  id: string;
  skipThreshold?: number;
  ignoreInitialSkips: boolean;
  autoCleanupLimit?: number;
};

export type DatabasePlaylistResponse = DatabasePlaylist;

export type AddDatabasePlaylistRequest = DatabasePlaylist;

export type UpdateDatabasePlaylistRequest = Omit<DatabasePlaylist, "id">;
