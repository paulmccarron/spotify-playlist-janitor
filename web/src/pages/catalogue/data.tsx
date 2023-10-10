import { Column } from "../../shared/components/table";
import { PiClock } from "react-icons/pi";
import { VscTrash } from "react-icons/vsc";

export const selectOptions = [
  { label: "Option 1", value: "option1" },
  { label: "Option 2", value: "option2" },
  { label: "Option 3", value: "option3" },
];

export const tableColumns: Column[] = [
  {
    label: "",
    accessor: "image",
    sortable: false,
    render: (url: string) => <img alt="" width={40} height={40} src={url} />,
  },
  { label: "Title", accessor: "title", sortable: true, sortbyOrder: "desc" },
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
      <VscTrash onClick={() => alert(`Deleted track id: ${id}`)} />
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
    date_skipped: new Date(2023, 9, 15, 15, 30, 45, 25),
    delete: undefined,
  },
];
