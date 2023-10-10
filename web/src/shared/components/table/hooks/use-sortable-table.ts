import { useState } from "react";
import { Column, SortOrder } from "../table-types";

function getDefaultSorting(defaultTableData: any, columns: Column[]) {
  const sorted = [...defaultTableData].sort((a, b) => {
    const filterColumn = columns.filter((column: any) => column.sortbyOrder);

    // Merge all array objects into single object and extract accessor and sortbyOrder keys
    let { accessor = "id", sortbyOrder = "asc" } = Object.assign(
      {},
      ...filterColumn
    );

    if (a[accessor] === null) return 1;
    if (b[accessor] === null) return -1;
    if (a[accessor] === null && b[accessor] === null) return 0;

    const ascending = a[accessor]
      .toString()
      .localeCompare(b[accessor].toString(), "en", {
        numeric: true,
      });

    return sortbyOrder === "asc" ? ascending : -ascending;
  });
  return sorted;
}

export const useSortableTable = (
  data: any,
  columns: Column[]
): [any[], (sortField: string, sortOrder: SortOrder) => void] => {
  const [tableData, setTableData] = useState(getDefaultSorting(data, columns));

  const handleSorting = (sortField: string, sortOrder: SortOrder) => {
    if (sortField) {
      const sorted = [...tableData].sort((a, b) => {
        if (a[sortField] === null) {
          return 1;
        }
        if (b[sortField] === null) {
          return -1;
        }
        if (a[sortField] === null && b[sortField] === null) {
          return 0;
        }

        if (a[sortField] instanceof Date && b[sortField] instanceof Date){
          var aDate: Date = a[sortField];
          var bDate: Date = b[sortField];
          return (
            (aDate > bDate ? 1 : -1) * (sortOrder === "asc" ? 1 : -1)
          );
        }

        return (
          a[sortField].toString().localeCompare(b[sortField].toString(), "en", {
            numeric: true,
          }) * (sortOrder === "asc" ? 1 : -1)
        );
      });
      setTableData(sorted);
    }
  };

  return [tableData, handleSorting];
};
