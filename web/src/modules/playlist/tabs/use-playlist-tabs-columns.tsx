import { PiClock } from "react-icons/pi";
import { VscTrash } from "react-icons/vsc";
import format from "date-fns/format";
import { Column } from "shared/components/table";
import { SkippedTrackAlbum, SkippedTrackArtist } from "./playlist-tabs-types";
import { Image } from "shared/types";

type UsePlaylistTabsColumnsProps = {
  onDeleteClick(id: string): void;
};

export const usePlaylistTabsColumns = ({
  onDeleteClick,
}: UsePlaylistTabsColumnsProps) => {
  const skippedTrackColumns: Column[] = [
    {
      label: "",
      accessor: "image",
      primary: true,
      render: (image: Image) => (
        <div className="centered">
          <img alt="" width={40} height={40} src={image.url} />
        </div>
      ),
    },
    {
      label: "Title",
      accessor: "name",
      sortable: true,
      primary: true,
    },
    {
      label: "Artist",
      accessor: "artists",
      sortable: true,
      render: (artists: SkippedTrackArtist[]) => (
        <>
          {artists.map((artist, index) => (
            <>
              {`${index === 0 ? "" : ",,,,,"}`}
              {artist.name}
            </>
          ))}
        </>
      ),
    },
    {
      label: "Album",
      accessor: "album",
      sortable: true,
      render: (album: SkippedTrackAlbum) => <>{album.name}</>,
    },
    {
      label: <PiClock />,
      accessor: "duration",
      sortable: true,
      render: (millis: number) => {
        var minutes = Math.floor(millis / 60000);
        var seconds = parseInt(((millis % 60000) / 1000).toFixed(0));
        return seconds === 60
          ? minutes + 1 + ":00"
          : minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
      },
    },
    {
      label: "Total Skipped",
      accessor: "skippedTotal",
      sortable: true,
      sortbyOrder: "desc",
    },
    {
      label: "",
      accessor: "id",
      render: (id: string) => (
        <div className="centered">
          <VscTrash
            style={{ cursor: "pointer" }}
            onClick={() => onDeleteClick(id)}
          />
        </div>
      ),
    },
  ];

  const skippedTrackHistoryColumns: Column[] = [
    {
      label: "",
      accessor: "image",
      primary: true,
      render: (image: Image) => (
        <div className="centered">
          <img alt="" width={40} height={40} src={image.url} />
        </div>
      ),
    },
    {
      label: "Title",
      accessor: "name",
      sortable: true,
      primary: true,
    },
    {
      label: "Artists",
      accessor: "artists",
      sortable: true,
      render: (artists: SkippedTrackArtist[]) => (
        <>
          {artists.map((artist) => (
            <>{artist.name}</>
          ))}
        </>
      ),
    },
    {
      label: "Album",
      accessor: "album",
      sortable: true,
      render: (album: SkippedTrackAlbum) => <>{album.name}</>,
    },
    {
      label: <PiClock />,
      accessor: "duration",
      sortable: true,
      render: (millis: number) => {
        var minutes = Math.floor(millis / 60000);
        var seconds = parseInt(((millis % 60000) / 1000).toFixed(0));
        return seconds === 60
          ? minutes + 1 + ":00"
          : minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
      },
    },
    {
      label: "Date Skipped",
      accessor: "skippedDate",
      sortable: true,
      sortbyOrder: "desc",
      render: (date: Date) =>
        format(date, "MMM dd yyyy") + " at " + format(date, "HH:mm:ss"),
    },
    {
      label: "",
      accessor: "id",
      render: (id: string) => (
        <div className="centered">
          <VscTrash
            style={{ cursor: "pointer" }}
            onClick={() => onDeleteClick(id)}
          />
        </div>
      ),
    },
  ];

  const spotifyTrackColumns: Column[] = [
    {
      label: "",
      accessor: "image",
      primary: true,
      render: (image: Image) => (
        <div className="centered">
          <img alt="" width={40} height={40} src={image.url} />
        </div>
      ),
    },
    {
      label: "Title",
      accessor: "name",
      sortable: true,
      primary: true,
    },
    {
      label: "Artist",
      accessor: "artists",
      sortable: true,
      sortbyOrder: "asc",
      render: (artists: SkippedTrackArtist[]) => (
        <>
          {artists.map((artist) => (
            <>{artist.name}</>
          ))}
        </>
      ),
    },
    {
      label: "Album",
      accessor: "album",
      sortable: true,
      render: (album: SkippedTrackAlbum) => <>{album.name}</>,
    },
    {
      label: <PiClock />,
      accessor: "duration",
      sortable: true,
      render: (millis: number) => {
        var minutes = Math.floor(millis / 60000);
        var seconds = parseInt(((millis % 60000) / 1000).toFixed(0));
        return seconds === 60
          ? minutes + 1 + ":00"
          : minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
      },
    },
    {
      label: "",
      accessor: "id",
      render: (id: string) => (
        <div className="centered">
          <VscTrash
            style={{ cursor: "pointer" }}
            onClick={() => onDeleteClick(id)}
          />
        </div>
      ),
    },
  ];

  return {
    skippedTrackColumns,
    skippedTrackHistoryColumns,
    spotifyTrackColumns,
  };
};
