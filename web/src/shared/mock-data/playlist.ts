import { DetailedPlaylist } from "modules/playlist/playlist-types";
import {
  SkippedTrackHistory,
  SkippedTrackTotal,
  Track,
} from "modules/playlist/tabs/playlist-tabs-types";
import { DEFAULT_ALBUM_IMAGE } from "shared/constants";
import {
  databasePlaylists,
  spotifyPlaylists,
  skippedTracks,
  spotifyTracks as mockSpotifyTracks,
} from "shared/mock-data/api";
import { Image } from "shared/types";

const image: Image = {
  height:
    spotifyPlaylists.find((x) => x.id === databasePlaylists[0].id)?.images[0]
      ?.height || 100,
  width:
    spotifyPlaylists.find((x) => x.id === databasePlaylists[0].id)?.images[0]
      ?.width || 100,
  url:
    spotifyPlaylists.find((x) => x.id === databasePlaylists[0].id)?.images[0]
      ?.url || "href",
};

export const playlist: DetailedPlaylist = {
  id: databasePlaylists[0].id,
  href:
    spotifyPlaylists.find((x) => x.id === databasePlaylists[0].id)?.href || "",
  name:
    spotifyPlaylists.find((x) => x.id === databasePlaylists[0].id)?.name || "",
  image: image,
  ignoreInitialSkips: databasePlaylists[0].ignoreInitialSkips,
  autoCleanupLimit: databasePlaylists[0].autoCleanupLimit,
  skipThreshold: databasePlaylists[0].skipThreshold,
};

export const skippedTrackTotals: SkippedTrackTotal[] = [
  {
    id: skippedTracks[0].trackId,
    name: skippedTracks[0].name,
    duration: skippedTracks[0].duration,
    skippedTotal: 3,
    album: skippedTracks[0].album,
    artists: skippedTracks[0].artists,
    image: skippedTracks[0].album.images.reduce((prev, curr) => {
      return prev.height < curr.height ? prev : curr;
    }),
  },
  {
    id: skippedTracks[3].trackId,
    name: skippedTracks[3].name,
    duration: skippedTracks[3].duration,
    skippedTotal: 2,
    album: skippedTracks[3].album,
    artists: skippedTracks[3].artists,
    image: skippedTracks[3].album.images.reduce((prev, curr) => {
      return prev.height < curr.height ? prev : curr;
    }),
  },
];

export const skippedTrackHistory: SkippedTrackHistory[] = skippedTracks.map(
  (skippedTrack) => ({
    id: skippedTrack.trackId,
    name: skippedTrack.name,
    duration: skippedTrack.duration,
    skippedDate: new Date(skippedTrack.skippedDate),
    album: skippedTrack.album,
    artists: skippedTrack.artists,
    image: skippedTrack.album.images.reduce((prev, curr) => {
      return prev.height < curr.height ? prev : curr;
    }),
  })
);

export const spotifyTracks: Track[] = mockSpotifyTracks.map((spotifytrack) => {
  let image: Image = {
    height: 40,
    width: 40,
    url: DEFAULT_ALBUM_IMAGE,
  };

  if (spotifytrack.album.images.length > 0) {
    image = spotifytrack.album.images.reduce((prev, curr) => {
      return prev.height < curr.height ? prev : curr;
    });
  }

  return {
    id: spotifytrack.id,
    name: spotifytrack.name,
    duration: spotifytrack.duration,
    album: spotifytrack.album,
    artists: spotifytrack.artists,
    image,
  };
});
