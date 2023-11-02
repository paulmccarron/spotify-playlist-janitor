import styled from "styled-components";

import { BACKGROUND, TABLE_HOVER, WHITE } from "shared/constants";

import { TableBody } from "./table-body";
import { TableHead } from "./table-head";
import { useTableSorting } from "./hooks/use-table-sorting";
import { Column } from "./table-types";

type TableProps = {
  caption?: string;
  data: any[];
  columns: Column[];
  loading?: boolean;
};

const TableComponent = styled.table`
  color: ${WHITE};
  border-collapse: collapse;
  width: 100%;

  th {
    border: none !important;
    border-bottom: 1px solid #121212 !important;
    min-width: 34px;
    text-align: left;
    padding: 8px;
  }

  tbody tr:nth-child(even) {
    background-color: ${BACKGROUND};
  }

  tbody tr:hover {
    background-color: ${TABLE_HOVER};
  }

  td {
    border: none !important;
    padding: 8px;
  }

  .centered {
    display: flex;
    justify-content: center;
  }
`;

export const Table = ({ caption, data, columns, loading }: TableProps) => {
  const [tableData, handleSorting] = useTableSorting({ data, columns });

  return (
    <TableComponent>
      {caption && <caption>{caption}</caption>}
      <TableHead {...{ columns, handleSorting }} />
      <TableBody {...{ columns, tableData, loading }} />
    </TableComponent>
  );
};

Table.displayName = "Table";
