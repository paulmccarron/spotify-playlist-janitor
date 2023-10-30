import { DetailedPlaylist } from "modules/playlist/playlist-types";
import { databasePlaylists, spotifyPlaylists } from "shared/mock-data/api";
import { Image } from "shared/types";

const image: Image = {
  height: spotifyPlaylists.find(x => x.id === databasePlaylists[0].id)?.images[0]?.height || 100,
  width: spotifyPlaylists.find(x => x.id === databasePlaylists[0].id)?.images[0]?.width || 100,
  url: spotifyPlaylists.find(x => x.id === databasePlaylists[0].id)?.images[0]?.url || "href"
}

export const playlist: DetailedPlaylist = {
  id: databasePlaylists[0].id,
  href: spotifyPlaylists.find(x => x.id === databasePlaylists[0].id)?.href || "",
  name: spotifyPlaylists.find(x => x.id === databasePlaylists[0].id)?.name || "",
  image: image,
  ignoreInitialSkips: databasePlaylists[0].ignoreInitialSkips,
  autoCleanupLimit: databasePlaylists[0].autoCleanupLimit,
  skipThreshold: databasePlaylists[0].skipThreshold,
};
