import { TableBody } from "./table-body";
import { TableHead } from "./table-head";
import { useSortableTable } from "./hooks/use-sortable-table";
import { Column } from "./table-types";

type TableProps = {
  caption: string;
  data: any[];
  columns: Column[];
};

export const Table = ({ caption, data, columns }: TableProps) => {
  const [tableData, handleSorting] = useSortableTable(data, columns);

  return (
    <>
      <table className="table">
        {caption && <caption>{caption}</caption>}
        <TableHead {...{ columns, handleSorting }} />
        <TableBody {...{ columns, tableData }} />
      </table>
    </>
  );
};
