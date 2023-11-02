import { useEffect, useState } from "react";
import { Column, SortOrder } from "../table-types";

type useTableSortingProps = {
  data: any[];
  columns: Column[];
  loading?: boolean;
};

const sortCompare = (
  valueA: string | number | Date | any | any[],
  valueB: string | number | Date | any | any[],
  sortOrder: SortOrder
) => {
  if (valueA === null || valueA === undefined) {
    return 1;
  }
  if (valueB === null || valueB === undefined) {
    return -1;
  }
  if ((valueA === null && valueB === null) || (valueA === undefined && valueB === undefined)) {
    return 0;
  }

  if (valueA instanceof Date && valueB instanceof Date) {
    var aDate: Date = valueA;
    var bDate: Date = valueB;
    return (aDate > bDate ? 1 : -1) * (sortOrder === "asc" ? 1 : -1);
  }

  if (
    (typeof valueA === "string" && typeof valueB === "string") ||
    (typeof valueA === "number" && typeof valueB === "number")
  ) {
    return (
      valueA.toString().localeCompare(valueB.toString(), "en", {
        numeric: true,
      }) * (sortOrder === "asc" ? 1 : -1)
    );
  }

  if (typeof valueA !== "string" && typeof valueB !== "string") {
    if ("name" in valueA && "name" in valueA) {
      return (
        valueA.name.toString().localeCompare(valueB.name.toString(), "en", {
          numeric: true,
        }) * (sortOrder === "asc" ? 1 : -1)
      );
    } else if (valueA instanceof Array && valueB instanceof Array) {
      const valueANames = valueA
        .map((valueAValue) =>
          "name" in valueAValue
            ? (valueAValue.name as string).toLowerCase()
            : ""
        )
        .filter(Boolean)
        .join(",");

      const valueBNames = valueB
        .map((valueBValue) =>
          "name" in valueBValue
            ? (valueBValue.name as string).toLowerCase()
            : ""
        )
        .filter(Boolean)
        .join(",");

      return (
        valueANames.toString().localeCompare(valueBNames.toString(), "en", {
          numeric: true,
        }) * (sortOrder === "asc" ? 1 : -1)
      );
    }
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

  useEffect(() => {
    setTableData(getDefaultSorting(data, columns))
  }, [setTableData, data, columns])

  return [tableData, handleSorting];
};
