import { PiClock } from "react-icons/pi";
import { VscTrash } from "react-icons/vsc";
import format from "date-fns/format";
import { Column } from "shared/components/table";
import { SkippedTrackAlbum, SkippedTrackArtist } from "./playlist-tabs-types";
import { Image } from "shared/types";
import { useMemo } from "react";
import styled from "styled-components";
import { SKELETON_HIGHLIGHT, SKELETON_PLAYLIST_BASE, WHITE } from "shared/constants";
import { Skeleton, SkeletonTheme } from "shared/components/skeleton";

const Link = styled.a`
  &:link {
    color: ${WHITE};
    text-decoration: none;
  }

  &:visited {
    color: ${WHITE};
  }

  &:hover {
    color: ${WHITE};
    text-decoration: underline;
  }

  &:active {
    color: ${WHITE};
  }
`;

type UsePlaylistTabsColumnsProps = {
  onDeleteClick(id: string): void;
};

const skeleton = <SkeletonTheme
  baseColor={SKELETON_PLAYLIST_BASE}
  highlightColor={SKELETON_HIGHLIGHT}
  height="100%"
>
  <Skeleton />
</SkeletonTheme>;

const imageColumn: Column = {
  label: "",
  accessor: "image",
  primary: true,
  render: (image: Image, loading?: boolean) => (
    <div className="centered">
      {loading ?
        <div style={{ height: 40, width: 40 }}>{skeleton}</div>
        :
        <img alt="" width={40} height={40} src={image.url} />}
    </div>
  ),
};

const titleColumn: Column = {
  label: "Title",
  accessor: "name",
  sortable: true,
  primary: true,
  render: (value: string, loading?: boolean) => (
    loading ? skeleton : value
  ),
};

const artistColumn: Column = {
  label: "Artist",
  accessor: "artists",
  sortable: true,
  render: (artists: SkippedTrackArtist[], loading?: boolean) => (
    <>
      {loading ?
        skeleton
        :
        artists.map((artist, index) => (
          <>
            {`${index === 0 ? "" : ", "}`}
            <Link
              href={artist.href}
              target="_blank"
              rel="noreferrer"
            >
              {artist.name}
            </Link>
          </>
        ))}
    </>
  ),
};

const albumColumn: Column = {
  label: "Album",
  accessor: "album",
  sortable: true,
  render: (album: SkippedTrackAlbum, loading?: boolean) => (
    loading ? skeleton : <Link
      href={album.href}
      target="_blank"
      rel="noreferrer"
    >
      {album.name}
    </Link>
  ),
};

const durationColumn: Column = {
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

export const usePlaylistTabsColumns = ({
  onDeleteClick,
}: UsePlaylistTabsColumnsProps) => {

  const deleteColumn = useMemo(() => ({
    label: "",
    accessor: "id",
    render: (id: string, loading?: boolean) => (
      loading ?
        <></>
        :
        <div className="centered">
          <VscTrash
            style={{ cursor: "pointer" }}
            onClick={() => onDeleteClick(id)}
          />
        </div>
    ),
  }), [onDeleteClick])

  const skippedTrackColumns: Column[] = [
    imageColumn,
    titleColumn,
    artistColumn,
    albumColumn,
    durationColumn,
    {
      label: "Total Skipped",
      accessor: "skippedTotal",
      sortable: true,
      sortbyOrder: "desc",
      render: (value: string, loading?: boolean) => (
        loading ? skeleton : value
      ),
    },
    deleteColumn,
  ];

  const skippedTrackHistoryColumns: Column[] = [
    imageColumn,
    titleColumn,
    artistColumn,
    albumColumn,
    durationColumn,
    {
      label: "Date Skipped",
      accessor: "skippedDate",
      sortable: true,
      sortbyOrder: "desc",
      render: (date: Date, loading?: boolean) =>
        loading ? skeleton : format(date, "MMM dd yyyy") + " at " + format(date, "HH:mm:ss"),
    },
    deleteColumn,
  ];

  const spotifyTrackColumns: Column[] = [
    imageColumn,
    titleColumn,
    { ...artistColumn, sortbyOrder: "asc", },
    albumColumn,
    durationColumn,
    deleteColumn,
  ];

  return {
    skippedTrackColumns,
    skippedTrackHistoryColumns,
    spotifyTrackColumns,
  };
};
