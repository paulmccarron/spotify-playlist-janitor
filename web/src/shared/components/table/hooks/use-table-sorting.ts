import { useState } from "react";
import { Column, SortOrder } from "../table-types";

type useTableSortingProps = {
  data: any[];
  columns: Column[];
};

const sortCompare = (
  valueA: string | number | Date,
  valueB: string | number | Date,
  sortOrder: SortOrder
) => {
  if (valueA === null) {
    return 1;
  }
  if (valueB === null) {
    return -1;
  }
  if (valueA === null && valueB === null) {
    return 0;
  }

  if (valueA instanceof Date && valueB instanceof Date) {
    var aDate: Date = valueA;
    var bDate: Date = valueB;
    return (aDate > bDate ? 1 : -1) * (sortOrder === "asc" ? 1 : -1);
  }

  return (
    valueA.toString().localeCompare(valueB.toString(), "en", {
      numeric: true,
    }) * (sortOrder === "asc" ? 1 : -1)
  );
};

const getDefaultSorting = (defaultTableData: any[], columns: Column[]) => {
  const filterColumn = columns.find((column) => column.sortbyOrder);
  if (filterColumn) {
    return [...defaultTableData].sort((a, b) => {
      let sortField = filterColumn?.accessor || "id";
      let sortbyOrder = filterColumn?.sortbyOrder || "asc";

      return sortCompare(a[sortField], b[sortField], sortbyOrder);
    });
  }

  return defaultTableData;
};

export const useTableSorting = ({
  data,
  columns,
}: useTableSortingProps): [
  any[],
  (sortField: string, sortOrder: SortOrder) => void
] => {
  const [tableData, setTableData] = useState(getDefaultSorting(data, columns));

  const handleSorting = (sortField: string, sortOrder: SortOrder) => {
    if (sortField) {
      const sorted = [...tableData].sort((a, b) => {
        return sortCompare(a[sortField], b[sortField], sortOrder);
      });
      setTableData(sorted);
    }
  };

  return [tableData, handleSorting];
};
