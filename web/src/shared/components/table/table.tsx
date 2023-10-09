import {TableBody} from "./table-body";
import {TableHead} from "./table-head";
import { useSortableTable } from "./hooks/use-sortable-table";

type AppTableProps = {
    caption: string;
    data: any[];
    columns: any[];
}

export const AppTable = ({ caption, data, columns }: AppTableProps) => {
  const [tableData, handleSorting] = useSortableTable(data, columns);

  return (
    <>
      <table className="table">
        <caption>{caption}</caption>
        <TableHead {...{ columns, handleSorting }} />
        <TableBody {...{ columns, tableData }} />
      </table>
    </>
  );
};