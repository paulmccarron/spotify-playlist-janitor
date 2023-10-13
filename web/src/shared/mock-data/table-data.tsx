import { Column } from "../components/table";
import { PiClock } from "react-icons/pi";
import { VscTrash } from "react-icons/vsc";

export const tableColumns: Column[] = [
  {
    label: "",
    accessor: "image",
    sortable: false,
    primary: true,
    render: (url: string) => <img alt="" width={40} height={40} src={url} />,
  },
  {
    label: "Title",
    accessor: "title",
    sortable: true,
    sortbyOrder: "desc",
    primary: true,
  },
  { label: "Artist", accessor: "artist", sortable: true },
  { label: "Album", accessor: "album", sortable: true },
  {
    label: <PiClock />,
    accessor: "length",
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
    accessor: "date_skipped",
    sortable: true,
    render: (date: Date) => date.toISOString(),
  },
  {
    label: "",
    accessor: "delete",
    sortable: false,
    render: (id: string) => (
      <VscTrash
        style={{ cursor: "pointer" }}
        onClick={() => alert(`Deleted track id: ${id}`)}
      />
    ),
  },
];

export const tableData = [
  {
    id: 1,
    image: "https://i.scdn.co/image/ab67616d00004851835a9e77dae1c928f871ac73",
    title: "Longshot",
    artist: "Catfish and the Bottlemen",
    album: "The Balance",
    length: 232960,
    date_skipped: new Date(2023, 9, 15, 15, 30, 45, 20),
    delete: undefined,
  },
  {
    id: 2,
    image: "https://i.scdn.co/image/ab67616d0000485158406b3f1ac3ceaff7a64fef",
    title: "Dark Necessities",
    artist: "Red Hot Chili Peppers",
    album: "The Getaway",
    length: 302000,
    date_skipped: new Date(2023, 9, 15, 15, 30, 46, 25),
    delete: undefined,
  },
  {
    id: 3,
    image: "https://i.scdn.co/image/ab67616d00004851d86a1e021e7acc7c07c7d668",
    title: "Live Forever",
    artist: "Oasis",
    album: "Definitely Maybe",
    length: 302000,
    date_skipped: new Date(2023, 9, 15, 15, 30, 47, 30),
    delete: undefined,
  },
  {
    id: 4,
    image: "https://i.scdn.co/image/ab67616d00004851176ca6e2a6efb0e3aaf4e37a",
    title: "Touch It / Technologic",
    artist: "Daft Punk",
    album: "Alive 2007",
    length: 276666,
    date_skipped: new Date(2023, 9, 15, 15, 30, 47, 40),
    delete: undefined,
  },
];
