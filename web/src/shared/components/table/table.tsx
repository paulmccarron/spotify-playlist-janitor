import { TableBody } from "./table-body";
import { TableHead } from "./table-head";
import { useSortableTable } from "./hooks/use-sortable-table";
import { Column } from "./table-types";
import styled from "styled-components";

type TableProps = {
  caption: string;
  data: any[];
  columns: Column[];
};

const TableComponent = styled.table`
  border-collapse: collapse;
  width: 100%;

  th {
    border: none !important;
    border-bottom: 1px solid #121212 !important;
    min-width: 34px;
    text-align: left;
    color: white;
  }

  tbody tr:nth-child(even) {
    background-color: #121212;
  }

  tbody tr:hover {
    background-color: #232323;
  }

  td {
    border: none !important;
    padding: 8px;
  }
`;

export const Table = ({ caption, data, columns }: TableProps) => {
  const [tableData, handleSorting] = useSortableTable(data, columns);

  return (
    <TableComponent>
      {caption && <caption>{caption}</caption>}
      <TableHead {...{ columns, handleSorting }} />
      <TableBody {...{ columns, tableData }} />
    </TableComponent>
  );
};

Table.displayName = "Table";
