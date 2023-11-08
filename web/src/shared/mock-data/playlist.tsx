import format from "date-fns/format";
import { DetailedPlaylist } from "modules/playlist/playlist-types";
import {
  SkippedTrackAlbum,
  SkippedTrackArtist,
  SkippedTrackHistory,
  SkippedTrackTotal,
  Track,
} from "modules/playlist/tabs/playlist-tabs-types";
import { PiClock } from "react-icons/pi";
import { VscTrash } from "react-icons/vsc";
import { Skeleton, SkeletonTheme } from "shared/components/skeleton";
import { Column } from "shared/components/table";
import { SecondaryText } from "shared/components/typography";
import {
  DEFAULT_ALBUM_IMAGE,
  SKELETON_HIGHLIGHT,
  SKELETON_PLAYLIST_BASE,
} from "shared/constants";
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

const skeleton = (
  <SkeletonTheme
    baseColor={SKELETON_PLAYLIST_BASE}
    highlightColor={SKELETON_HIGHLIGHT}
    height="100%"
  >
    <Skeleton />
  </SkeletonTheme>
);

export const imageColumn: Column = {
  label: "",
  accessor: "image",
  primary: true,
  render: (image: Image, loading?: boolean) => (
    <div className="centered">
      {loading ? (
        <div style={{ height: 40, width: 40 }}>{skeleton}</div>
      ) : (
        <img alt="" width={40} height={40} src={image.url} />
      )}
    </div>
  ),
};

export const titleColumn: Column = {
  label: "Title",
  accessor: "name",
  sortable: true,
  primary: true,
  render: (value: string, loading?: boolean) => (loading ? skeleton : value),
};

export const artistColumn: Column = {
  label: "Artist",
  accessor: "artists",
  sortable: true,
  render: (artists: SkippedTrackArtist[], loading?: boolean) => (
    <>
      {loading
        ? skeleton
        : artists.map((artist, index) => (
            <>
              {`${index === 0 ? "" : ", "}`}
              {!!artist.href ? (
                <a href={artist.href} target="_blank" rel="noreferrer">
                  {artist.name}
                </a>
              ) : (
                <SecondaryText>{artist.name}</SecondaryText>
              )}
            </>
          ))}
    </>
  ),
};

export const albumColumn: Column = {
  label: "Album",
  accessor: "album",
  sortable: true,
  render: (album: SkippedTrackAlbum, loading?: boolean) =>
    loading ? (
      skeleton
    ) : !!album.href ? (
      <a href={album.href} target="_blank" rel="noreferrer">
        {album.name}
      </a>
    ) : (
      <SecondaryText>{album.name}</SecondaryText>
    ),
};

export const durationColumn: Column = {
  label: <PiClock />,
  accessor: "duration",
  sortable: true,
  render: (millis: number, loading?: boolean) => {
    if (loading) {
      return skeleton;
    }
    var minutes = Math.floor(millis / 60000);
    var seconds = parseInt(((millis % 60000) / 1000).toFixed(0));
    return seconds === 60
      ? minutes + 1 + ":00"
      : minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
  },
};

export const totalSkippedColumn: Column = {
  label: "Date Skipped",
  accessor: "skippedDate",
  sortable: true,
  sortbyOrder: "desc",
  render: (date: Date, loading?: boolean) =>
    loading
      ? skeleton
      : format(date, "MMM dd yyyy") + " at " + format(date, "HH:mm:ss"),
};

export const dateSkippedColumn: Column = {
  label: "Total Skipped",
  accessor: "skippedTotal",
  sortable: true,
  sortbyOrder: "desc",
  render: (value: string, loading?: boolean) => (loading ? skeleton : value),
};

export const getDeleteColumn = (onDeleteClick: (id: string) => void) => ({
  label: "",
  accessor: "id",
  render: (id: string, loading?: boolean) =>
    loading ? (
      <></>
    ) : (
      <div className="centered">
        <VscTrash
          style={{ cursor: "pointer" }}
          onClick={() => onDeleteClick(id)}
        />
      </div>
    ),
});
